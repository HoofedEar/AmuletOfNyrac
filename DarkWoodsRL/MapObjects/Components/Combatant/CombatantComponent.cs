using System;
using System.Linq;
using DarkWoodsRL.MapObjects.Components.EnemyAI;
using DarkWoodsRL.MapObjects.Components.Interfaces;
using DarkWoodsRL.Themes;
using GoRogue.DiceNotation;
using GoRogue.Random;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Combatant;

/// <summary>
/// Component for entities that allows them to engage in combat.
/// Provides them with Base Stats and Health.
/// </summary>
/// <remarks>
/// Strength determines how much damage a combatant does when they successfully hit something.
/// Endurance determines how much damage is resisted when they take damage.
/// Dexterity determines how often they are successful when attempting to hit something.
/// </remarks>
internal class CombatantComponent : RogueLikeComponentBase<RogueLikeEntity>, IBumpable
{
    private int _hp;
    private int _str;
    private int _end;
    private int _dex;

    public int STR
    {
        get => _str;
        set
        {
            _str = value;
            STRChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? STRChanged; // Typically when the player levels up.

    public int END
    {
        get => _end;
        set
        {
            _end = value;
            ENDChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? ENDChanged; // Typically when the player levels up.

    public int DEX
    {
        get => _dex;
        set
        {
            _dex = value;
            DEXChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? DEXChanged; // Typically when the player levels up.

    public int HP
    {
        get => _hp;
        private set
        {
            if (_hp == value) return;

            _hp = Math.Min(Math.Max(0, value), MaxHP);
            HPChanged?.Invoke(this, EventArgs.Empty);

            if (_hp == 0)
                Died?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? HPChanged;
    public event EventHandler? Died;

    public int MaxHP { get; private set; }
    private readonly string _combatVerb;

    public int Xp
    {
        get => _xp;
        set
        {
            _xp = value;
            XpChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? XpChanged;

    private int _xp; // Current XP
    private int _lvl; // Current LVL
    private int _nextXp; // Required XP for next LVL
    public readonly int ProvidedXp; // XP given when killed
    public RogueLikeEntity? LastHit;

    public CombatantComponent(int hp, int endurance, int strength, int dexterity = 3, string combatVerb = "swings at",
        int xp = 10)
        : base(false, false, false, false)
    {
        HP = MaxHP = hp;
        END = endurance;
        STR = strength;
        DEX = dexterity;
        _combatVerb = combatVerb;
        ProvidedXp = xp;
        _lvl = 1;
        _nextXp = (int) Math.Pow(5 * _lvl, 2);
        XpChanged += CheckXp;
    }

    private void CheckXp(object? sender, EventArgs e)
    {
        if (_xp < _nextXp) return;
        // Level Up!
        Engine.GameScreen?.MessageLog.AddMessage(new ColoredString(
            $"You leveled up!",
            MessageColors.HealthRecoveredAppearance));
        _lvl += 1;
        MaxHP += GlobalRandom.DefaultRNG.NextInt(2, 11);
        HPChanged?.Invoke(this, EventArgs.Empty);
        _nextXp = (int) Math.Pow(5 * _lvl, 2);
    }

    public int Heal(int amount)
    {
        amount = Math.Min(amount, MaxHP - HP);
        HP += amount;

        return amount;
    }

    /// <summary>
    /// Whenever something attacks something else, the attack is calculated as the result of
    /// 1d20 + 1 + any to hit modifiers. The defenders total defense is calculated as
    /// 20 - the attackers level - the defenders armor. If the attack is greater or equal to the defense, the attack succeeded.
    /// </summary>
    /// <param name="target"></param>
    private void Attack(CombatantComponent target)
    {
        var roll = Dice.Roll("1d20");
        var result = Parent == Engine.Player ? roll + DEX + 2 : roll + DEX;
        var atkTextColor = Parent == Engine.Player
            ? MessageColors.PlayerAtkAppearance
            : MessageColors.EnemyAtkAtkAppearance;
        var attackDesc = $"{Parent!.Name} {_combatVerb} {target.Parent!.Name}";
        if (Parent.AllComponents.Contains<IEnemyAI>() && target.Parent.AllComponents.Contains<IEnemyAI>()) return;

        if (result <= Dice.Roll("1d20") + target.END)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new ColoredString(
                $"{Parent!.Name} {_combatVerb} {target.Parent!.Name} but misses.",
                atkTextColor));

            target.LastHit = Parent;
            if (target.LastHit != Engine.Player) return;
            var aimless = target.Parent.AllComponents.GetFirstOrDefault<AimlessAI>();
            if (aimless is {IsAngry: false})
            {
                aimless.IsAngry = true;
            }

            return;
        }

        // Successful hit
        var damage = Dice.Roll("1d6") + STR;
        if (damage > 0)
        {
            var prefixWord = GeneratePrefixWord();
            Engine.GameScreen?.MessageLog.AddMessage(
                new ColoredString($"{prefixWord} {Parent!.Name} deals {damage} damage to {target.Parent!.Name}.",
                    atkTextColor));

            target.LastHit = Parent;

            target.HP -= damage;
            if (target.LastHit != Engine.Player) return;

            // Make AimlessAI angry
            var aimless = target.Parent.AllComponents.GetFirstOrDefault<AimlessAI>();
            if (aimless is {IsAngry: false})
            {
                aimless.IsAngry = true;
            }
        }
        else
        {
            Engine.GameScreen?.MessageLog.AddMessage(new ColoredString($"{attackDesc} but does no damage.",
                atkTextColor));


            target.LastHit = Parent;
            if (target.LastHit != Engine.Player) return;
            var aimless = Parent.AllComponents.GetFirstOrDefault<AimlessAI>();
            if (aimless is {IsAngry: false})
            {
                aimless.IsAngry = true;
            }
        }
    }

    private string GeneratePrefixWord()
    {
        return GlobalRandom.DefaultRNG.NextInt(0, 7) switch
        {
            0 => "BAM!",
            1 => "BOINK!",
            2 => "SMACK!",
            3 => "SPLAT!",
            4 => "KAPOW!",
            5 => "WHAM!",
            6 => "SLAM!",
            _ => "CRACK!"
        };
    }

    public bool OnBumped(RogueLikeEntity source)
    {
        var combatant = source.AllComponents.GetFirstOrDefault<CombatantComponent>();
        if (combatant == null) return false;

        combatant.Attack(this);
        return true;
    }
}
using System;
using DarkWoodsRL.Themes;
using GoRogue.DiceNotation;
using GoRogue.Random;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components;

/// <summary>
/// Component for entities that allows them to engage in combat.
/// Provides them with Base Stats and Health.
/// </summary>
/// <remarks>
/// Strength determines how much damage a combatant does when they successfully hit something.
/// Endurance determines how much damage is resisted when they take damage.
/// Dexterity determines how often they are successful when attempting to hit something.
/// </remarks>
internal class Combatant : RogueLikeComponentBase<RogueLikeEntity>, IBumpable
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

    public int MaxHP { get; }
    public string CombatVerb;

    public Combatant(int hp, int endurance, int strength, int dexterity = 3, string combatVerb = "swings at")
        : base(false, false, false, false)
    {
        HP = MaxHP = hp;
        END = endurance;
        STR = strength;
        DEX = dexterity;
        CombatVerb = combatVerb;
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
    private void Attack(Combatant target)
    {
        var roll = Dice.Roll("1d20");
        var result = Parent == Engine.Player ? roll + DEX + 2 : roll + DEX;
        var atkTextColor = Parent == Engine.Player
            ? MessageColors.PlayerAtkAppearance
            : MessageColors.EnemyAtkAtkAppearance;
        var attackDesc = $"{Parent!.Name} {CombatVerb} {target.Parent!.Name}";

        if (result <= Dice.Roll("1d20") + target.END)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new($"{Parent!.Name} {CombatVerb} {target.Parent!.Name} but misses.",
                atkTextColor));
            return;
        }

        // Successful hit
        var damage = STR - target.END;
        if (damage > 0)
        {
            var prefixWord = GeneratePrefixWord();
            Engine.GameScreen?.MessageLog.AddMessage(new($"{prefixWord} {Parent!.Name} deals {damage} damage to {target.Parent!.Name}.", atkTextColor));
            target.HP -= damage;
        }
        else
            Engine.GameScreen?.MessageLog.AddMessage(new($"{attackDesc} but does no damage.", atkTextColor));
    }

    private string GeneratePrefixWord()
    {
        return GlobalRandom.DefaultRNG.NextInt(0, 6) switch
        {
            0 => "BAM!",
            1 => "BOINK!",
            2 => "SMACK!",
            3 => "SPLAT!",
            4 => "KAPOW!",
            5 => "WHAM!",
            _ => "CRACK!"
        };
    }

    public bool OnBumped(RogueLikeEntity source)
    {
        var combatant = source.AllComponents.GetFirstOrDefault<Combatant>();
        if (combatant == null) return false;

        combatant.Attack(this);
        return true;
    }
}
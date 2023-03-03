using System;
using DarkWoodsRL.Themes;
using GoRogue.DiceNotation;
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
    private int _str; // Base Strength
    private int _end; // Base Endurance
    private int _dex; // Base Dexterity

    public int STR
    {
        get => _str;
        private set
        {
            if (_str == value) return;

            _str = value;
            STRChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? STRChanged; // Typically when the player levels up.

    public int END
    {
        get => _end;
        private set
        {
            if (_end == value) return;

            _end = value;
            ENDChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? ENDChanged; // Typically when the player levels up.

    public int DEX
    {
        get => _dex;
        private set
        {
            if (_dex == value) return;

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

    public int Strength { get; }
    public int Endurance { get; }
    public int Dexterity { get; }

    public Combatant(int hp, int endurance, int strength, int dexterity = 3)
        : base(false, false, false, false)
    {
        HP = MaxHP = hp;
        Endurance = endurance;
        Strength = strength;
        Dexterity = dexterity;
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
    public void Attack(Combatant target)
    {
        // TODO Combat math
        var roll = Dice.Roll("1d20");
        var result = roll + DEX;
        var atkTextColor = Parent == Engine.Player
            ? MessageColors.PlayerAtkAppearance
            : MessageColors.EnemyAtkAtkAppearance;
        string attackDesc = $"{Parent!.Name} attacks {target.Parent!.Name}";
        
        if (result <= Dice.Roll("1d20") + target.Endurance)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new($"{Parent!.Name} swings at {target.Parent!.Name} but misses.", atkTextColor));
            return;
        }
        
        // Successful hit
        int damage = Strength - target.Endurance;
        if (damage > 0)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new($"{attackDesc} for {damage} damage.", atkTextColor));
            target.HP -= damage;
        }
        else
            Engine.GameScreen?.MessageLog.AddMessage(new($"{attackDesc} but does no damage.", atkTextColor));
    }

    public bool OnBumped(RogueLikeEntity source)
    {
        var combatant = source.AllComponents.GetFirstOrDefault<Combatant>();
        if (combatant == null) return false;

        combatant.Attack(this);
        return true;
    }
}
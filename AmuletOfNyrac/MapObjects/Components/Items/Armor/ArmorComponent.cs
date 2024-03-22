using AmuletOfNyrac.Themes;
using AmuletOfNyrac.MapObjects.Components.Items.Weapon;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items.Armor;

public class ArmorComponent : RogueLikeComponentBase<RogueLikeEntity>, IArmor
{
    public bool IsEquipped;
    public int EnchantLvl = 0;
    public int ENDMod;

    public ArmorComponent(int end = 0) : base(false, false, false, false)
    {
        ENDMod = end;
    }

    public bool Equip()
    {
        if (IsEquipped) return false;
        if (Parent != null) Parent.Name += " (e)";
        IsEquipped = true;
        Engine.Player.AllComponents.GetFirst<Combatant.CombatantComponent>().END += ENDMod;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You donned the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;
    }

    public bool Unequip()
    {
        if (!IsEquipped) return false;
        if (Parent != null) Parent.Name = Parent.Name.Replace(" (e)", "");
        IsEquipped = false;
        Engine.Player.AllComponents.GetFirst<Combatant.CombatantComponent>().END -= ENDMod;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You removed the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;
    }
}
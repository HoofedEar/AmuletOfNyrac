using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using DarkWoodsRL.Themes;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items.Armor;

public class ArmorComponent : RogueLikeComponentBase<RogueLikeEntity>, IArmor
{
    public bool IsEquipped;

    public ArmorComponent() : base(false, false, false, false)
    {
    }

    public bool Equip(RogueLikeEntity user)
    {
        if (IsEquipped) return false;
        if (Parent != null) Parent.Name += " (e)";
        IsEquipped = true;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You donned the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;

    }

    public bool Unequip(RogueLikeEntity user)
    {
        if (!IsEquipped) return false;
        if (Parent != null) Parent.Name = Parent.Name.Replace(" (e)", "");
        IsEquipped = false;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You removed the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;
    }
}
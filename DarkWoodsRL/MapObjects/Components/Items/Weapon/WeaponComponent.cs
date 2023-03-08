using DarkWoodsRL.Themes;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items.Weapon;

public class WeaponComponent : RogueLikeComponentBase<RogueLikeEntity>, IWeapon
{
    public bool IsEquipped;
    public int STRMod;
    public int DEXMod;
    public WeaponComponent(int str = 0, int dex = 0) : base(false, false, false, false)
    {
        STRMod = str;
        DEXMod = dex;
    }

    public bool Equip()
    {
        if (IsEquipped)
        {
            Unequip();
            return true;
        }
        if (Parent != null) Parent.Name += " (e)";
        IsEquipped = true;
        Engine.Player.AllComponents.GetFirst<Combatant>().STR += STRMod;
        Engine.Player.AllComponents.GetFirst<Combatant>().DEX += DEXMod;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You wield the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;
    }

    public bool Unequip()
    {
        if (!IsEquipped) return false;
        if (Parent != null) Parent.Name = Parent.Name.Replace(" (e)", "");
        IsEquipped = false;
        Engine.Player.AllComponents.GetFirst<Combatant>().STR -= STRMod;
        Engine.Player.AllComponents.GetFirst<Combatant>().DEX -= DEXMod;
        Engine.GameScreen?.MessageLog.AddMessage(new(
            $"You put away the {Parent?.Name.Replace(" (e)", "")}.",
            MessageColors.ItemPickedUpAppearance));
        return true;
    }
}
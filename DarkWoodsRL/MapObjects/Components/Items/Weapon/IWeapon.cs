using SadRogue.Integration;

namespace DarkWoodsRL.MapObjects.Components.Items.Weapon;

internal interface IWeapon : ICarryable
{
    bool Equip();
    bool Unequip();
}
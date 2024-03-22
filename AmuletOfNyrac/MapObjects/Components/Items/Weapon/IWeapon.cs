using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;

namespace AmuletOfNyrac.MapObjects.Components.Items.Weapon;

internal interface IWeapon : ICarryable
{
    bool Equip();
    bool Unequip();
}
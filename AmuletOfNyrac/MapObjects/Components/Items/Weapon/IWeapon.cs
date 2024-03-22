using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using SadRogue.Integration;

namespace AmuletOfNyrac.MapObjects.Components.Items.Weapon;

internal interface IWeapon : ICarryable
{
    bool Equip();
    bool Unequip();
}
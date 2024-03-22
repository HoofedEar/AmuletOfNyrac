using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using SadRogue.Integration;

namespace AmuletOfNyrac.MapObjects.Components.Items.Armor;

internal interface IArmor : ICarryable
{
    bool Equip();
    bool Unequip();
}
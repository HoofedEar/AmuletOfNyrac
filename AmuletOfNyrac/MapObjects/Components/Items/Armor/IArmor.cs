using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;

namespace AmuletOfNyrac.MapObjects.Components.Items.Armor;

internal interface IArmor : ICarryable
{
    bool Equip();
    bool Unequip();
}
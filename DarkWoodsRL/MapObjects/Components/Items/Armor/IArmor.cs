using SadRogue.Integration;

namespace DarkWoodsRL.MapObjects.Components.Items.Armor;

internal interface IArmor : ICarryable
{
    bool Equip(RogueLikeEntity user);
    bool Unequip(RogueLikeEntity user);
}
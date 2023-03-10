using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Items;

/// <summary>
/// Simple class with some static functions for creating items.
/// </summary>
internal static class Other
{
    public static RogueLikeEntity Gold()
    {
        var gold = new RogueLikeEntity(Color.Gold, Color.Black, '*', layer: (int) GameMap.Layer.Items)
        {
            Name = "Gold"
        };
        gold.AllComponents.Add(new GoldComponent());

        return gold;
    }
    public static RogueLikeEntity Honeycomb()
    {
        var honeycomb = new RogueLikeEntity(Color.Goldenrod, Color.Black, '#', layer: (int)GameMap.Layer.Items)
        {
            Name = "Honeycomb"
        };
        honeycomb.AllComponents.Add(new HealingConsumable(4));
        honeycomb.AllComponents.Add(new DetailsComponent("Food", new []{"A delicious delicacy."}));

        return honeycomb;
    }
}
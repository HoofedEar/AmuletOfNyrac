using System.Linq;
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
internal static class Factory
{
    public static RogueLikeEntity HealthPotion()
    {
        var potion = new RogueLikeEntity(Color.Goldenrod, Color.Black, '#', layer: (int)GameMap.Layer.Items)
        {
            Name = "Honeycomb"
        };
        potion.AllComponents.Add(new HealingConsumable(4));

        return potion;
    }
    
    public static RogueLikeEntity Dagger()
    {
        var potion = new RogueLikeEntity(Color.Silver, Color.Black, ')', layer: (int)GameMap.Layer.Items)
        {
            Name = "Dagger"
        };
        potion.AllComponents.Add(new WeaponComponent());

        return potion;
    }
    
    public static RogueLikeEntity LeatherArmor()
    {
        var potion = new RogueLikeEntity(Color.SaddleBrown, Color.Black, ']', layer: (int)GameMap.Layer.Items)
        {
            Name = "LeatherArmor"
        };
        potion.AllComponents.Add(new ArmorComponent());

        return potion;
    }
}
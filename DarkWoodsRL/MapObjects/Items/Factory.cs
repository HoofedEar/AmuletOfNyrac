﻿using System.Linq;
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
    public static RogueLikeEntity Gold()
    {
        var gold = new RogueLikeEntity(Color.Gold, Color.Black, '*', layer: (int) GameMap.Layer.Items)
        {
            Name = "Gold"
        };
        gold.AllComponents.Add(new GoldComponent());

        return gold;
    }
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
        var potion = new RogueLikeEntity(Color.Silver, Color.Black, '/', layer: (int)GameMap.Layer.Items)
        {
            Name = "Dagger"
        };
        potion.AllComponents.Add(new WeaponComponent(10, 2));

        return potion;
    }
    
    public static RogueLikeEntity LeatherArmor()
    {
        var potion = new RogueLikeEntity(Color.SaddleBrown, Color.Black, ']', layer: (int)GameMap.Layer.Items)
        {
            Name = "LeatherArmor"
        };
        potion.AllComponents.Add(new ArmorComponent(10));

        return potion;
    }
}
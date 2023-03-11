using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.ItemDefinitions;

internal static class Weapons
{
    public static RogueLikeEntity PaperMachete()
    {
        var e = new RogueLikeEntity(Color.Silver, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Papier-Machete"
        };
        e.AllComponents.Add(new WeaponComponent(4, 3));
        e.AllComponents.Add(new DetailsComponent("Machete", new[]
        {
            "Just because this is made",
            "of paper with random words",
            "on it doesn't make it any",
            "less badass."
        }));
        return e;
    }
    
    public static RogueLikeEntity WoodenStick()
    {
        var e = new RogueLikeEntity(Color.SaddleBrown, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Wooden Stick"
        };
        e.AllComponents.Add(new WeaponComponent(2, 3));
        e.AllComponents.Add(new DetailsComponent("Stick", new[]
        {
            "Ol' reliable."
        }));
        return e;
    }
    
    // Mythic Weapon
    public static RogueLikeEntity BugleberrysDarkstaff()
    {
        var e = new RogueLikeEntity(Color.Crimson, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Bugleberry's Darkstaff"
        };
        e.AllComponents.Add(new WeaponComponent(10, 10));
        e.AllComponents.Add(new DetailsComponent("Mythic Staff", new[]
        {
            "B. F. Bugleberry imbued this",
            "staff with power of the Dark",
            "Arcanas. Use it wisely."
        }));
        return e;
    }
    
    public static RogueLikeEntity FleetwoodChain()
    {
        var e = new RogueLikeEntity(Color.LightSteelBlue, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Fleetwood Chain"
        };
        e.AllComponents.Add(new WeaponComponent(3, 2));
        e.AllComponents.Add(new DetailsComponent("Chain", new[]
        {
            "This chain has not yet been",
            "broken, and if you don't ",
            "love that now you'll never",
            "love it again."
        }));
        return e;
    }
}
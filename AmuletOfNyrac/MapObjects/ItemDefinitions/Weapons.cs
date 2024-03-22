using AmuletOfNyrac.MapObjects.Components.Items;
using AmuletOfNyrac.MapObjects.Components.Items.Weapon;
using AmuletOfNyrac.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace AmuletOfNyrac.MapObjects.ItemDefinitions;

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
    
    /// <summary>
    /// TODO rework this
    /// </summary>
    /// <returns></returns>
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
    
    public static RogueLikeEntity FlameLiberator()
    {
        var e = new RogueLikeEntity(Color.Firebrick, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Flame Liberator"
        };
        e.AllComponents.Add(new WeaponComponent(6, 6));
        e.AllComponents.Add(new DetailsComponent("Keyblade", new[]
        {
            "Helps form a blazing bond",
            "between you and your best",
            "friends. Especially if they",
            "have spikey hair."
        }));
        return e;
    }
    
    /// <summary>
    /// Can be enchanted up to +9
    /// </summary>
    /// <returns></returns>
    public static RogueLikeEntity DaedalusHammer()
    {
        var e = new RogueLikeEntity(Color.Firebrick, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "Daedalus Hammer"
        };
        e.AllComponents.Add(new WeaponComponent(6, 6));
        e.AllComponents.Add(new DetailsComponent("Hammer", new[]
        {
            ""
        }));
        return e;
    }
    
    // ReSharper disable once InconsistentNaming
    public static RogueLikeEntity NGGoldenSword()
    {
        var e = new RogueLikeEntity(Color.Goldenrod, Color.Black, '/', layer: (int) GameMap.Layer.Items)
        {
            Name = "NG Golden Sword"
        };
        e.AllComponents.Add(new WeaponComponent(5, 2));
        e.AllComponents.Add(new DetailsComponent("Sword", new[]
        {
            ""
        }));
        return e;
    }
}
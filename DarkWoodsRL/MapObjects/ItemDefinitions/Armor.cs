using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.ItemDefinitions;

public static class Armor
{
    public static RogueLikeEntity ChestBarrel()
    {
        var e = new RogueLikeEntity(Color.SaddleBrown, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Chest Barrel"
        };
        e.AllComponents.Add(new ArmorComponent(6));
        e.AllComponents.Add(new DetailsComponent("Barrel", new[]
        {
            "It has two arm holes in",
            "addition to its bung hole."
        }));
        return e;
    }

    public static RogueLikeEntity PunkRockJacket()
    {
        var e = new RogueLikeEntity(Color.DarkGray, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Punk Rock Jacket"
        };
        e.AllComponents.Add(new ArmorComponent(5));
        e.AllComponents.Add(new DetailsComponent("Jacket", new[]
        {
            "An assortment of sewn together",
            "strips of ripped-up T-shirts",
            "and random military patches."
        }));
        return e;
    }
    
    public static RogueLikeEntity SplinterArmor()
    {
        var e = new RogueLikeEntity(Color.SandyBrown, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Splinter Armor"
        };
        e.AllComponents.Add(new ArmorComponent(3));
        e.AllComponents.Add(new DetailsComponent("Armor", new[]
        {
            "Really crappy armor made",
            "from really crappy wood."
        }));
        return e;
    }
    
    public static RogueLikeEntity TyeDyeShirt()
    {
        var e = new RogueLikeEntity(Color.Yellow, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Tye Dye Shirt"
        };
        e.AllComponents.Add(new ArmorComponent(2));
        e.AllComponents.Add(new DetailsComponent("Shirt", new[]
        {
            "Trippy!"
        }));
        return e;
    }
    
    public static RogueLikeEntity TortoiseShell()
    {
        var e = new RogueLikeEntity(Color.Green, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Tortoise Shell"
        };
        e.AllComponents.Add(new ArmorComponent(7));
        e.AllComponents.Add(new DetailsComponent("Shell", new[]
        {
            "It shows no sign of a previous",
            "owner. It's free real estate."
        }));
        return e;
    }
    
    // Mythic Armor
    public static RogueLikeEntity RuneBodyplate()
    {
        var e = new RogueLikeEntity(Color.Teal, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Rune Bodyplate"
        };
        e.AllComponents.Add(new ArmorComponent(15));
        e.AllComponents.Add(new DetailsComponent("Mythic Armor", new[]
        {
            "Provides excellent protection.",
            "... or so I'm told."
        }));
        return e;
    }
}
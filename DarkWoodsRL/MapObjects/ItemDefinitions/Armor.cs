using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.ItemDefinitions;

public static class Armor
{
    //An assortment of sewn-together strips of ripped-up T-shirts and random military patches
    public static RogueLikeEntity ChestBarrel()
    {
        var e = new RogueLikeEntity(Color.SandyBrown, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Chest Barrel"
        };
        e.AllComponents.Add(new ArmorComponent(4));
        e.AllComponents.Add(new DetailsComponent("Armor", new[]
        {
            "This barrel has two arm",
            "holes in addition to its", "bung hole."
        }));
        return e;
    }

    public static RogueLikeEntity PunkRockJacket()
    {
        var e = new RogueLikeEntity(Color.SandyBrown, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "Punk Rock Jacket"
        };
        e.AllComponents.Add(new ArmorComponent(2));
        e.AllComponents.Add(new DetailsComponent("Armor", new[]
        {
            "An assortment of sewn",
            "-together strips of ripped-up",
            "T-shirts and random military patches."
        }));
        return e;
    }
}
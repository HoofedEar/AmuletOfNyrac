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
        e.AllComponents.Add(new WeaponComponent(2, 2));
        e.AllComponents.Add(new DetailsComponent("Weapon", new[] {""}));
        return e;
    }

    public static RogueLikeEntity LeatherArmor()
    {
        var e = new RogueLikeEntity(Color.SaddleBrown, Color.Black, ']', layer: (int) GameMap.Layer.Items)
        {
            Name = "LeatherArmor"
        };
        e.AllComponents.Add(new ArmorComponent(10));
        e.AllComponents.Add(new DetailsComponent("Armor", new[] {"Comfy boi."}));
        return e;
    }
}
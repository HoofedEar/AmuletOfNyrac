using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.ItemDefinitions;

/// <summary>
/// Simple class with some static functions for creating items.
/// </summary>
internal static class Other
{
    /// <summary>
    /// Gold, used for scoring purposes
    /// </summary>
    /// <returns></returns>
    public static RogueLikeEntity Gold()
    {
        var e = new RogueLikeEntity(Color.Gold, Color.Black, '*', layer: (int) GameMap.Layer.Items)
        {
            Name = "Gold"
        };
        e.AllComponents.Add(new GoldComponent());
        return e;
    }

    public static RogueLikeEntity Honeycomb()
    {
        var e = new RogueLikeEntity(Color.Goldenrod, Color.Black, '#', layer: (int) GameMap.Layer.Items)
        {
            Name = "Honeycomb"
        };
        e.AllComponents.Add(new HealingConsumableComponent(4));
        e.AllComponents.Add(new DetailsComponent("Food", new[]
        {
            "Super yummy, but super",
            "sticky. Heals you a lil."
        }));
        return e;
    }

    public static RogueLikeEntity ScrollOfEnchantWeapon()
    {
        var e = new RogueLikeEntity(Color.BlueViolet, Color.Black, '?', layer: (int) GameMap.Layer.Items)
        {
            Name = "Scroll of Enchant Weapon"
        };
        e.AllComponents.Add(new EnchantWeaponComponent());
        e.AllComponents.Add(new DetailsComponent("Scroll", new[]
        {
            "Improves an equipped weapon."
        }));
        return e;
    }

    public static RogueLikeEntity ScrollOfEnchantArmor()
    {
        var e = new RogueLikeEntity(Color.AnsiGreenBright, Color.Black, '?', layer: (int) GameMap.Layer.Items)
        {
            Name = "Scroll of Enchant Armor"
        };
        e.AllComponents.Add(new EnchantArmorComponent());
        e.AllComponents.Add(new DetailsComponent("Scroll", new[]
        {
            "Improves worn armor."
        }));
        return e;
    }

    public static RogueLikeEntity MrGreenzWallet()
    {
        var e = new RogueLikeEntity(Color.AnsiGreenBright, Color.Black, 29, layer: (int) GameMap.Layer.Items)
        {
            Name = "Mr. Greenz Wallet"
        };
        e.AllComponents.Add(new MrGreenzComponent());
        e.AllComponents.Add(new DetailsComponent("Wallet", new[]
        {
            "It has a small note inside:",
            "'Don't return this to me I",
            // ReSharper disable once StringLiteralTypo
            "got hundrez of dez tings.'",
            "-Mr. Greenz"
        }));
        return e;
    }
    
    /// <summary>
    /// Functionally a Scroll of Alert Monsters
    /// </summary>
    public static RogueLikeEntity BalloonDog()
    {
        var e = new RogueLikeEntity(Color.Cyan, Color.Black, 227, layer: (int) GameMap.Layer.Items)
        {
            Name = "Balloon Dog"
        };
        e.AllComponents.Add(new BalloonComponent());
        e.AllComponents.Add(new DetailsComponent("Balloon", new[]
        {
            "What a cute little dog!",
            "A faithful companion on your",
            "long and arduous quest. Keep",
            "him close."
        }));
        return e;
    }
    
    public static RogueLikeEntity AmuletOfNyrac()
    {
        var e = new RogueLikeEntity(Color.Turquoise, Color.Black, 12, layer: (int) GameMap.Layer.Items)
        {
            Name = "Amulet of Nyrac"
        };
        e.AllComponents.Add(new WinningComponent());
        e.AllComponents.Add(new DetailsComponent("Amulet", new[]
        {
            "The reason for your quest.",
            "Don this amulet to instantly",
            "escape the dungeon alive."
        }));
        return e;
    }
}
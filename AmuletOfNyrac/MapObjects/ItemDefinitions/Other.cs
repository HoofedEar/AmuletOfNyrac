using AmuletOfNyrac.MapObjects.Components.Items;
using AmuletOfNyrac.Maps;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace AmuletOfNyrac.MapObjects.ItemDefinitions;

/// <summary>
/// Simple class with some static functions for creating items.
/// </summary>
internal static class Other
{
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
            "Super yummy, but super sticky.",
            "Heals you a lil."
        }));
        return e;
    }

    public static RogueLikeEntity HealingPotion()
    {
        var e = new RogueLikeEntity(Color.Red, Color.Black, '!', layer: (int) GameMap.Layer.Items)
        {
            Name = "Healing Potion"
        };
        e.AllComponents.Add(new HealingConsumableComponent(15, "drink"));
        e.AllComponents.Add(new DetailsComponent("Potion", new[]
        {
            "Does what it says on the tin.",
            "Bottoms up, bucko!"
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
        var e = new RogueLikeEntity(Color.OrangeRed, Color.Black, '?', layer: (int) GameMap.Layer.Items)
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
        var e = new RogueLikeEntity(Color.HotPink, Color.Black, 227, layer: (int) GameMap.Layer.Items)
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

    public static RogueLikeEntity CanOfBluePaint()
    {
        var e = new RogueLikeEntity(Color.Blue, Color.Black, 229, layer: (int) GameMap.Layer.Items)
        {
            Name = "Can of Blue Paint"
        };
        e.AllComponents.Add(new PaintComponent(Color.Blue));
        e.AllComponents.Add(new DetailsComponent("Paint Can", new[]
        {
            "Wonder if it tastes like",
            "red paint."
        }));
        return e;
    }
}
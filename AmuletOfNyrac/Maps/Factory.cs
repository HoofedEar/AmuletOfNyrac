using System;
using System.Collections.Generic;
using System.Linq;
using AmuletOfNyrac.MapObjects;
using AmuletOfNyrac.MapObjects.ItemDefinitions;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using ShaiRandom.Generators;

// ReSharper disable SuggestBaseTypeForParameter

namespace AmuletOfNyrac.Maps;

/// <summary>
/// Basic factory which produces different types of maps.
/// </summary>
/// <remarks>
/// The map gen below won't use GoRogue's map generation system for the custom parts, although we could; it simply isn't
/// necessary for the relatively simple, single type of map we have below.
/// </remarks>
internal static class Factory
{
    private const int MaxMonstersPerRoom = 2;
    private const int MaxItemsPerRoom = 4;

    public static int CurrentDungeonDepth = 1;

    public static (GameMap map, Point playerSpawn) Dungeon()
    {
        // Generate a dungeon maze map
        var generator = new Generator(60, 30)
            .ConfigAndGenerateSafe(gen =>
            {
                gen.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps(minRooms: 20, maxRooms: 30, roomMinSize: 6,
                    roomMaxSize: 12, saveDeadEndChance: 0));
            });

        // Extract components from the map GoRogue generated which hold basic information about the map
        var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");
        var rooms = generator.Context.GetFirst<ItemList<Rectangle>>("Rooms");

        // Create actual integration library map with a proper component for the character "memory" system.
        var map = new GameMap(generator.Context.Width, generator.Context.Height, null);
        map.AllComponents.Add(new TerrainFOVVisibilityHandler());

        // Translate GoRogue's terrain data into actual integration library objects.
        map.ApplyTerrainOverlay(generatedMap,
            (pos, val) => val ? MapObjects.Factory.Floor(pos) : MapObjects.Factory.Wall(pos));

        // Set player spawn
        var playerSpawn = GetPlayerSpawn(rooms);

        UpdateTerrain(map);

        // Spawn enemies/items/etc
        SpawnItems(map, rooms, playerSpawn);
        SpawnStairs(map, rooms, playerSpawn);

        switch (CurrentDungeonDepth)
        {
            case >= 15:
                SpawnToughestMonsters(map, rooms, playerSpawn);
                break;
            case >= 10:
                SpawnTougherMonsters(map, rooms, playerSpawn);
                break;
            case >= 5:
                SpawnTougherMonsters(map, rooms, playerSpawn);
                break;
            case <= 4:
                SpawnStandardMonsters(map, rooms, playerSpawn);
                break;
        }


        return (map, playerSpawn);
    }

    private static void SpawnStandardMonsters(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        foreach (var room in rooms.Items)
        {
            var enemies =
                GlobalRandom.DefaultRNG.NextInt(0, MaxMonstersPerRoom + (int) Math.Ceiling(CurrentDungeonDepth / 3.0));
            for (var i = 0; i < enemies; i++)
            {
                var isTough = GlobalRandom.DefaultRNG.PercentageCheck(70f);
                var enemy = isTough ? MapObjects.Enemies.Aimless.Bumbler() : MapObjects.Enemies.Aimless.Rat();

                var isLegendary = GlobalRandom.DefaultRNG.PercentageCheck(5f);
                if (isLegendary)
                    enemy = MapObjects.Enemies.Aimless.GaintRat();

                enemy.Position =
                    GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                map.AddEntity(enemy);
            }
        }
    }

    private static void SpawnTougherMonsters(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        foreach (var room in rooms.Items)
        {
            var enemies =
                GlobalRandom.DefaultRNG.NextInt(0, MaxMonstersPerRoom + (int) Math.Ceiling(CurrentDungeonDepth / 3.0));
            for (var i = 0; i < enemies; i++)
            {
                var isTough = GlobalRandom.DefaultRNG.PercentageCheck(70f);
                var enemy = isTough ? MapObjects.Enemies.Aimless.Rat() : MapObjects.Enemies.Hostile.Wuff();

                var isLegendary = GlobalRandom.DefaultRNG.PercentageCheck(5f);
                if (isLegendary)
                    enemy = MapObjects.Enemies.Hostile.Glomper();

                enemy.Position =
                    GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                map.AddEntity(enemy);
            }
        }
    }

    private static void SpawnToughestMonsters(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        foreach (var room in rooms.Items)
        {
            var enemies =
                GlobalRandom.DefaultRNG.NextInt(0, MaxMonstersPerRoom + (int) Math.Ceiling(CurrentDungeonDepth / 3.0));
            for (var i = 0; i < enemies; i++)
            {
                var isTough = GlobalRandom.DefaultRNG.PercentageCheck(70f);
                var enemy = isTough ? MapObjects.Enemies.Hostile.Wuff() : MapObjects.Enemies.Hostile.Owlbear();

                var isLegendary = GlobalRandom.DefaultRNG.PercentageCheck(5f);
                if (isLegendary)
                    enemy = MapObjects.Enemies.Hostile.Dragoon();

                enemy.Position =
                    GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                map.AddEntity(enemy);
            }
        }
    }

    private static Point GetPlayerSpawn(ItemList<Rectangle> rooms)
    {
        // Add player to map at the center of the first room we placed
        return rooms.Items[0].Center;
    }

    private static void SpawnItems(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        // Generate between zero and the max potions per room.
        foreach (var room in rooms.Items)
        {
            var items = GlobalRandom.DefaultRNG.NextInt(0, MaxItemsPerRoom + 1);
            for (var i = 0; i < items; i++)
            {
                var mythic = GlobalRandom.DefaultRNG.PercentageCheck(2f);
                if (mythic)
                {
                    var mythicType = GlobalRandom.DefaultRNG.NextInt(0, 4);
                    switch (mythicType)
                    {
                        case 0:
                        {
                            var e = Weapons.BugleberrysDarkstaff();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                        case 1:
                        {
                            var e = Armor.RuneBodyplate();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                        case 2:
                        {
                            var e = Other.MrGreenzWallet();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                        case 3:
                        {
                            var e = Other.CanOfBluePaint();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                    }

                    continue;
                }

                var uncommon = GlobalRandom.DefaultRNG.PercentageCheck(10f);
                if (uncommon)
                {
                    var uncommonType = GlobalRandom.DefaultRNG.NextInt(0, 3);
                    switch (uncommonType)
                    {
                        case 0:
                        {
                            var e = Other.ScrollOfEnchantArmor();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                        case 1:
                        {
                            var e = Other.ScrollOfEnchantWeapon();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                        case 2:
                        {
                            var e = Other.BalloonDog();
                            e.Position =
                                GlobalRandom.DefaultRNG.RandomPosition(room,
                                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                            map.AddEntity(e);
                            break;
                        }
                    }

                    continue;
                }

                var item = GlobalRandom.DefaultRNG.RandomElement(new List<Func<RogueLikeEntity>>()
                {
                    Other.Honeycomb, Other.Gold, Other.Gold, Other.Honeycomb, Other.HealingPotion,
                    Weapons.FleetwoodChain, Weapons.PaperMachete, Weapons.WoodenStick,
                    Armor.ChestBarrel, Armor.PunkRockJacket, Armor.TortoiseShell, Weapons.FlameLiberator
                });
                var o = item.Invoke();
                o.Position = GlobalRandom.DefaultRNG.RandomPosition(room,
                    pos => map.WalkabilityView[pos] && pos != playerSpawn);
                map.AddEntity(o);
            }
        }
    }

    private static void SpawnStairs(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        var last = rooms.Items[^1];
        foreach (var room in rooms.Items)
        {
            if (room != last) continue;
            var pos =
                GlobalRandom.DefaultRNG.RandomPosition(room, p => map.WalkabilityView[p] && p != playerSpawn);
            while (map.GetEntitiesAt<RogueLikeEntity>(pos).Any())
            {
                pos =
                    GlobalRandom.DefaultRNG.RandomPosition(room, p => map.WalkabilityView[p] && p != playerSpawn);
            }

            if (CurrentDungeonDepth < 20)
            {
                var floorTerrain = map.GetTerrainAt<Terrain>(pos);
                if (floorTerrain == null) continue;
                floorTerrain.Appearance.Glyph = 240;
                floorTerrain.Appearance.Foreground = Color.Cyan;
                floorTerrain.TrueAppearance.CopyAppearanceFrom(floorTerrain.Appearance);
            }
            else
            {
                var floorTerrain = map.GetTerrainAt<Terrain>(pos);
                if (floorTerrain == null) continue;
                floorTerrain.Appearance.Glyph = 12;
                floorTerrain.Appearance.Foreground = Color.DarkTurquoise;
                floorTerrain.TrueAppearance.CopyAppearanceFrom(floorTerrain.Appearance);
            }
        }
    }

    private static void UpdateTerrain(GameMap map)
    {
        foreach (var t in map.Terrain.Positions())
        {
            var obj = map.GetTerrainAt<Terrain>(t);
            if (obj == null) continue;

            var belowPos = obj.Position + Direction.Down;
            if (belowPos.Y > 29) continue;

            var below = map.GetTerrainAt<Terrain>(belowPos);
            if (below is not {Appearance.Glyph: 46}) continue;
            switch (obj)
            {
                case {Appearance.Glyph: 177}:
                    obj.DarkAppearance.Glyph = 128;
                    obj.Appearance.Glyph = 128;
                    obj.TrueAppearance.CopyAppearanceFrom(obj.Appearance);
                    break;
                case {Appearance.Glyph: 178}:
                    obj.DarkAppearance.Glyph = 130;
                    obj.Appearance.Glyph = 130;
                    obj.TrueAppearance.CopyAppearanceFrom(obj.Appearance);
                    break;
            }
        }
    }
}
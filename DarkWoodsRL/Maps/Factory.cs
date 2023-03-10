using System;
using System.Collections.Generic;
using System.Linq;
using DarkWoodsRL.MapObjects;
using GoRogue.MapGeneration;
using GoRogue.MapGeneration.ContextComponents;
using GoRogue.Random;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using ShaiRandom.Generators;

namespace DarkWoodsRL.Maps;

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
    private const int MaxPotionsPerRoom = 4;

    public static int CurrentDungeonLevel = 0;

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
        SpawnMonsters(map, rooms, playerSpawn);
        SpawnPotions(map, rooms, playerSpawn);
        SpawnStairs(map, rooms, playerSpawn);

        switch (CurrentDungeonLevel)
        {
            // Handle spawning different enemies based on the floor
        }

        return (map, playerSpawn);
    }

    private static Point GetPlayerSpawn(ItemList<Rectangle> rooms)
    {
        // Add player to map at the center of the first room we placed
        return rooms.Items[0].Center;
    }

    private static void SpawnMonsters(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        // Generate between zero and the max monsters per room.  Each monster has an 80% chance of being an orc (weaker)
        // and a 20% chance of being a troll (stronger).
        foreach (var room in rooms.Items)
        {
            int enemies = GlobalRandom.DefaultRNG.NextInt(0, MaxMonstersPerRoom + 1);
            for (int i = 0; i < enemies; i++)
            {
                bool isOrc = GlobalRandom.DefaultRNG.PercentageCheck(80f);

                var enemy = isOrc ? MapObjects.Factory.Orc() : MapObjects.Factory.Troll();
                enemy.Position =
                    GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                map.AddEntity(enemy);
            }
        }
    }

    private static void SpawnPotions(GameMap map, ItemList<Rectangle> rooms, Point playerSpawn)
    {
        // Generate between zero and the max potions per room.
        foreach (var room in rooms.Items)
        {
            int potions = GlobalRandom.DefaultRNG.NextInt(0, MaxPotionsPerRoom + 1);
            for (int i = 0; i < potions; i++)
            {
                var type = GlobalRandom.DefaultRNG.NextInt(0, 4);
                switch (type)
                {
                    case 0:
                    {
                        var potion = MapObjects.Items.Other.Honeycomb();
                        potion.Position =
                            GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                        map.AddEntity(potion);
                        break;
                    }
                    case 1:
                    {
                        var armor = MapObjects.Items.Weapons.LeatherArmor();
                        armor.Position =
                            GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                        map.AddEntity(armor);
                        break;
                    }
                    case 2:
                    {
                        var weapon = MapObjects.Items.Weapons.Dagger();
                        weapon.Position =
                            GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                        map.AddEntity(weapon);
                        break;
                    }
                    case 3:
                    {
                        var gold = MapObjects.Items.Other.Gold();
                        gold.Position =
                            GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
                        map.AddEntity(gold);
                        break;
                    }
                }
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
                GlobalRandom.DefaultRNG.RandomPosition(room, pos => map.WalkabilityView[pos] && pos != playerSpawn);
            var floorTerrain = map.GetTerrainAt<Terrain>(pos);
            if (floorTerrain == null) continue;
            floorTerrain.Appearance.Glyph = 240;
            floorTerrain.Appearance.Foreground = Color.Cyan;
            floorTerrain.TrueAppearance.CopyAppearanceFrom(floorTerrain.Appearance);
        }
    }

    private static void UpdateTerrain(GameMap map)
    {
        foreach (var t in map.Terrain.Positions())
        {
            var obj = map.GetTerrainAt<Terrain>(t);
            if (obj == null) continue;

            var x = obj.Position.X;
            var y = obj.Position.Y;

            var pos = new Point(x, y);
            var belowPos = pos + Direction.Down;
            if (belowPos.Y > 29) continue;

            var below = map.GetTerrainAt<Terrain>(belowPos);
            if (below is not {Appearance.Glyph: 46} || obj is not {Appearance.Glyph: 177}) continue;
            obj.DarkAppearance.Glyph = 128;
            obj.Appearance.Glyph = 128;
            obj.TrueAppearance.CopyAppearanceFrom(obj.Appearance);
        }
    }
}
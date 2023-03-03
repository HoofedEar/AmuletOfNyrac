using System.Collections.Generic;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.Maps;
using DarkWoodsRL.Screens.MainGameMenus;
using DarkWoodsRL.Themes;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects;

internal readonly record struct TerrainAppearanceDefinition(ColoredGlyph Light, ColoredGlyph Dark);

/// <summary>
/// Simple class with some static functions for creating map objects.
/// </summary>
internal static class Factory
{
    /// <summary>
    /// Appearance definitions for various types of terrain objects.  It defines both their normal color, and their
    /// "explored but out of FOV" color.
    /// </summary>
    private static readonly Dictionary<string, TerrainAppearanceDefinition> AppearanceDefinitions = new()
    {
        {
            "Floor",
            new TerrainAppearanceDefinition(
                new ColoredGlyph(Color.DarkGray, Color.Black, '.'),
                new ColoredGlyph(Color.Gray, Color.Black, '.')
            )
        },
        {
            "Wall",
            new TerrainAppearanceDefinition(
                new ColoredGlyph(Color.Ivory, Color.Black, 176),
                new ColoredGlyph(Color.Gray, Color.Black, 176)
            )
        },
    };

    public static Terrain Floor(Point position)
        => new(position, AppearanceDefinitions["Floor"], (int) GameMap.Layer.Terrain);

    public static Terrain Wall(Point position)
        => new(position, AppearanceDefinitions["Wall"], (int) GameMap.Layer.Terrain, false, false);

    public static RogueLikeEntity Player()
    {
        // Create entity with appropriate attributes
        var player = new RogueLikeEntity(Color.Yellow, Color.Black,1, false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Player"
        };

        // Add component for controlling player movement via keyboard.
        var motionControl = new CustomPlayerKeybindingsComponent();
        motionControl.SetMotions(PlayerKeybindingsComponent.ArrowMotions);
        motionControl.SetMotions(PlayerKeybindingsComponent.NumPadAllMotions);
        motionControl.SetMotion(Keys.NumPad5, Direction.None);
        motionControl.SetMotion(Keys.OemPeriod, Direction.None);
        motionControl.SetAction(new InputKey(Keys.OemPeriod, KeyModifiers.Shift), () =>
        {
            // Generate a dungeon map, spawn enemies, and note player spawn location
            var (map, playerSpawn) = Maps.Factory.Dungeon();

            // Set GameScreen's map to the new one and spawn the player in appropriately
            Engine.GameScreen?.SetMap(map, playerSpawn);

            // Calculate initial FOV for player on this new map
            Engine.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();
        });
        
        player.AllComponents.Add(motionControl);

        // Add controls for picking up items and getting to inventory screen.
        motionControl.SetAction(Keys.OemComma,
            () => PlayerActionHelper.PlayerTakeAction(e => e.AllComponents.GetFirst<Inventory>().PickUp()));
        motionControl.SetAction(Keys.C, () => Game.Instance.Screen.Children.Add(new ConsumableSelect()));

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController {FOVRadius = 6});

        // Player combatant
        player.AllComponents.Add(new Combatant(30, 2, 5));

        // Player inventory
        player.AllComponents.Add(new Inventory(26));

        return player;
    }

    public static RogueLikeEntity Orc()
    {
        var enemy = new RogueLikeEntity(MainPalette.Sage, Color.Black, 'o', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Orc"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new Combatant(10, 0, 3));

        return enemy;
    }

    public static RogueLikeEntity Troll()
    {
        var enemy = new RogueLikeEntity(MainPalette.LightGray, Color.Black, 'T', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Troll"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new Combatant(16, 1, 4));

        return enemy;
    }

    public static RogueLikeEntity Corpse(RogueLikeEntity entity)
        => new(entity.Appearance, layer: (int) GameMap.Layer.Items)
        {
            Name = $"Corpse - {entity.Name}",
            Position = entity.Position,
            Appearance =
            {
                Glyph = '%'
            }
        };
}
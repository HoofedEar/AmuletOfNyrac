using System.Collections.Generic;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Combatant;
using DarkWoodsRL.Maps;
using DarkWoodsRL.Screens.MainGameMenus;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects;

public readonly record struct TerrainAppearanceDefinition(ColoredGlyph Light, ColoredGlyph Dark);

/// <summary>
/// Simple class with some static functions for creating map objects.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Appearance definitions for various types of terrain objects.  It defines both their normal color, and their
    /// "explored but out of FOV" color.
    /// </summary>
    public static readonly Dictionary<string, TerrainAppearanceDefinition> AppearanceDefinitions = new()
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
                new ColoredGlyph(Color.Ivory, Color.Black, 177),
                new ColoredGlyph(Color.Gray, Color.Black, 177)
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
        var player = new RogueLikeEntity(Color.Yellow, Color.Black, 1, false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Hero"
        };

        // Add component for controlling player movement via keyboard.
        var motionControl = new CustomPlayerKeybindingsComponent();
        motionControl.SetMotions(KeybindingsComponent.ArrowMotions);
        motionControl.SetMotions(KeybindingsComponent.NumPadAllMotions);
        motionControl.SetMotions(KeybindingsComponent.WasdMotions);
        motionControl.SetMotion(Keys.NumPad5, Direction.None);
        motionControl.SetMotion(Keys.OemPeriod, Direction.None);
        motionControl.SetAction(new InputKey(Keys.OemPeriod, KeyModifiers.Shift),
            () => PlayerActionHelper.PlayerTakeAction(e => e.AllComponents.GetFirst<DepthHandlerComponent>().Descend()));

        // Add controls for picking up items and getting to inventory screen.
        motionControl.SetAction(Keys.Space,
            () => PlayerActionHelper.PlayerTakeAction(e => e.AllComponents.GetFirst<InventoryComponent>().PickUp()));
        motionControl.SetAction(Keys.E, () => Game.Instance.Screen.Children.Add(new InventoryScreen()));
        
        motionControl.SetAction(Keys.Escape, () => Game.Instance.Screen.Children.Add(new PauseView()));

        player.AllComponents.Add(motionControl);

        // Add component for updating map's player FOV as they move
        player.AllComponents.Add(new PlayerFOVController {FOVRadius = 6});

        // Player combatant
        player.AllComponents.Add(new CombatantComponent(20, 2, 5, 1));

        // Player inventory
        player.AllComponents.Add(new InventoryComponent(26));
        player.AllComponents.Add(new DepthHandlerComponent());


        return player;
    }
}
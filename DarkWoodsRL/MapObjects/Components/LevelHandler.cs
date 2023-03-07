using System;
using System.Linq;
using DarkWoodsRL.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components;

public class LevelHandler : RogueLikeComponentBase<RogueLikeEntity>
{
    public LevelHandler() : base(false, false, false, false)
    {
    }

    public bool Descend()
    {
        var isPlayer = Parent == Engine.Player;
        var getTerrain = Parent?.CurrentMap?.GetTerrainAt<Terrain>(Parent.Position);
        if (getTerrain is {Appearance.Glyph: 240})
        {
            if (!isPlayer) return false;
            Maps.Factory.CurrentDungeonLevel += 1;

            // Set map type based on amulets found
            // if (Maps.Factory.AmuletsFound[0])
            if (Maps.Factory.CurrentDungeonLevel >= 1)
            {
                GreenMap();
            }
            else
            {
                DefaultMap();
            }

            Engine.GameScreen?.MessageLog.AddMessage(new ColoredString($"You descend the stairs.",
                MessageColors.ItemPickedUpAppearance));
            // Generate a dungeon map, spawn enemies, and note player spawn location
            var (map, playerSpawn) = Maps.Factory.Dungeon();

            // Set GameScreen's map to the new one and spawn the player in appropriately
            Engine.GameScreen?.SetMap(map, playerSpawn);

            // Calculate initial FOV for player on this new map
            Engine.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();

            return true;
        }

        if (isPlayer)
            Engine.GameScreen?.MessageLog.AddMessage(new("There are no stairs to descend here.",
                MessageColors.ImpossibleActionAppearance));
        return false;
    }

    private void DefaultMap()
    {
        var floor = Factory.AppearanceDefinitions.First(f => f.Key == "Floor").Value;
        floor.Light.Foreground = Color.DarkGray;
        floor.Dark.Foreground = Color.Gray;

        var walls = Factory.AppearanceDefinitions.First(t => t.Key == "Wall").Value;
        walls.Light.Glyph = 177;
        walls.Dark.Glyph = 177;
        walls.Light.Foreground = Color.Ivory;
        walls.Dark.Foreground = Color.Gray;
    }

    private void GreenMap()
    {
        var floor = Factory.AppearanceDefinitions.First(f => f.Key == "Floor").Value;
        floor.Light.Foreground = new Color(107, 142, 35);
        floor.Dark.Foreground = new Color(85, 107, 47);

        var walls = Factory.AppearanceDefinitions.First(t => t.Key == "Wall").Value;
        walls.Light.Glyph = 20;
        walls.Dark.Glyph = 20;
        walls.Light.Foreground = Color.LawnGreen;
        walls.Dark.Foreground = Color.DarkGreen;
    }

    private void RedMap()
    {
        var floor = Factory.AppearanceDefinitions.First(f => f.Key == "Floor").Value;
        floor.Light.Foreground = Color.DarkRed;
        floor.Dark.Foreground = new Color(50, 20, 20);

        var walls = Factory.AppearanceDefinitions.First(t => t.Key == "Wall").Value;
        walls.Light.Foreground = new Color(165, 42, 42, 255);
        walls.Dark.Foreground = new Color(65, 42, 42, 255);
        walls.Light.Glyph = 178;
        walls.Dark.Glyph = 178;
    }
    
    private void BlueMap()
    {
        var floor = Factory.AppearanceDefinitions.First(f => f.Key == "Floor").Value;
        floor.Light.Foreground = Color.DarkBlue;
        floor.Dark.Foreground = new Color(20, 20, 50);

        var walls = Factory.AppearanceDefinitions.First(t => t.Key == "Wall").Value;
        walls.Light.Foreground = new Color(42, 42, 205, 255);
        walls.Dark.Foreground = new Color(42, 42, 105, 255);
        walls.Light.Glyph = 129;
        walls.Dark.Glyph = 129;
    }
}
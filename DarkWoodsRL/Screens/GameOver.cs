using System;
using DarkWoodsRL.MapObjects;
using DarkWoodsRL.MapObjects.Components;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace DarkWoodsRL.Screens;

public class GameOver : ControlsConsole
{
    private int _score;

    public GameOver(string message, Color color, int score) : base(16, 10)
    {
        _score = score;
        Position = (Engine.ScreenWidth / 2 - Width / 2, Engine.ScreenHeight / 2 - Height / 2);

        var title = new Label(message)
        {
            Name = "GameOver",
            TextColor = color,
            Alignment = HorizontalAlignment.Center,
            Position = (0, 0)
        };
        Controls.Add(title);

        var scoreVis = new Label("Final score:")
        {
            Name = "ScoreLabel",
            TextColor = Color.Gold,
            Alignment = HorizontalAlignment.Center,
            Position = (0, 2)
        };
        Controls.Add(scoreVis);

        var scoreLabel = new Label("" + score)
        {
            Name = "ScoreValue",
            TextColor = Color.Silver,
            Alignment = HorizontalAlignment.Center,
            Position = (0, 3)
        };
        Controls.Add(scoreLabel);

        var floorLabel = new Label("Depth: " + Maps.Factory.CurrentDungeonDepth)
        {
            Name = "FloorDepth",
            TextColor = Color.Silver,
            Alignment = HorizontalAlignment.Center,
            Position = (0, 5)
        };
        Controls.Add(floorLabel);

        // Add buttons
        var newGame = new Button(Width)
        {
            Name = "NewGameBtn",
            Text = "New Game",
            Position = (0, 9)
        };
        newGame.Click += NewGameOnClick;
        Controls.Add(newGame);
    }

    private void NewGameOnClick(object? sender, EventArgs e)
    {
        // Create player entity
        Engine.Player = Factory.Player();
        Maps.Factory.CurrentDungeonDepth = 0;
        DepthHandlerComponent.DefaultMap();

        // Generate a dungeon map, spawn enemies, and note player spawn location
        var (map, playerSpawn) = Maps.Factory.Dungeon();

        // Create a MapScreen and set it as the active screen so that it processes input and renders itself.
        Engine.GameScreen = new MainGame(map, playerSpawn);
        GameHost.Instance.Screen = Engine.GameScreen;

        // Calculate initial FOV for player
        Engine.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();
    }
}
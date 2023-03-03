using System;
using DarkWoodsRL.MapObjects;
using DarkWoodsRL.MapObjects.Components;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;

namespace DarkWoodsRL.Screens;

/// <summary>
/// The main menu screen.
/// </summary>
public class MainMenu : ControlsConsole
{
    public MainMenu()
        : base(15, 10)
    {
        // Position controls console
        Position = (Engine.ScreenWidth / 2 - Width / 2, Engine.ScreenHeight / 2 - Height / 2);

        // Game Title
        var title = new Label("Overgrown Tomb")
        {
            Name = "GameTitle",
            TextColor = Color.DarkGreen,
            Position = (0, 0)
        };
        Controls.Add(title);
        
        // Add buttons
        var newGame = new Button(Width)
        {
            Name = "NewGameBtn",
            Text = "New Game",
            Position = (0, 2)
        };
        newGame.Click += NewGameOnClick;
        Controls.Add(newGame);

        var loadGame = new Button(Width)
        {
            Name = "LoadGameBtn",
            Text = "Load Game",
            Position = (0, 3)
        };
        loadGame.Click += LoadGameOnClick;
        Controls.Add(newGame);

        var exit = new Button(Width)
        {
            Name = "ExitBtn",
            Text = "Exit",
            Position = (0, 4)
        };
        exit.Click += ExitOnClick;
        Controls.Add(exit);
    }

    private void NewGameOnClick(object? sender, EventArgs e)
    {
        // Create player entity
        Engine.Player = Factory.Player();

        // Generate a dungeon map, spawn enemies, and note player spawn location
        var (map, playerSpawn) = Maps.Factory.Dungeon();
        
        // Create a MapScreen and set it as the active screen so that it processes input and renders itself.
        Engine.GameScreen = new MainGame(map, playerSpawn);
        GameHost.Instance.Screen = Engine.GameScreen;

        // Calculate initial FOV for player
        Engine.Player.AllComponents.GetFirst<PlayerFOVController>().CalculateFOV();
    }

    private void LoadGameOnClick(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ExitOnClick(object? sender, EventArgs e)
    {
        Game.Instance.MonoGameInstance.Exit();
    }
}
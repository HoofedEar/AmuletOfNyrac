using DarkWoodsRL.Screens;
using SadConsole;
using SadConsole.SplashScreens;
using SadRogue.Integration;

namespace DarkWoodsRL;

internal static class Engine
{
    // Window width/height
    public const int ScreenWidth = 60;
    public const int ScreenHeight = 25;

    public static MainGame? GameScreen;

    // Null override because it's initialized via new-game/load game
    public static RogueLikeEntity Player = null!;

    private static void Main()
    {
        Settings.WindowTitle = "Amulet of Nyrac";

        Game.Create(ScreenWidth, ScreenHeight, "Fonts/Andux2x.font");
        Game.Instance.OnStart = Init;
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void Init()
    {
        SadConsole.Game.Instance.SetSplashScreens(new SadConsole.SplashScreens.Ansi1());

        // Main menu
        GameHost.Instance.Screen = new MainMenu();

        // Destroy the default starting console that SadConsole created automatically because we're not using it.
        GameHost.Instance.DestroyDefaultStartingConsole();
    }
}
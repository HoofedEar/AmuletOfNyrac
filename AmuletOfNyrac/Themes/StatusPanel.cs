using SadConsole.UI;

namespace AmuletOfNyrac.Themes;

/// <summary>
/// Colors/themes related to the Screens.Surfaces.StatusPanel.
/// </summary>
internal static class StatusPanel
{

    public static readonly Colors HPBarColors = GetHPBarColors();

    private static Colors GetHPBarColors()
    {
        var colors = new Colors();
        colors.Appearance_ControlNormal.Foreground = MainPalette.Sage;
        colors.Appearance_ControlNormal.Background = MainPalette.Magenta;

        return colors;
    }
}
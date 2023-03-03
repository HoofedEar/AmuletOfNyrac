using SadConsole.UI;
using SadConsole.UI.Themes;

namespace DarkWoodsRL.Themes;

/// <summary>
/// Colors/themes related to the Screens.Surfaces.StatusPanel.
/// </summary>
internal static class StatusPanel
{

    public static readonly Colors HPBarColors = GetHPBarColors();

    private static Colors GetHPBarColors()
    {
        var colors = Library.Default.Colors.Clone();
        colors.Appearance_ControlNormal.Foreground = MainPalette.Sage;
        colors.Appearance_ControlNormal.Background = MainPalette.Magenta;

        return colors;
    }
}
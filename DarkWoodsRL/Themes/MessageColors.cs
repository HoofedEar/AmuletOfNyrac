using SadConsole;

namespace DarkWoodsRL.Themes;

/// <summary>
/// Static class which defines <see cref="ColoredString.ColoredGlyphEffect"/> instances which define the colors used for
/// different types of messages.
/// </summary>
internal static class MessageColors
{
    /// <summary>
    /// Initial welcome text printed on dungeon entrance.
    /// </summary>
    public static readonly ColoredGlyphAndEffect WelcomeTextAppearance = new()
    {
        Foreground = new(0x20, 0xA0, 0xFF)
    };

    /// <summary>
    /// Text indicating the player attacked something.
    /// </summary>
    public static readonly ColoredGlyphAndEffect PlayerAtkAppearance = new()
    {
        Foreground = new(0xE0, 0xE0, 0xE0)
    };

    /// <summary>
    /// Text indicating an enemy attacked the player.
    /// </summary>
    public static readonly ColoredGlyphAndEffect EnemyAtkAtkAppearance = new()
    {
        Foreground = new(0xFF, 0xC0, 0xC0)
    };

    /// <summary>
    /// Text indicating an enemy died.
    /// </summary>
    public static readonly ColoredGlyphAndEffect EnemyDiedAppearance = new()
    {
        Foreground = new(0xFF, 0xA0, 0x30)
    };

    /// <summary>
    /// Text indicating the player tried to take an action which is not possible (ie. moving into a wall).
    /// </summary>
    public static readonly ColoredGlyphAndEffect ImpossibleActionAppearance = new()
    {
        Foreground = new(0x80, 0x80, 0x80)
    };

    /// <summary>
    /// Text indicating the player picked up an item.
    /// TODO: Pick proper color
    /// </summary>
    public static readonly ColoredGlyphAndEffect ItemPickedUpAppearance = new()
    {
        Foreground = new(0xFF, 0xFF, 0xFF)
    };

    /// <summary>
    /// Text indicating the player dropped an item.
    /// TODO: Pick proper color
    /// </summary>
    public static readonly ColoredGlyphAndEffect ItemDroppedAppearance = new()
    {
        Foreground = new(0xFF, 0xFF, 0xFF)
    };

    /// <summary>
    /// Text indicating the player recovered health.
    /// </summary>
    public static readonly ColoredGlyphAndEffect HealthRecoveredAppearance = new()
    {
        Foreground = new(0x0, 0xFF, 0x0)
    };
}
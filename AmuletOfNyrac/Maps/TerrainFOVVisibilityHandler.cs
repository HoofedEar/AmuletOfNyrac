using AmuletOfNyrac.MapObjects;
using SadRogue.Integration.FieldOfView.Memory;

namespace AmuletOfNyrac.Maps;

/// <summary>
/// Visibility handler that handles FOV cells going out of FOV and into memory by simply changing it to its darkened appearance field.
/// </summary>
internal class TerrainFOVVisibilityHandler : MemoryFieldOfViewHandlerBase
{
    protected override void ApplyMemoryAppearance(MemoryAwareRogueLikeCell terrain)
    {
        terrain.LastSeenAppearance.CopyAppearanceFrom(((Terrain)terrain).DarkAppearance);
    }
}
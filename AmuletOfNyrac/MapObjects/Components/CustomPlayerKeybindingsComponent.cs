using AmuletOfNyrac.Maps;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace AmuletOfNyrac.MapObjects.Components;

/// <summary>
/// Subclass of the integration library's keybindings component that ensures the player's movements count as the player's turn when successful.
/// </summary>
internal class CustomPlayerKeybindingsComponent : KeybindingsComponent<RogueLikeEntity>
{
    protected override void MotionHandler(Direction direction)
    {
        if (Parent == null) return;

        // If we're waiting a turn, there's nothing to do; it's always a valid turn to wait
        if (direction == Direction.None)
            PlayerActionHelper.PlayerTakeAction(_ => true);
        else
            PlayerActionHelper.PlayerTakeAction(GameMap.MoveOrBump, direction);
    }
}
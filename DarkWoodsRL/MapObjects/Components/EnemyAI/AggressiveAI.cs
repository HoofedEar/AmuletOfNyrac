using System.Linq;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.EnemyAI;

/// <summary>
/// Parent remembers that they saw the player and will keep moving towards them.
/// </summary>
public class AggressiveAI : RogueLikeComponentBase<RogueLikeEntity>, IEnemyAI
{
    private bool _playerSeen;
    public AggressiveAI(bool angry = false) : base(false, false, false, false)
    {
        // Useful if we want to make an enemy hunt the player down by default
        _playerSeen = angry;
    }

    public void TakeTurn()
    {
        if (Parent?.CurrentMap == null) return;
        if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position) && !_playerSeen) return;
        if (Parent.AllComponents.GetFirst<Combatant.CombatantComponent>().HP <= 0) return;

        _playerSeen = true;
        var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Engine.Player.Position);
        if (path == null) return;
        var firstPoint = path.GetStep(0);
        GameMap.MoveOrBump(Parent, Direction.GetDirection(Parent.Position, firstPoint));
    }
}
using System.Linq;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.EnemyAI;

public class AggressiveAI : RogueLikeComponentBase<RogueLikeEntity>, IEnemyAI
{
    private bool _playerSeen;

    public AggressiveAI() : base(false, false, false, false)
    {
    }

    public void TakeTurn()
    {
        if (Parent?.CurrentMap == null) return;
        if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position) && !_playerSeen) return;
        if (Parent.AllComponents.GetFirst<Combatant.CombatantComponant>().HP <= 0) return;

        _playerSeen = true;
        var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Engine.Player.Position);
        if (path == null) return;
        var firstPoint = path.GetStep(0);
        GameMap.MoveOrBump(Parent, Direction.GetDirection(Parent.Position, firstPoint));
    }
}
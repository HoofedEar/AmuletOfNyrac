using System.Linq;
using DarkWoodsRL.Maps;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.EnemyAI;

/// <summary>
/// Parent only moves towards the player when they are visible.
/// </summary>
public class HostileAI : RogueLikeComponentBase<RogueLikeEntity>, IEnemyAI
{
    public HostileAI()
        : base(false, false, false, false)
    {
    }

    public void TakeTurn()
    {
        if (Parent?.CurrentMap == null) return;
        if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position)) return;
        if (Parent.AllComponents.GetFirst<Combatant.CombatantComponent>().HP <= 0) return;

        var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Engine.Player.Position);
        if (path == null) return;
        var firstPoint = path.GetStep(0);
        GameMap.MoveOrBump(Parent, Direction.GetDirection(Parent.Position, firstPoint));
    }
}
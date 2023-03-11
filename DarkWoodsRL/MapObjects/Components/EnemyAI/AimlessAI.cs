using System;
using System.Linq;
using DarkWoodsRL.Maps;
using GoRogue.Random;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.EnemyAI;

/// <summary>
/// Parent moves around randomly, can sometimes beat up other enemies
/// </summary>
public class AimlessAI : RogueLikeComponentBase<RogueLikeEntity>, IEnemyAI
{
    public bool IsAngry;

    public AimlessAI() : base(false, false, false, false)
    {
    }

    public void TakeTurn()
    {
        if (Parent?.CurrentMap == null) return;
        if (!IsAngry)
        {
            var choice = GlobalRandom.DefaultRNG.NextInt(0, 4);
            var dir = choice switch
            {
                0 => Direction.Up,
                1 => Direction.Left,
                2 => Direction.Right,
                3 => Direction.Down,
                _ => throw new ArgumentOutOfRangeException()
            };

            GameMap.MoveOrBump(Parent, dir);
        }
        else
        {
            if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position)) return;
            if (Parent.AllComponents.GetFirst<Combatant.CombatantComponent>().HP <= 0) return;

            var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Engine.Player.Position);
            if (path == null) return;
            var firstPoint = path.GetStep(0);
            GameMap.MoveOrBump(Parent, Direction.GetDirection(Parent.Position, firstPoint));
        }
    }
}
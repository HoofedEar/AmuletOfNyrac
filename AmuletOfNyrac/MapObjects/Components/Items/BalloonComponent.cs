using System.Linq;
using AmuletOfNyrac.MapObjects.Components.EnemyAI;
using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using AmuletOfNyrac.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class BalloonComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public BalloonComponent() : base(false, false, false, false)
    {
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        Engine.GameScreen?.MessageLog.AddMessage(
            new ColoredString($"POP! Your companion is reduced to rubber pieces on the floor.",
                MessageColors.EnemyAtkAtkAppearance));
        // Aggro everyone
        foreach (var p in consumer.CurrentMap!.Entities.AsEnumerable())
        {
            var pos = p.Position;
            var entity = consumer.CurrentMap.GetEntityAt<RogueLikeEntity>(pos);

            if (entity == null) continue;
            if (entity == consumer) continue;
            if (!entity.AllComponents.Contains<IEnemyAI>()) continue;

            entity.AllComponents.Remove(entity.AllComponents.GetFirst<IEnemyAI>());
            entity.AllComponents.Add(new AggressiveAI(true));
        }

        return true;
    }
}
using System.Linq;
using DarkWoodsRL.MapObjects.Components.EnemyAI;
using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using DarkWoodsRL.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items;

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
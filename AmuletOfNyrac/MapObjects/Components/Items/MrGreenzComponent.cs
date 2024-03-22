using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using AmuletOfNyrac.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class MrGreenzComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public MrGreenzComponent() : base(false, false, false, false)
    {
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        var inv = consumer.AllComponents.GetFirst<InventoryComponent>();
        inv.Gold += 2000;
        Engine.GameScreen?.MessageLog.AddMessage(new ColoredString(
            $"The wallet contains $2000! Thanks, Mr. Greenz!",
            MessageColors.HealthRecoveredAppearance));
        return true;
    }
}
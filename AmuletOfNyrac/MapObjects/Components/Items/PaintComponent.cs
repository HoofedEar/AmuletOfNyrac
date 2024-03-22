using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using AmuletOfNyrac.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class PaintComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    private readonly Color _color;

    public PaintComponent(Color color) : base(false, false, false, false)
    {
        _color = color;
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        consumer.AppearanceSingle!.Appearance.Foreground = _color;
        // consumer.Appearance.Foreground = _color;
        Engine.GameScreen?.MessageLog.AddMessage(
            new ColoredString($"ACK! Yup, tastes like red paint.",
                MessageColors.EnemyAtkAtkAppearance));
        return true;
    }
}
using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.Items;

public class PaintComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    private readonly Color _color;

    protected PaintComponent(Color color) : base(false, false, false, false)
    {
        _color = color;
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        consumer.Appearance.Foreground = _color;
        return true;
    }
}
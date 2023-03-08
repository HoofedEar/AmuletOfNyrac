using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items;

public class GoldComponent : RogueLikeComponentBase<RogueLikeEntity>, ICarryable
{
    protected GoldComponent() : base(false, false, false, false)
    {
    }
    
    
}
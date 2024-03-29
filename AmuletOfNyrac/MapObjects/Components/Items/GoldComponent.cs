using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using GoRogue.Random;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class GoldComponent : RogueLikeComponentBase<RogueLikeEntity>, ICarryable
{
    public int Value { get; }

    public GoldComponent() : base(false, false, false, false)
    {
        Value = GlobalRandom.DefaultRNG.NextInt(10, 51);
    }
}
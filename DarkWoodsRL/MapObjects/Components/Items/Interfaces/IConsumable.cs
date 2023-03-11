using SadRogue.Integration;

namespace DarkWoodsRL.MapObjects.Components.Items.Interfaces;

/// <summary>
/// Implemented by things that can be consumed.
/// </summary>
internal interface IConsumable : ICarryable
{
    bool Consume(RogueLikeEntity consumer);
}
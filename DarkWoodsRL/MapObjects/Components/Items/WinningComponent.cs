using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using DarkWoodsRL.Screens;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Components.Items;

public class WinningComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public WinningComponent() : base(false, false, false, false)
    {
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        Game.Instance.Screen = new GameOver("    YOU WIN ", Color.AnsiGreenBright,
            Engine.Player.AllComponents.GetFirst<InventoryComponent>().Gold);
        return true;
    }
}
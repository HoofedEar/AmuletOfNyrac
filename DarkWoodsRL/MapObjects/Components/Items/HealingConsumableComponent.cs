using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using DarkWoodsRL.Themes;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items;

/// <summary>
/// Consumable that restores up to the given amount of HP.
/// </summary>
internal class HealingConsumableComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public int Amount { get; }
    public string Action { get; }
    public HealingConsumableComponent(int amount, string action = "eat")
        : base(false, false, false, false)
    {
        Amount = amount;
        Action = action;
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        var isPlayer = consumer == Engine.Player;

        var combatant = consumer.AllComponents.GetFirst<Combatant.CombatantComponent>();
        var amountRecovered = combatant.Heal(Amount);
        if (amountRecovered > 0)
        {
            if (isPlayer)
                Engine.GameScreen?.MessageLog.AddMessage(new(
                    $"You {Action} the {Parent!.Name}, and heal {amountRecovered} HP!",
                    MessageColors.HealthRecoveredAppearance));
            return true;
        }

        if (isPlayer)
            Engine.GameScreen?.MessageLog.AddMessage(new("Your health is already full.", MessageColors.ImpossibleActionAppearance));
        return false;
    }
}
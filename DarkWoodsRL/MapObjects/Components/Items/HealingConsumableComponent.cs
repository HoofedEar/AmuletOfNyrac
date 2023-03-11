﻿using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
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

    public HealingConsumableComponent(int amount)
        : base(false, false, false, false)
    {
        Amount = amount;
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        var isPlayer = consumer == Engine.Player;

        var combatant = consumer.AllComponents.GetFirst<Combatant.CombatantComponant>();
        var amountRecovered = combatant.Heal(Amount);
        if (amountRecovered > 0)
        {
            if (isPlayer)
                Engine.GameScreen?.MessageLog.AddMessage(new(
                    $"You eat the {Parent!.Name}, and recover {amountRecovered} HP!",
                    MessageColors.HealthRecoveredAppearance));
            return true;
        }

        if (isPlayer)
            Engine.GameScreen?.MessageLog.AddMessage(new("Your health is already full.", MessageColors.ImpossibleActionAppearance));
        return false;
    }
}
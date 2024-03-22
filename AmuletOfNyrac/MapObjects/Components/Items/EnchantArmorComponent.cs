using AmuletOfNyrac.MapObjects.Components.Items.Armor;
using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using AmuletOfNyrac.Themes;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class EnchantArmorComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public EnchantArmorComponent() : base(false, false, false, false)
    {
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        var used = false;
        var maxEnch = false;
        var inventory = consumer.AllComponents.GetFirst<InventoryComponent>();
        RogueLikeEntity? enchanted = null;
        foreach (var item in inventory.Items)
        {
            var armor = item.AllComponents.GetFirstOrDefault<ArmorComponent>();
            // Check that it is a weapon
            if (armor == null || used || maxEnch) continue;
            
            // Then check that it is equipped, and not already enchant lvl 6
            if (armor is not {IsEquipped: true}) continue;
            
            if (armor is not {EnchantLvl: < 5})
            {
                maxEnch = true;
                continue;
            }
            
            var prev = armor.EnchantLvl;
            armor.EnchantLvl += 1;
            armor.ENDMod += 1;
            // have to adjust the players value live too
            var combat = consumer.AllComponents.GetFirst<Combatant.CombatantComponent>();
            combat.END += 1;
            if (armor.Parent != null)
                armor.Parent.Name = "+" + armor.EnchantLvl + " " + armor.Parent.Name.Replace("+" + prev + " ", "");
            used = true;
            enchanted = item;
        }

        // max enchant
        if (maxEnch)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new("You read the scroll, but it burns away in your hands.", MessageColors.ImpossibleActionAppearance));
            return true;
        }

        // no enchantment
        if (!used)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new("You read the scroll, but it crumbles in your hands.", MessageColors.ImpossibleActionAppearance));
            return true;
        }

        // Enchanted item successfully
        if (enchanted != null)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new(
                $"You read the scroll, and your armor glows brighter!",
                MessageColors.HealthRecoveredAppearance));
            return true;
        }

        return true;
    }
}
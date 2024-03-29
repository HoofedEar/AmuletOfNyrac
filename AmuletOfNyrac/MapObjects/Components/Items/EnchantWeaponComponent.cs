using AmuletOfNyrac.MapObjects.Components.Items.Interfaces;
using AmuletOfNyrac.MapObjects.Components.Items.Weapon;
using AmuletOfNyrac.Themes;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace AmuletOfNyrac.MapObjects.Components.Items;

public class EnchantWeaponComponent : RogueLikeComponentBase<RogueLikeEntity>, IConsumable
{
    public EnchantWeaponComponent() : base(false, false, false, false)
    {
    }

    public bool Consume(RogueLikeEntity consumer)
    {
        var isPlayer = consumer == Engine.Player;
        var used = false;
        var maxEnch = false;
        var inventory = consumer.AllComponents.GetFirst<InventoryComponent>();
        RogueLikeEntity? enchanted = null;
        foreach (var item in inventory.Items)
        {
            var weapon = item.AllComponents.GetFirstOrDefault<WeaponComponent>();
            // Check that it is a weapon
            if (weapon == null || used || maxEnch) continue;
            
            // Then check that it is equipped, and not already enchant lvl 6
            if (weapon is not {IsEquipped: true}) continue;
            
            if (weapon is not {EnchantLvl: < 5})
            {
                maxEnch = true;
                continue;
            }
            
            var prev = weapon.EnchantLvl;
            weapon.EnchantLvl += 1;
            weapon.STRMod += 1;
            weapon.DEXMod += 1;
            // have to adjust the players value live too
            var combat = consumer.AllComponents.GetFirst<Combatant.CombatantComponent>();
            combat.STR += 1;
            combat.DEX += 1;
            if (weapon.Parent != null)
                weapon.Parent.Name = "+" + weapon.EnchantLvl + " " + weapon.Parent.Name.Replace("+" + prev + " ", "");
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
                $"You read the scroll, and your weapon glows brighter!",
                MessageColors.HealthRecoveredAppearance));
            return true;
        }

        return true;
    }
}
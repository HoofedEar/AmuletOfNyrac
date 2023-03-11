using System;
using System.Collections.Generic;
using System.Linq;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using DarkWoodsRL.Themes;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components;

/// <summary>
/// Component representing an inventory which can hold a given number of items.
/// </summary>
internal class InventoryComponent : RogueLikeComponentBase<RogueLikeEntity>
{
    private int Capacity { get; }
    private int _gold;

    public readonly List<RogueLikeEntity> Items;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            GoldChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? GoldChanged;

    public InventoryComponent(int capacity)
        : base(false, false, false, false)
    {
        Capacity = capacity;
        Items = new List<RogueLikeEntity>(capacity);
        Items.Add(ItemDefinitions.Other.HealingPotion());
        Items.Add(ItemDefinitions.Other.HealingPotion());
    }

    /// <summary>
    /// Drops the given item from this inventory.
    /// </summary>
    public void Drop(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException(
                "Can't drop an entity from an inventory that's not connected to an object.");
        if (Parent.CurrentMap == null)
            throw new InvalidOperationException(
                "Objects are not allowed to drop items from their inventory when they're not part of a map.");

        if (!Items.Remove(item))
            throw new ArgumentException("Tried to drop an item from an inventory it was not a part of.", nameof(item));

        item.Position = Parent.Position;
        Parent.CurrentMap.AddEntity(item);

        if (Parent == Engine.Player)
            Engine.GameScreen?.MessageLog.AddMessage(new ColoredString($"You dropped {item.Name}.",
                MessageColors.ItemDroppedAppearance));
    }

    /// <summary>
    /// Tries to pick up the first item found at the Parent's location.
    /// </summary>
    /// <returns>True if an item was picked up; false otherwise.</returns>
    public bool PickUp()
    {
        if (Parent == null)
            throw new InvalidOperationException(
                "Can't pick up an item into an inventory that's not connected to an object.");

        if (Parent.CurrentMap == null)
            throw new InvalidOperationException("Entity must be part of a map to pick up items.");

        var isPlayer = Parent == Engine.Player;

        var inventory = Parent.AllComponents.GetFirst<InventoryComponent>();
        foreach (var item in Parent.CurrentMap.GetEntitiesAt<RogueLikeEntity>(Parent.Position))
        {
            if (!item.AllComponents.Contains<ICarryable>())
            {
                var terrain = Parent.CurrentMap.GetTerrainAt<Terrain>(Parent.Position);
                if (terrain is {Appearance.Glyph: 240} or {Appearance.Glyph: 12})
                {
                    Parent.AllComponents.GetFirst<DepthHandlerComponent>().Descend();
                    return false;
                }

                continue;
            }

            var gold = item.AllComponents.GetFirstOrDefault<GoldComponent>();
            if (inventory.Items.Count >= inventory.Capacity && gold == null)
            {
                if (isPlayer)
                    Engine.GameScreen?.MessageLog.AddMessage(new ColoredString("Your inventory is full.",
                        MessageColors.ImpossibleActionAppearance));
                return false;
            }

            item.CurrentMap!.RemoveEntity(item);
            if (gold != null)
            {
                Gold += gold.Value;
                if (isPlayer)
                    Engine.GameScreen?.MessageLog.AddMessage(new ColoredString($"You picked up {gold.Value} Gold.",
                        MessageColors.ItemPickedUpAppearance));

                return true;
            }

            inventory.Items.Add(item);

            if (isPlayer)
                Engine.GameScreen?.MessageLog.AddMessage(new ColoredString($"You picked up {item.Name}.",
                    MessageColors.ItemPickedUpAppearance));

            return true;
        }

        if (isPlayer)
            Engine.GameScreen?.MessageLog.AddMessage(new("There is nothing here to pick up.",
                MessageColors.ImpossibleActionAppearance));
        return false;
    }

    /// <summary>
    /// Causes the parent to consume the given consumable item.  The given entity must have some component implementing IConsumable.
    /// </summary>
    public bool Consume(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Cannot consume item from an inventory not attached to an object.");
        var consumable = item.AllComponents.GetFirst<IConsumable>();

        var idx = Items.FindIndex(i => i == item);
        if (idx == -1)
            throw new ArgumentException("Tried to consume a consumable that was not in the inventory.");

        var result = consumable.Consume(Parent);
        if (!result) return false;

        Items.RemoveAt(idx);
        return true;
    }

    public bool EquipWeapon(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Cannot equip item from an inventory not attached to an object.");
        var weapon = item.AllComponents.GetFirst<WeaponComponent>();
        var idx = Items.FindIndex(i => i == item);
        if (idx == -1)
            throw new ArgumentException("Tried to equip an equipment that was not in the inventory.");

        if (weapon.IsEquipped)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new(
                "You are already wielding this.",
                MessageColors.ImpossibleActionAppearance));
            return false;
        }

        // Unequip all other weapons
        var alreadyEquipped = Items.Where(i => i.AllComponents.Contains<WeaponComponent>()).ToList();
        if (alreadyEquipped.Count > 0)
        {
            foreach (var e in alreadyEquipped)
            {
                e.AllComponents.GetFirst<WeaponComponent>().Unequip();
            }
        }

        var result = weapon.Equip();
        return result;
    }

    public bool EquipArmor(RogueLikeEntity item)
    {
        if (Parent == null)
            throw new InvalidOperationException("Cannot equip item from an inventory not attached to an object.");
        var armor = item.AllComponents.GetFirst<ArmorComponent>();
        var idx = Items.FindIndex(i => i == item);
        if (idx == -1)
            throw new ArgumentException("Tried to equip an equipment that was not in the inventory.");

        if (armor.IsEquipped)
        {
            Engine.GameScreen?.MessageLog.AddMessage(new(
                "You are already wearing this.",
                MessageColors.ImpossibleActionAppearance));
            return false;
        }

        // Unequip all other weapons
        var alreadyEquipped = Items.Where(i => i.AllComponents.Contains<ArmorComponent>()).ToList();
        if (alreadyEquipped.Count > 0)
        {
            foreach (var e in alreadyEquipped)
            {
                e.AllComponents.GetFirst<ArmorComponent>().Unequip();
            }
        }

        var result = armor.Equip();
        return result;
    }

    public void UnequipDrop(RogueLikeEntity item)
    {
        var isWeapon = item.AllComponents.GetFirstOrDefault<WeaponComponent>();
        if (isWeapon != null)
        {
            isWeapon.Unequip();
            return;
        }

        var isArmor = item.AllComponents.GetFirstOrDefault<ArmorComponent>();
        if (isArmor != null)
        {
            isArmor.Unequip();
        }
    }
}
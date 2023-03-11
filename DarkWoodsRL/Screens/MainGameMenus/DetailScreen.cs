using System;
using System.Collections.Generic;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace DarkWoodsRL.Screens.MainGameMenus;

public class DetailScreen : MainGameMenu
{
    public Label ItemType;
    public List<Label> Details = new() {new Label(28), new Label(28), new Label(28), new Label(28), new Label(28)};

    public DetailScreen(RogueLikeEntity item, int type) : base(30, 11)
    {
        Title = item.Name.Replace(" (e)", "");
        var playerInventory = Engine.Player.AllComponents.GetFirst<InventoryComponent>();

        ItemType = new Label("Type: Weapon")
        {
            Position = (1, 1)
        };
        Controls.Add(ItemType);

        for (var i = 0; i < Details.Count; i++)
        {
            Details[i].Position = (1, 2 + i);
            Controls.Add(Details[i]);
        }

        var action = new Button(7)
        {
            Text = ParseType(type),
            Position = (1, 10)
        };
        action.Click += (sender, args) => Use(sender, args, item, type, playerInventory);
        Controls.Add(action);

        var drop = new Button(6)
        {
            Text = "Drop",
            Position = (9, 10)
        };
        drop.Click += (sender, args) => Drop(sender, args, item, playerInventory);
        Controls.Add(drop);

        ParseDetails(item);
    }

    private void ParseDetails(RogueLikeEntity item)
    {
        var details = item.AllComponents.GetFirst<DetailsComponent>();
        ItemType.DisplayText = "Type: " + details.Type;
        for (var i = 0; i < details.Description.Length; i++)
        {
            Details[i].DisplayText = details.Description[i];
        }
    }

    private void Drop(object? sender, EventArgs args, RogueLikeEntity item, InventoryComponent inv)
    {
        Game.Instance.Screen.Children[^2].IsVisible = false;
        Hide();
        inv.UnequipDrop(item);
        PlayerActionHelper.PlayerTakeAction(_ =>
        {
            inv.Drop(item);
            return true;
        });
    }

    private void Use(object? sender, EventArgs args, RogueLikeEntity item, int type, InventoryComponent playerInventoryComponent)
    {
        switch (type)
        {
            case 0:
            {
                var consumable = item.AllComponents.GetFirstOrDefault<IConsumable>();
                if (consumable != null)
                {
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventoryComponent.Consume(item));
                }

                Game.Instance.Screen.Children[^2].IsVisible = false;
                Hide();
                break;
            }
            case 1:
            {
                var weapon = item.AllComponents.GetFirstOrDefault<IWeapon>();
                if (weapon != null)
                {
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventoryComponent.EquipWeapon(item));
                }

                var armor = item.AllComponents.GetFirstOrDefault<IArmor>();
                if (armor != null)
                {
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventoryComponent.EquipArmor(item));
                }

                Game.Instance.Screen.Children[^2].IsVisible = false;
                Hide();
                break;
            }
        }
    }

    private string ParseType(int type)
    {
        return type switch
        {
            // Non-equipment
            0 => "Use",
            1 => "Equip",
            _ => string.Empty
        };
    }
}
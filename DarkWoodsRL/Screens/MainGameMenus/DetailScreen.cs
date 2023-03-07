using System;
using System.Linq;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace DarkWoodsRL.Screens.MainGameMenus;

public class DetailScreen : MainGameMenu
{
    private RogueLikeEntity _item;

    public DetailScreen(RogueLikeEntity item, int type) : base(20, 10)
    {
        Title = item.Name.Replace(" (e)", "");
        var playerInventory = Engine.Player.AllComponents.GetFirst<Inventory>();
        var action = new Button(7)
        {
            Text = ParseType(type),
            Position = (1, 9)
        };
        action.Click += (sender, args) =>  Use(sender, args, item, type, playerInventory);
        Controls.Add(action);

        var drop = new Button(6)
        {
            Text = "Drop",
            Position = (9, 9)
        };
        Controls.Add(drop);
    }

    private void Use(object sender, EventArgs args, RogueLikeEntity item, int type, Inventory playerInventory)
    {
        switch (type)
        {
            case 0:
            {
                var consumable = item.AllComponents.GetFirstOrDefault<IConsumable>();
                if (consumable != null)
                {
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventory.Consume(item));
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
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventory.EquipWeapon(item));
                }

                var armor = item.AllComponents.GetFirstOrDefault<IArmor>();
                if (armor != null)
                {
                    PlayerActionHelper.PlayerTakeAction(_ => playerInventory.EquipArmor(item));
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
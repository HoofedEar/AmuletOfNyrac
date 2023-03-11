using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Interfaces;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace DarkWoodsRL.Screens.MainGameMenus;

/// <summary>
/// Menu used by the player to select a consumable from their inventory to use.
/// </summary>
internal class ConsumeScreen : MainGameMenu
{
    private readonly InventoryComponent _playerInventoryComponent;

    public ConsumeScreen()
        : base(51, 15)
    {
        Title = "Select an item to consume:";

        _playerInventoryComponent = Engine.Player.AllComponents.GetFirst<InventoryComponent>();
        if (_playerInventoryComponent.Items.Count == 0)
        {
            PrintTextAtCenter("There are no items in your inventory.");
            return;
        }

        // Find any consumable items and add them to a ListBox
        bool foundItem = false;
        var list = new ListBox(Width - 2, Height - 2) { Position = (1, 1), SingleClickItemExecute = true };

        foreach (var item in _playerInventoryComponent.Items)
        {
            var consumable = item.AllComponents.GetFirstOrDefault<IConsumable>();
            if (consumable == null) continue;

            foundItem = true;
            list.Items.Add(new ListItem { Item = item });
        }

        if (!foundItem)
            PrintTextAtCenter("There are no consumable items in your inventory.");
        else
            Controls.Add(list);

        // Handle when an item is selected by using it.
        list.SelectedItemExecuted += OnItemSelected;
    }

    private void OnItemSelected(object? sender, ListBox.SelectedItemEventArgs e)
    {
        Hide();

        var item = ((ListItem)e.Item).Item;
        PlayerActionHelper.PlayerTakeAction(_ => _playerInventoryComponent.Consume(item));
    }
}
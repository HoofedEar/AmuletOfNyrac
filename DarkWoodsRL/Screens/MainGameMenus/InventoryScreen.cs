using System.Threading.Tasks;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
using SadConsole.Input;
using SadConsole.UI.Controls;
using SadRogue.Integration;

namespace DarkWoodsRL.Screens.MainGameMenus;

/// <summary>
/// A wrapper around a RogueLikeEntity, which ensures that when it is displayed in a menu, it is displayed as its Name field.
/// </summary>
internal class ListItem
{
    public RogueLikeEntity Item { get; init; } = null!;

    public override string ToString()
    {
        return Item.Name;
    }
}

/// <summary>
/// Menu used by the player to select a consumable from their inventory to use.
/// </summary>
internal class InventoryScreen : MainGameMenu
{
    private readonly Inventory _playerInventory;
    private readonly ListBox? _itemList;

    public InventoryScreen()
        : base(30, 15)
    {
        Title = "Inventory";

        _playerInventory = Engine.Player.AllComponents.GetFirst<Inventory>();
        if (_playerInventory.Items.Count == 0)
        {
            PrintTextAtCenter("No items in your inventory.");
            return;
        }

        // Find any consumable items and add them to a ListBox
        _itemList = new ListBox(Width - 2, Height - 2) {Position = (1, 1), SingleClickItemExecute = true};

        foreach (var item in _playerInventory.Items)
        {
            _itemList.Items.Add(new ListItem {Item = item});
        }

        Controls.Add(_itemList);

        // Handle when an item is selected by using it.
        _itemList.SelectedItemExecuted += OnItemSelected;
    }

    private void OnItemSelected(object? sender, ListBox.SelectedItemEventArgs e)
    {
        var item = ((ListItem) e.Item).Item;
        _itemList!.SelectedItem = null;
        var type = 0;
        if (item.AllComponents.Contains<IArmor>() || item.AllComponents.Contains<IWeapon>())
            type = 1;
        var desc = new DetailScreen(item, type);
        desc.Show();
    }
    
    public override bool ProcessKeyboard(Keyboard info)
    {
        if (!info.HasKeysPressed) return base.ProcessKeyboard(info);
        if (!info.IsKeyPressed(Keys.Escape)) return base.ProcessKeyboard(info);

        Hide();
        Engine.GameScreen!.Map.IsFocused = true;

        return base.ProcessKeyboard(info);
    }
}
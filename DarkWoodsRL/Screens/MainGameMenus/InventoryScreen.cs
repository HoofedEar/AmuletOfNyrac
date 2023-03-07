using System.Threading.Tasks;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Items;
using DarkWoodsRL.MapObjects.Components.Items.Armor;
using DarkWoodsRL.MapObjects.Components.Items.Weapon;
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
        var list = new ListBox(Width - 2, Height - 2) {Position = (1, 1), SingleClickItemExecute = true};

        foreach (var item in _playerInventory.Items)
        {
            list.Items.Add(new ListItem {Item = item});
        }

        Controls.Add(list);

        // Handle when an item is selected by using it.
        list.SelectedItemExecuted += OnItemSelected;
    }

    private void OnItemSelected(object? sender, ListBox.SelectedItemEventArgs e)
    {
        var item = ((ListItem) e.Item).Item;
        var type = 0;
        if (item.AllComponents.Contains<IArmor>() || item.AllComponents.Contains<IWeapon>())
            type = 1;
        var desc = new DetailScreen(item, type);
        desc.Show();
    }
}
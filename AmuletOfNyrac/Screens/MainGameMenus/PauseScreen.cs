using System.Threading.Tasks;
using SadConsole;
using SadConsole.UI.Controls;

namespace AmuletOfNyrac.Screens.MainGameMenus;

internal class PauseView : MainGameMenu
{
    public PauseView() : base(11, 6)
    {
        // This is silly but we have to do this, or the window will immediately close because its invoked with Escape
        CloseOnEscKey = false;
        Title = "PAUSED";
        
        var list = new ListBox(Width - 2, Height - 2) {Position = (1, 1), SingleClickItemExecute = true};
        //list.Items.Add("About");
        list.Items.Add("Exit");
        
        Controls.Add(list);
        list.SelectedItemExecuted += OnItemSelected;
    }
    
    private void OnItemSelected(object? sender, ListBox.SelectedItemEventArgs e)
    {
        if ((string) e.Item! == "Exit")
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        Hide();
    }

    public override async void OnFocused()
    {
        await Task.Delay(200);
        CloseOnEscKey = true;
        base.OnFocused();
    }
}
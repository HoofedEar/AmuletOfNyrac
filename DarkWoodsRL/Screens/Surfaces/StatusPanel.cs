using System;
using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Combatant;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using SadRogue.Primitives;

namespace DarkWoodsRL.Screens.Surfaces;

/// <summary>
/// A ControlsConsole subclass which resides on the main game screen and displays the player's health and similar "hud" statistics.
/// </summary>
internal class StatusPanel : ControlsConsole
{
    public readonly ProgressBar HPBar;
    public readonly Label STRStat;
    public readonly Label DEXStat;
    public readonly Label ENDStat;
    public readonly Label GoldAmount;

    public StatusPanel(int width, int height)
        : base(width, height)
    {
        // Create an HP bar with the appropriate coloring and background glyphs
        HPBar = new ProgressBar(Width, 1, HorizontalAlignment.Left)
        {
            DisplayTextColor = Color.White
        };
        HPBar.SetThemeColors(Themes.StatusPanel.HPBarColors);
        ((ProgressBarTheme) HPBar.Theme).Background.SetGlyph(' ');

        // Add HP bar to controls, and ensure HP bar updates when the player's health changes
        Controls.Add(HPBar);
        Engine.Player.AllComponents.GetFirst<CombatantComponent>().HPChanged += OnPlayerHPChanged;
        UpdateHPBar();

        STRStat = new Label("STR 00")
        {
            Position = (1, 2)
        };
        Engine.Player.AllComponents.GetFirst<CombatantComponent>().STRChanged += OnPlayerStatsChanged;
        Controls.Add(STRStat);

        DEXStat = new Label("DEX 10")
        {
            Position = (8, 2)
        };
        Engine.Player.AllComponents.GetFirst<CombatantComponent>().DEXChanged += OnPlayerStatsChanged;
        Controls.Add(DEXStat);

        ENDStat = new Label("END 10")
        {
            Position = (1, 3)
        };
        Engine.Player.AllComponents.GetFirst<CombatantComponent>().ENDChanged += OnPlayerStatsChanged;
        Controls.Add(ENDStat);
        UpdateStats();

        GoldAmount = new Label("$:0   ")
        {
            Position = (8, 3)
        };
        Engine.Player.AllComponents.GetFirst<InventoryComponent>().GoldChanged += OnPlayerGoldChanged;
        Controls.Add(GoldAmount);
    }

    private void OnPlayerGoldChanged(object? sender, EventArgs e)
    {
        UpdateGold();
    }
    
    private void OnPlayerStatsChanged(object? sender, EventArgs e)
    {
        UpdateStats();
    }

    private void OnPlayerHPChanged(object? sender, EventArgs e)
    {
        UpdateHPBar();
    }

    private void UpdateGold()
    {
        var inventory = Engine.Player.AllComponents.GetFirst<InventoryComponent>();
        GoldAmount.DisplayText = $"$:" + inventory.Gold;
    }

    private void UpdateHPBar()
    {
        var combatant = Engine.Player.AllComponents.GetFirst<CombatantComponent>();
        HPBar.DisplayText = $"HP: {combatant.HP} / {combatant.MaxHP}";
        HPBar.Progress = (float) combatant.HP / combatant.MaxHP;
    }

    private void UpdateStats()
    {
        var combatant = Engine.Player.AllComponents.GetFirst<CombatantComponent>();
        var baseSTR = combatant.STR;
        var baseDEX = combatant.DEX;
        var baseEND = combatant.END;
        STRStat.DisplayText = "STR " + (baseSTR);
        DEXStat.DisplayText = "DEX " + (baseDEX);
        ENDStat.DisplayText = "END " + (baseEND);
    }
}
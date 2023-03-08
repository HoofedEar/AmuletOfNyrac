using System;
using DarkWoodsRL.MapObjects.Components;
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
        Engine.Player.AllComponents.GetFirst<Combatant>().HPChanged += OnPlayerHPChanged;
        UpdateHPBar();

        STRStat = new Label("STR 00")
        {
            Position = (1, 1)
        };
        Engine.Player.AllComponents.GetFirst<Combatant>().STRChanged += OnPlayerSTRChanged;
        Controls.Add(STRStat);

        DEXStat = new Label("DEX 10")
        {
            Position = (1, 2)
        };
        Engine.Player.AllComponents.GetFirst<Combatant>().DEXChanged += OnPlayerSTRChanged;
        Controls.Add(DEXStat);

        ENDStat = new Label("END 10")
        {
            Position = (1, 3)
        };
        Engine.Player.AllComponents.GetFirst<Combatant>().ENDChanged += OnPlayerSTRChanged;
        Controls.Add(ENDStat);
        UpdateStats();

        var gold = new Label("Gold: 0000")
        {
            Position = (9, 2)
        };
        Controls.Add(gold);
    }

    private void OnPlayerSTRChanged(object? sender, EventArgs e)
    {
        UpdateStats();
    }

    private void OnPlayerHPChanged(object? sender, EventArgs e)
    {
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        var combatant = Engine.Player.AllComponents.GetFirst<Combatant>();
        HPBar.DisplayText = $"HP: {combatant.HP} / {combatant.MaxHP}";
        HPBar.Progress = (float) combatant.HP / combatant.MaxHP;
    }

    private void UpdateStats()
    {
        var combatant = Engine.Player.AllComponents.GetFirst<Combatant>();
        var inventory = Engine.Player.AllComponents.GetFirst<Inventory>();
        var baseSTR = combatant.STR;
        var baseDEX = combatant.DEX;
        var baseEND = combatant.END;
        STRStat.DisplayText = "STR " + (baseSTR);
        DEXStat.DisplayText = "DEX " + (baseDEX);
        ENDStat.DisplayText = "END " + (baseEND);
    }
}
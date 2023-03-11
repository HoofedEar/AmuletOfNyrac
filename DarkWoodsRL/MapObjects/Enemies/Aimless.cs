using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Combatant;
using DarkWoodsRL.MapObjects.Components.EnemyAI;
using DarkWoodsRL.Maps;
using DarkWoodsRL.Themes;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Enemies;

public static class Aimless
{
    public static RogueLikeEntity Bumbler()
    {
        var enemy = new RogueLikeEntity(MainPalette.Sage, Color.Black, 'b', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Bumbler"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new AimlessAI());
        enemy.AllComponents.Add(new CombatantComponent(5, 0, 3, combatVerb: "rambles at"));

        return enemy;
    }

    public static RogueLikeEntity Rat()
    {
        var enemy = new RogueLikeEntity(MainPalette.LightGray, Color.Black, 'r', false,
            layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Rat"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new AimlessAI());
        enemy.AllComponents.Add(new CombatantComponent(8, 1, 2, 8, combatVerb: "tries to bite"));

        return enemy;
    }
    
    /// <summary>
    /// He makes all the rules
    /// </summary>
    public static RogueLikeEntity GaintRat()
    {
        var enemy = new RogueLikeEntity(MainPalette.LightGray, Color.Black, 'R', false,
            layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Gaint Rat Who Makes All Da Rulez"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new AimlessAI());
        enemy.AllComponents.Add(new CombatantComponent(25, 1, 3, 12, combatVerb: "tries to bite"));

        return enemy;
    }
}
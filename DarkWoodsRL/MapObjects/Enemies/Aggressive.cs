using DarkWoodsRL.MapObjects.Components;
using DarkWoodsRL.MapObjects.Components.Combatant;
using DarkWoodsRL.MapObjects.Components.EnemyAI;
using DarkWoodsRL.Maps;
using DarkWoodsRL.Themes;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace DarkWoodsRL.MapObjects.Enemies;

public static class Aggressive
{
    public static RogueLikeEntity Orc()
    {
        var enemy = new RogueLikeEntity(MainPalette.Sage, Color.Black, 'o', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Orc"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new AimlessAI());
        enemy.AllComponents.Add(new CombatantComponent(10, 0, 3));

        return enemy;
    }

    public static RogueLikeEntity Troll()
    {
        var enemy = new RogueLikeEntity(MainPalette.LightGray, Color.Black, 'T', false,
            layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Troll"
        };

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new AimlessAI());
        enemy.AllComponents.Add(new CombatantComponent(16, 1, 4, combatVerb: "tries to bludgeon"));

        return enemy;
    }
}
using AmuletOfNyrac.MapObjects.Components.Combatant;
using AmuletOfNyrac.MapObjects.Components.EnemyAI;
using AmuletOfNyrac.Maps;
using AmuletOfNyrac.Themes;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace AmuletOfNyrac.MapObjects.Enemies;

public static class Hostile
{
    public static RogueLikeEntity Wuff()
    {
        var enemy = new RogueLikeEntity(Color.Plum, Color.Black, 'W', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Wuff"
        }; 

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new CombatantComponent(15, 0, 3, dexterity: 7, combatVerb: "bites at", xp: 20));

        return enemy;
    }
    
    public static RogueLikeEntity Glomper()
    {
        var enemy = new RogueLikeEntity(MainPalette.Lime, Color.Black, 'G', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Glomper"
        }; 

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new CombatantComponent(15, 2, 3, combatVerb: "glomps", xp: 20));

        return enemy;
    }
    
    public static RogueLikeEntity Owlbear()
    {
        var enemy = new RogueLikeEntity(Color.BlueViolet, Color.Black, 'O', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Owlbear"
        }; 

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new CombatantComponent(15, 2, 6, combatVerb: "slashes at", xp: 25));

        return enemy;
    }
    
    public static RogueLikeEntity Dragoon()
    {
        var enemy = new RogueLikeEntity(Color.Red, Color.Black, 'D', false, layer: (int) GameMap.Layer.Monsters)
        {
            Name = "Dragoon"
        }; 

        // Add AI component to bump action toward the player if the player is in view
        enemy.AllComponents.Add(new HostileAI());
        enemy.AllComponents.Add(new CombatantComponent(15, 2, 7, 8, combatVerb: "slashes at", xp: 25));

        return enemy;
    }
}
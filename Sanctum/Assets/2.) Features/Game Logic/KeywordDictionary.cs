
public static class  KeywordDictionary 
{
    public enum ItemType
    {
        Weapon, 
        Armor, 
        Consumable, 
        Perk
    };

    public enum Scenes
    {
        GameManager = 0,
        MainMenu = 1,
        GamePlay = 2,
        GameUI = 3,
        Testing = 4,
        Sandbox = 5
    };

    /// <summary>
    /// These must match the name of the actual game objects in the scene or else the camera
    /// system won't actually switch to them.
    /// </summary>
    public enum Cameras
    {
        vcam_Normal, // third person camera
        vcam_ADS // ADS third person camera
    }

    public enum MainMenuPanel
    {
        Home,
        Settings,
        PlayerSelection,
        CreateCharacter
    }
    
    public enum PlayerCharacterType
    {
        Balanced, // well roudned stats
        Fast, // fast movement speed with high energy burn
        Tough, // high health with slow movement speed
        Athletic, // high energy with low sprint speed
    }

    public enum EnemyType
    {
        Walker, // slow chase
        Runner, // fast chase
        Screamer, // spawner
        Bloater, // blow up
        Shooter, // basic ranged
        Sniper, // high dmg ranged
        Riot, // carrying shields, 
    }

    public enum EnemyArchType
    {
        Infected, // stinks, poisonous aura that dmgs
        Armored, // shielded
        Drowned, // slows on hit
        Charred, // burned, fire auro that dmgs
    }
}

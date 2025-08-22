
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

    public enum Cameras
    {
        vcam_Normal,
        vcam_ADS
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
}

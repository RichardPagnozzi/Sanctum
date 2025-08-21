
[System.Serializable]
public class PlayerDetails
{

    public KeywordDictionary.PlayerCharacterType CharacterType;
    public PlayerStats Stats;



    // Constructor to be used when creating a NEW character
    public PlayerDetails(KeywordDictionary.PlayerCharacterType characterType)
    {
        CharacterType = characterType;
        ResetToBaseStats();
    }
    
    // Constructor to be used when creating a From-Exisintg character
    public PlayerDetails(PlayerStats stats, KeywordDictionary.PlayerCharacterType characterType )
    {
        Stats = stats;
        CharacterType = characterType;
    }
    
    private void ResetToBaseStats()
    {
        switch (CharacterType)
        {
            case KeywordDictionary.PlayerCharacterType.Balanced:
            {
                Stats = new CharacterBalancedStats();
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Fast:
            {
                Stats = new CharacterFastStats();
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Tough:
            {
                Stats = new CharacterToughStats();
                break;
            }
            case KeywordDictionary.PlayerCharacterType.Athletic:
            {
                Stats = new CharacterAthleticStats();
                break;
            }
        }
    }

}
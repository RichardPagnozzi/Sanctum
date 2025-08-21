using System;
using UnityEngine;

public class PlayerRepository
{
    private const string PlayerPrefsKey_PlayerInitialized = "PlayerInitialized";
    private const string PlayerPrefsKey_PlayerStats = "PlayerStats";
    private const string PlayerPrefsKey_CharacterType = "PlayerCharacterType";

    public PlayerDetails CurrentSessionPlayerDetails { get; private set; }
    private PlayerStats CurrentSessionPlayerStats;
    private KeywordDictionary.PlayerCharacterType CurrentSessionCharacterType;

    

    private bool LoadPlayerDetails()
    {
        try
        {
            // Do we have a saved player ?
            if (PlayerPrefs.HasKey(PlayerPrefsKey_PlayerInitialized))
            {
                // Assign Stats
                if (PlayerPrefs.HasKey(PlayerPrefsKey_PlayerStats))
                {
                    string json = PlayerPrefs.GetString(PlayerPrefsKey_PlayerStats); 
                    CurrentSessionPlayerStats = JsonUtility.FromJson<PlayerStats>(json);
                }

                // Assign Type
                if (PlayerPrefs.HasKey(PlayerPrefsKey_CharacterType))
                {
                    string json = PlayerPrefs.GetString(PlayerPrefsKey_CharacterType); 
                    CurrentSessionCharacterType = JsonUtility.FromJson<KeywordDictionary.PlayerCharacterType>(json);
                }

                // Set local player details using loaded data
                SetPlayerDetails(new PlayerDetails(CurrentSessionPlayerStats,
                    CurrentSessionCharacterType)); 
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            DebugLogger.Log($"Error in Player Repository. Exception Caught in LoadPlayerDetails -> {e}",
                DebugLogger.LogStyle.Normal, Color.red);
            return false;
        }
    }

    private void SavePlayerDetails()
    {
        string jsonStats = JsonUtility.ToJson(CurrentSessionPlayerDetails.Stats);
        string jsonCharacterType = JsonUtility.ToJson(CurrentSessionPlayerDetails.CharacterType);
        PlayerPrefs.SetString(PlayerPrefsKey_PlayerStats, jsonStats);
        PlayerPrefs.SetString(PlayerPrefsKey_CharacterType, jsonCharacterType);
        PlayerPrefs.SetInt(PlayerPrefsKey_PlayerInitialized, 0);
    }

    private void SetPlayerDetails(PlayerDetails details)
    {
        CurrentSessionPlayerDetails = details;
        SavePlayerDetails();
    }
    
    public void InitializeNewPlayer(KeywordDictionary.PlayerCharacterType characterType)
    {
        SetPlayerDetails(new PlayerDetails(characterType));
    }
    
    public bool TryLoadPlayer()
    {
        if (LoadPlayerDetails())
        {
            DebugLogger.Log($"Successfully Loaded Player", DebugLogger.LogStyle.Bold, Color.green);
            return true;
        }

        DebugLogger.Log($"Failed Loading Player: No Player Found", DebugLogger.LogStyle.Bold, Color.red);
        return false;
    }
    
}
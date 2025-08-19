using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ServiceLocator ServiceLocator;
    public PlayerRepository PlayerRepository;
    public bool IsInitialized { get; private set; }
    
    private void Awake()
    {
        IsInitialized = false;
        SingletonBuilder(this);
        Instance = this;
        IsInitialized = Initialize();
        if (IsInitialized)
        {
            DebugLogger.Log($"GameManager Initialized Successfully", DebugLogger.LogStyle.Bold, Color.green);
        }
        else
        {
            DebugLogger.Log($"GameManager Initialization Failed ", DebugLogger.LogStyle.Bold, Color.red);
        }
    }

    private bool Initialize()
    {
        try
        {
            ServiceLocator = gameObject.AddComponent<ServiceLocator>();
            ServiceLocator.Initialize();
            PlayerRepository = new PlayerRepository();
            PlayerRepository.TryLoadPlayer();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
}

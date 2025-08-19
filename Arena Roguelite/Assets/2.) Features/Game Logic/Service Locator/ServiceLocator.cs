using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServiceLocator : MonoBehaviour, IServiceLocator
{
    private IDictionary<Type, MonoBehaviour> services;

    public IDictionary<Type, MonoBehaviour> Services
    {
        get { return Services; }
    }

    // Services
    public EventManager EventManager { get; set; }

    private ServiceLocator _instance;

    public ServiceLocator Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }


    public T GetService<T>() where T : MonoBehaviour, new()
    {
        UnityEngine.Assertions.Assert.IsNotNull(services,
            "Someone has requested a service prior to the locator's intialization.");

        bool serviceLocated = services.ContainsKey(typeof(T));
        if (!serviceLocated)
        {
            services.Add(typeof(T), FindObjectOfType<T>());
        }

        UnityEngine.Assertions.Assert.IsTrue(services.ContainsKey(typeof(T)), "Could not find service: " + typeof(T));
        T service = (T)services[typeof(T)];
        UnityEngine.Assertions.Assert.IsNotNull(service, typeof(T).ToString() + " could not be found.");
        return service;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (services == null)
        {
            services = new Dictionary<Type, MonoBehaviour>();
        }
        if (EventManager == null)
        {
            EventManager = new EventManager();
        }
    }

    public void Initialize()
    {
        Instance = this;
        services = new Dictionary<Type, MonoBehaviour>();
        EventManager = new EventManager();
    }
}
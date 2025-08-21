using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IServiceLocator 
    {
     IDictionary<Type, MonoBehaviour> Services { get; }
    EventManager EventManager { get; }

    public abstract T GetService<T>() where T : MonoBehaviour, new();
    }


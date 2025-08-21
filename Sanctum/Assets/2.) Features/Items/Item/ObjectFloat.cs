using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{
    // User Inputs
    [SerializeField][Header("Leave Empty to float this object")]
    private GameObject Item;
    
    private float degreesPerSecond = 15.0f;
    private float amplitude = 0.1f;
    private float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Awake()
    {
        if (Item == null)
        {
            Item = gameObject;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = Item.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        Item.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        Item.transform.position = tempPos;
    }
}

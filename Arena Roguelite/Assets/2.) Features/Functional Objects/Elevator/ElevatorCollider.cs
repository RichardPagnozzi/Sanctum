using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCollider : MonoBehaviour
{
    [SerializeField] private ElevatorController _elevatorController;
    private void OnTriggerEnter(Collider other)
    {
        if (_elevatorController == null)
            return;
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("collided");
                _elevatorController.MoveElevator();
        }
    }
}

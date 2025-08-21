using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private RoomAttributes.RoomDifficultyName _difficulty;
    [SerializeField]
    private RoomAttributes.RoomTypeName _type;
    [Space]
    [SerializeField]
    Transform _roomEntrance;
    [SerializeField]
    Transform _roomExit;

    private RoomAttributes _roomAttributes;
    public RoomAttributes roomAttributes
    {
        get { return _roomAttributes; }
    }
    private bool _isRoomComplete = false;

    private void Awake()
    {
        _roomAttributes = new RoomAttributes();
        _roomAttributes.InitializeRoom(_type, _difficulty);
    }

    public Transform GetRoomEntrance()
    {
        return _roomEntrance;
    }
    public Transform GetRoomExit()
    {
        return _roomExit; 
    }

    public bool GetCompletionStatus()
    {
        return _isRoomComplete;
    }
}

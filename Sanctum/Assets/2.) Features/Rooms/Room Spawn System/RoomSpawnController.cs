using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnController : MonoBehaviour
{
    #region Members
    // Serialized
    [SerializeField]
    private List<Room> _rooms;
    [SerializeField]
    private Room _statisRoom;
    [SerializeField]
    private Transform _roomContainer;
    // Private
    private Room _currentRoom;
    private Room _previousRoom;
    private Room _nextRoom;
    private float _roomSpawnOffset;
    private int _spawnIndex = 0;
    private int _totalRooms => Rooms.Count;
    private const int TotalRoomsPerStage = 10;
    private int _currentRoomNumber = 1;
    // Public 
    public List<Room> Rooms
    {
        private set => _rooms = value;
        get => _rooms;
    }
    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        if (_currentRoom == null)
        {
            _currentRoom = _statisRoom;
            _previousRoom = null;
        }
    }
    private void Start()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnRoomExit += CycleRooms;
    }
    private void OnDisable()
    {
        GameManager.Instance.ServiceLocator.EventManager.OnRoomExit -= CycleRooms;
    }
    #endregion

    #region Methods
    private void RandomizeRoomSelection()
    {
        _spawnIndex = Random.Range(0, _totalRooms);
    }
    private void SetNextRoomPosition(Room nextRoom)
    {
        _roomSpawnOffset = Mathf.Abs(_currentRoom.GetRoomExit().localPosition.x) + Mathf.Abs(nextRoom.GetComponent<Room>().GetRoomEntrance().localPosition.x);
        Vector3 currentPosition = _currentRoom.transform.position;
        Vector3 spawnPosition = new Vector3(currentPosition.x + _roomSpawnOffset, currentPosition.y, currentPosition.z);

        nextRoom.transform.position = spawnPosition;
        nextRoom.transform.rotation = Quaternion.identity;
    }
    [ContextMenu("Spawn Room")]
    private void SpawnNextRoom()
    {
        if (_previousRoom != null && _previousRoom != _statisRoom)
        {
            Rooms.RemoveAt(_spawnIndex);
            Destroy(_previousRoom.gameObject);
        }
        RandomizeRoomSelection();
        GameObject nextRoom = Instantiate(Rooms[_spawnIndex].gameObject, _roomContainer);
        SetNextRoomPosition(nextRoom.GetComponent<Room>());
        _previousRoom = _currentRoom;
        _currentRoom = nextRoom.GetComponent<Room>();
        _statisRoom.gameObject.SetActive(false);
    }
    [ContextMenu("Spawn Statis")]
    private void SpawnStatisRoom()
    {
        _previousRoom = _currentRoom;
        _currentRoom = _statisRoom.GetComponent<Room>();

        _roomSpawnOffset = Mathf.Abs(_previousRoom.GetRoomEntrance().localPosition.x) + Mathf.Abs(_statisRoom.GetComponent<Room>().GetRoomExit().localPosition.x);
        Vector3 currentStatisRoomPos = _previousRoom.transform.position;
        Vector3 targetStatisRoomPos = new Vector3(currentStatisRoomPos.x + _roomSpawnOffset, currentStatisRoomPos.y, currentStatisRoomPos.z);

        _statisRoom.transform.position = targetStatisRoomPos;
        _statisRoom.transform.rotation = Quaternion.identity;
        _statisRoom.gameObject.SetActive(true);
    }

    private void CycleRooms()
    {
        if (_currentRoomNumber < TotalRoomsPerStage)
        {
            if(_currentRoomNumber % 2 != 0)
            {
                SpawnNextRoom();
            }
            else
            {
                SpawnStatisRoom();
            }
            _currentRoomNumber++;
        }
        else
        {
            Debug.Log("STAGE FINISHED - ALL ROOMS COMPLETE");
        }
    }
    #endregion 

}

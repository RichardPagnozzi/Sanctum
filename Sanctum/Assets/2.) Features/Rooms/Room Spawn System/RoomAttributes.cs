using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomAttributes 
{

    public enum RoomTypeName { Combat, Statis, Challenge}; 
    public enum RoomDifficultyName { Easy, Medium, Hard, Insane}
    
    public RoomTypeName RoomType;
    public RoomDifficultyName RoomDifficulty;
    

    public void InitializeRoom(RoomTypeName roomType, RoomDifficultyName roomDifficulty)
    {
        RoomType = roomType;
        RoomDifficulty = roomDifficulty;
    }

}

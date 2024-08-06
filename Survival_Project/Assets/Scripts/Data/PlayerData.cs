using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats;                         // [0] - Health, [1] - Calories, [2] - Hydration
    public float[] playerPositionAndRotation;           // position x, y, z and rotation x, y, z
    public float[] cameraPositionAndRotation; 
    public string[] inventoryContent;
    public string[] quickSlotContent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot, float[] _cameraPositionAndRotation, string[] _inventoryContent, string[] _quickSlotContent)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
        cameraPositionAndRotation = _cameraPositionAndRotation;
        inventoryContent = _inventoryContent;
        quickSlotContent = _quickSlotContent;
    }





}

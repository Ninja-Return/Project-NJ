using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirationType
{

    Left,
    Right,
    Forward,
    Back

}

[System.Serializable]
public struct CellData
{

    public Guid mapGuid;
    public List<DirationType> doorData;

}


public class MapGenerater : MonoBehaviour
{

    [SerializeField] private List<Room> rooms;
    [SerializeField] private List<Room> roads;
    [SerializeField] private List<Room> constRoom;

}

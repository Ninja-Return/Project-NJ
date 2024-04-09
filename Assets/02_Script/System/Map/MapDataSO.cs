using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum MapCell
{

    None = 0,
    Left = 1,
    Right = 2,
    Forward = 4,
    Back = 8,
    Room = 32,

}

[System.Serializable]
public struct Array2D<T>
{

    public Vector2Int size;

    public Array2D(int sizeX, int sizeY)
    {

        array = new T[sizeX * sizeY];
        size = new Vector2Int(sizeX, sizeY);

    }

    public T[] array;
    
    public T this[int x, int y]
    {

        get
        {

            return array[y * size.x + x];

        }
        set
        {

            array[y * size.x + x] = value;

        }

    }


}

public class MapDataSO : ScriptableObject
{

    [field:SerializeField] public Array2D<MapCell> data { get; private set; }    

    public void SetUp(Array2D<MapCell> data)
    {

        this.data = data;

    }

}

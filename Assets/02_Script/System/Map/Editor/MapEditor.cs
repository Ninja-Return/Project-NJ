using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

public class CellVisual : VisualElement
{

    private Vector2Int index;
    private int current;
    Func<int, Vector2Int, MapCell> cellChangeCallBack;

    public CellVisual(Vector2 pos, Vector2 size, Vector2Int index, int current,
        Func<int, Vector2Int, MapCell> cellChangeCallBack, Vector2 winsize)
    {

        this.index = index;
        this.cellChangeCallBack = cellChangeCallBack;

        style.width = size.x;
        style.height = size.y;

        style.position = Position.Absolute;
        transform.position = pos + new Vector2(1920, 1080 / 2) / 4;
        RegisterCallback<PointerDownEvent>(HandleClick);

        ChangeVisual(0);

    }

    private void HandleClick(PointerDownEvent evt)
    {

        //哭
        if(evt.button == 0)
        {

            if(current < 15)
            {

                current++;

            }
            else
            {

                current = 0;

            }

        }
        //坷
        else if(evt.button == 1)
        {

            if(current != 32)
            {

                current = 32;

            }

        }


        cellChangeCallBack(current, index);
        ChangeVisual(current);

    }

    private void ChangeVisual(int value)
    {

        //style.backgroundColor = UnityEngine.Random.ColorHSV();

        var old = style.backgroundImage;
        old.value = Background.FromTexture2D(Resources.Load<Texture2D>($"MapEd/{value}"));
        style.backgroundImage = old;

    }

}

public class MapEditor : EditorWindow
{

    private Button btnSave;
    private Button btnCreate;
    private TextField widthField;
    private TextField heightField;
    private MapCell[,] mapData;
    private List<CellVisual> cellVisuals = new();

    [MenuItem("Window/MapEditor")]
    public static void OpenWindow()
    {

        CreateWindow<MapEditor>();
        
    }

    private void OnEnable()
    {

        btnSave = new Button(HandleSave);
        btnSave.text = "历厘";

        btnCreate = new Button(HandleCreate);
        btnCreate.text = "积己";

        widthField = new TextField("Width");
        heightField = new TextField("Height");

        rootVisualElement.Add(btnSave);
        rootVisualElement.Add(btnCreate);
        rootVisualElement.Add(widthField);
        rootVisualElement.Add(heightField);

    }

    private void HandleCreate()
    {

        foreach(var item in cellVisuals)
        {

            rootVisualElement.Remove(item);

        }

        cellVisuals.Clear();

        int width = int.Parse(widthField.value);
        int height = int.Parse(heightField.value);
        Vector2 size = new Vector2(30, 30);

        mapData = new MapCell[width, height];

        for(int i = 0; i < width; i++)
        {

            for(int j = 0; j < height; j++)
            {

                Vector2Int index = new Vector2Int(i, j);

                var pos = (new Vector2(width, height) / 2) + index * 40;

                var cell = new CellVisual(pos, size, index, 0, ChangeCell, maxSize);
                rootVisualElement.Add(cell);
                cellVisuals.Add(cell);

            }

        }

    }

    private void HandleSave()
    {

        Debug.Log("历厘");

    }

    private MapCell ChangeCell(int cell, Vector2Int index)
    {

        mapData[index.x, index.y] = (MapCell)cell;


        return MapCell.None;

    }

}

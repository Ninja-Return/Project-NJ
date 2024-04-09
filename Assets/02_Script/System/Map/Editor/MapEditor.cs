using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;



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

        ChangeVisual(current);

    }

    private void HandleClick(PointerDownEvent evt)
    {

        //왼
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
        //오
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
    private TextField mapNameField;
    private Array2D<MapCell> mapData;
    private List<CellVisual> cellVisuals = new();
    private static MapDataSO data;

    [MenuItem("Window/MapEditor")]
    public static void OpenWindow()
    {

        data = null;
        CreateWindow<MapEditor>();
        
    }

    private static void OpenWindow(MapDataSO dataSo)
    {

        data = dataSo;
        CreateWindow<MapEditor>();

    }

    private void OnEnable()
    {

        btnSave = new Button(HandleSave);
        btnSave.text = "저장";

        btnCreate = new Button(HandleCreate);
        btnCreate.text = "생성";

        widthField = new TextField("Width");
        heightField = new TextField("Height");
        mapNameField = new TextField("MapName");

        rootVisualElement.Add(btnSave);
        rootVisualElement.Add(btnCreate);
        rootVisualElement.Add(mapNameField);
        rootVisualElement.Add(widthField);
        rootVisualElement.Add(heightField);

        if(data != null)
        {

            Open();

        }

    }

    [OnOpenAsset]
    public static bool OpenSO(int instanceId, int line)
    {

        if(EditorUtility.InstanceIDToObject(instanceId) as MapDataSO)
        {

            OpenWindow(EditorUtility.InstanceIDToObject(instanceId) as MapDataSO);
            return true;

        }

        return false;

    }

    private void HandleCreate()
    {

        data = ScriptableObject.CreateInstance<MapDataSO>();
        AssetDatabase.CreateAsset(data, $"Assets/Resources/MapData/{mapNameField.value}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(data);

        foreach(var item in cellVisuals)
        {

            rootVisualElement.Remove(item);

        }

        cellVisuals.Clear();

        int width = int.Parse(widthField.value);
        int height = int.Parse(heightField.value);
        Vector2 size = new Vector2(30, 30);

        mapData = new Array2D<MapCell>(width, height);

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

    private void Open()
    {

        foreach (var item in cellVisuals)
        {

            rootVisualElement.Remove(item);

        }

        cellVisuals.Clear();

        Vector2 size = new Vector2(30, 30);
    
        mapData = data.data;
        int width = data.data.size.x;
        int height = data.data.size.y;

        for (int i = 0; i < width; i++)
        {

            for (int j = 0; j < height; j++)
            {

                Vector2Int index = new Vector2Int(i, j);

                var pos = (new Vector2(width, height) / 2) + index * 40;

                var cell = new CellVisual(pos, size, index, (int)mapData[i, j], ChangeCell, maxSize);
                rootVisualElement.Add(cell);
                cellVisuals.Add(cell);

            }

        }

    }

    private void HandleSave()
    {

        data.SetUp(mapData);
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();

    }

    private MapCell ChangeCell(int cell, Vector2Int index)
    {

        mapData[index.x, index.y] = (MapCell)cell;


        return mapData[index.x, index.y];

    }

}

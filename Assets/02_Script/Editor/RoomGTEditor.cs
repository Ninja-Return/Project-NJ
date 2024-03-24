using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(EditorMapGT))]
public class RoomGTEditor : Editor
{

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        if (GUILayout.Button("asdf"))
        {

            ((EditorMapGT)target).Generate();

        }

    }

}
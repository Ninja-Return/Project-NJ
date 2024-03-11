using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeType))]
public class ChangeTypeEditor : Editor
{

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        if (GUILayout.Button("asdf"))
        {

            ((ChangeType)target).Change();

        }

    }

}

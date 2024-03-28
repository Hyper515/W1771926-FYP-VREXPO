using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for creating and joining rooms", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;

        if (GUILayout.Button("Join Public Room A"))
        {
            roomManager.OnEnterButtonClicked_RoomA();
        }

        if (GUILayout.Button("Join Public Room B"))
        {
            roomManager.OnEnterButtonClicked_RoomB();
        }

        if (GUILayout.Button("Join Public Room C"))
        {
            roomManager.OnEnterButtonClicked_RoomC();
        }
    }
}

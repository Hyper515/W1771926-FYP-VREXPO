using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManagers))]

public class LoginManagerEditorScript : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for connecting to Photon Servers.", MessageType.Info);

        LoginManagers loginManager = (LoginManagers)target;

        if (GUILayout.Button("Connect Anonymously")) 
        {
            loginManager.ConnectAnonymously();
        }
    }

}

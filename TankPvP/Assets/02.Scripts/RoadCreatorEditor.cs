using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoadCreator))]
public class RoadCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoadCreator myScript = (RoadCreator)target;
        if (GUILayout.Button("생성"))
        {
            myScript.RoadBuilder();
        }
    }
}

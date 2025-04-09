using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements.Experimental;
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        if (DrawDefaultInspector()){
            if(mapGen.autoUpdate)
            {
                mapGen.DrawMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.DrawMap();
        }
    }
}

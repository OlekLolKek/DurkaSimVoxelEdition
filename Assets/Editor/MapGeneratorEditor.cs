using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    #region UnityMethods

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.AutoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }

    #endregion
}

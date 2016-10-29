using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Research))]
public class EditorResearch : Editor {
    public override void OnInspectorGUI() {
        Research rs = target as Research;
        if (GUILayout.Button("Apply")) {
            rs.EditorRefresh();
            SceneView.RepaintAll();
        }
        if(rs.editorShowResult ? GUILayout.Button("Hide Result") : GUILayout.Button("Show Result")) {
            rs.EditorToggleResult();
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();     
    }
}
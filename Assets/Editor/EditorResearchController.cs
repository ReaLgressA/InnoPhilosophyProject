using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ResearchController))]
public class EditorResearchController : Editor {
    public override void OnInspectorGUI() {
        ResearchController rsc = target as ResearchController;
        if (GUILayout.Button("Refresh")) {
            rsc.EditorRefresh();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Generate Years")) {
            rsc.EditorGenerateYears();
            SceneView.RepaintAll();
        }
        DrawDefaultInspector();
    }
}
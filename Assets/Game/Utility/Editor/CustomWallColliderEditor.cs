#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CustomWallCollider))]
public class CustomWallColliderEditor : Editor {
    CustomWallCollider script;
    bool isEditMode = false;

    void OnEnable() {
        script = (CustomWallCollider)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GUILayout.Space(5);

        if (GUILayout.Button("Add Point to Start")) {
            Undo.RecordObject(script, "Add Point Start");
            script.points.Insert(0, script.points[0] + Vector3.left * 2);
        }
        if (GUILayout.Button("Add Point to End")) {
            Undo.RecordObject(script, "Add Point End");
            script.points.Add(script.points[^1] + Vector3.right * 2);
        }
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = isEditMode ? Color.green : Color.yellow;
        if (GUILayout.Button("Edit Mode")) {
            isEditMode = !isEditMode;
            SceneView.RepaintAll();
        }
        GUI.backgroundColor = originalColor;
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Bake & Delete")) {
            script.Bake();
            script.Delete();
        }
        GUI.backgroundColor = Color.white;
    }

    void OnSceneGUI() {
        if (isEditMode)
            Tools.hidden = true;
        else {
            Tools.hidden = false;
            return;
        }

        for (int i = 0; i < script.points.Count; i++) {
            Vector3 worldPos = script.transform.TransformPoint(script.points[i]);
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(worldPos, Quaternion.identity);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(script, "Move Point");
                script.points[i] = script.transform.InverseTransformPoint(newPos);
            }
        }

        if (script.points.Count > 1) {
            Vector3 firstPointWorld = script.transform.TransformPoint(script.points[0]);
            Vector3 secondPointWorld = script.transform.TransformPoint(script.points[1]);

            Vector3 startDir = (secondPointWorld - firstPointWorld).normalized;
            Vector3 startOffset = firstPointWorld - startDir * 1;

            GUIStyle centeredStyle = new GUIStyle(EditorStyles.boldLabel);
            centeredStyle.alignment = TextAnchor.MiddleCenter;

            Handles.color = Color.green;
            if (Handles.Button(startOffset, Quaternion.identity, 0.25f, 0.25f, Handles.CubeHandleCap)) {
                Undo.RecordObject(script, "Insert Start");
                script.points.Insert(0, script.points[0] - script.transform.InverseTransformVector(startDir * 1));
            }
            Handles.Label(startOffset + Vector3.up * 0.3f, "Add Point To Start", centeredStyle);
            Handles.color = Color.white;
        
            for (int i = 0; i < script.points.Count - 1; i++) {
                Vector3 a = script.transform.TransformPoint(script.points[i]);
                Vector3 b = script.transform.TransformPoint(script.points[i + 1]);
                Vector3 midPoint = (a + b) / 2f;

                Handles.color = Color.yellow;
                if (Handles.Button(midPoint, Quaternion.identity, 0.1f, 0.1f, Handles.CubeHandleCap)) {
                    Undo.RecordObject(script, "Insert Middle");
                    Vector3 newPoint = script.transform.InverseTransformPoint(midPoint);
                    script.points.Insert(i + 1, newPoint);
                }
                Handles.Label(midPoint + Vector3.up * 0.3f, "Add Middle Point", centeredStyle);
                Handles.color = Color.white;
            }

            Vector3 lastPointWorld = script.transform.TransformPoint(script.points[^1]);
            Vector3 prevPointWorld = script.transform.TransformPoint(script.points[^2]);

            Vector3 endDir = (lastPointWorld - prevPointWorld).normalized;
            Vector3 endOffset = lastPointWorld + endDir * 2;

            Handles.color = Color.green;
            if (Handles.Button(endOffset, Quaternion.identity, 0.25f, 0.25f, Handles.CubeHandleCap)) {
                Undo.RecordObject(script, "Insert End");
                script.points.Add(script.points[^1] + script.transform.InverseTransformVector(endDir * 2));
            }
            Handles.Label(endOffset + Vector3.up * 0.3f, "Add Point To End", centeredStyle);
            Handles.color = Color.white;
        }
    }

    [MenuItem("Tools/Custom Wall Collider/Bake All Custom Colliders")]
    public static void BakeAll() {
        var colliders = FindObjectsOfType<CustomWallCollider>();
        foreach (var col in colliders) {
            col.Bake();
        }
    }

    [MenuItem("Tools/Custom Wall Collider/Delete All Custom Colliders")]
    public static void DeleteAll() {
        var colliders = FindObjectsOfType<CustomWallCollider>();
        foreach (var col in colliders) {
            col.Delete();
        }
    }

    [MenuItem("GameObject/Create WallCollider", false, 0)]
    public static void CreateCustomWallCollider() {
        GameObject newWall = new GameObject("WallCollider");
        newWall.AddComponent<CustomWallCollider>();
        Selection.activeGameObject = newWall;
        Undo.RegisterCreatedObjectUndo(newWall, "Create WallCollider");
    }

    [MenuItem("Tools/Custom Wall Collider/Revert Baked Collider (Selection)")]
    static void RevertBakedColliderFromSelection()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogError("Select a baked collider game object!");
            return;
        }

        GameObject newWallGO = new GameObject();
        if(selected.name == "WallCollider_Baked")
            newWallGO.name = "WallCollider_Unbaked";
        else
            newWallGO.name = selected.name + "_Unbaked";

        Undo.RegisterCreatedObjectUndo(newWallGO, "Create Unbaked Collider");
        var wallCollider = newWallGO.AddComponent<CustomWallCollider>();
        newWallGO.transform.SetParent(selected.transform.parent);
        newWallGO.transform.position = selected.transform.position;
        
        List<Transform> cornerColliders = new List<Transform>();
        foreach (Transform child in selected.transform)
        {
            if (child.name.StartsWith("CornerCollider_"))
                cornerColliders.Add(child);
        }

        cornerColliders.Sort((a, b) =>
        {
            int indexA = ParseColliderIndex(a.name);
            int indexB = ParseColliderIndex(b.name);
            return indexA.CompareTo(indexB);
        });

        wallCollider.points.Clear();
        foreach (Transform corner in cornerColliders)
        {
            Vector3 localPos = newWallGO.transform.InverseTransformPoint(corner.position);
            wallCollider.points.Add(localPos);
        }

        List<Transform> edgeColliders = new List<Transform>();
        foreach (Transform child in selected.transform)
        {
            if (child.name.StartsWith("EdgeCollider_"))
                edgeColliders.Add(child);
        }

        if (edgeColliders.Count > 0)
        {
            Transform firstEdge = edgeColliders[0];
            wallCollider.height = firstEdge.localScale.y;
            wallCollider.yPosition = firstEdge.localPosition.y;
            wallCollider.physicsMaterial = firstEdge.GetComponent<BoxCollider>().material;
        }
        Debug.Log("Baked collider succesfully unbaked!");
    }

    static int ParseColliderIndex(string name)
    {
        string[] parts = name.Split('_');
        if (parts.Length < 2) return 0;
        if (int.TryParse(parts[1], out int result))
            return result;
        return 0;
    }
}
#endif

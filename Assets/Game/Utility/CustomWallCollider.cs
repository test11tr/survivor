using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PathPoint {
    public Vector3 position;

    public PathPoint(Vector3 pos) {
        position = pos;
    }
}

[ExecuteAlways]
public class CustomWallCollider : MonoBehaviour {

    [Header("Collider Points")]
    public List<Vector3> points = new List<Vector3> {
        new Vector3(2,0,0), new Vector3(4,0,0), new Vector3(6,0,0)
    };

    [Header("Custom Wall Collider Settings")]
    public float height = 2f;
    public float yPosition = 0f;
    public float width = 0.5f;
    public PhysicsMaterial physicsMaterial;
    public bool loop;

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        int count = points.Count;

        for (int i = 0; i < count + (loop ? 0 : -1); i++) {
            Vector3 currentPoint = transform.TransformPoint(points[i % count]);
            Vector3 nextPoint = transform.TransformPoint(points[(i + 1) % count]);

            Vector3 dir = (nextPoint - currentPoint).normalized;
            Vector3 offsetDir = Vector3.Cross(dir, Vector3.forward).normalized;
            offsetDir.y = 0;
            float radius = width / 2f;

            DrawCapsuleGizmo(currentPoint + Vector3.up * (height / 2f - yPosition), height, radius);

            Vector3 boxStart = currentPoint + offsetDir * radius;
            Vector3 boxEnd = nextPoint + offsetDir * radius;

            Vector3 mid = (boxStart + boxEnd) / 2f + Vector3.up * (height / 2f - yPosition);
            Vector3 scale = new Vector3(width, height, Vector3.Distance(boxStart, boxEnd));

            Gizmos.matrix = Matrix4x4.TRS(mid, Quaternion.LookRotation(boxEnd - boxStart), scale);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = Matrix4x4.identity;
        }

        if (!loop) {
            Vector3 lastPoint = transform.TransformPoint(points[points.Count - 1]);
            DrawCapsuleGizmo(lastPoint + Vector3.up * (height / 2f - yPosition), height, width / 2f);
        }
    }

    void DrawCapsuleGizmo(Vector3 center, float boxHeight, float radius) {
        float cylinderHeight = boxHeight;
        float totalHeight = cylinderHeight + radius * 2;

        Vector3 topSphere = center + Vector3.up * (cylinderHeight / 2f);
        Vector3 bottomSphere = center - Vector3.up * (cylinderHeight / 2f);

        Gizmos.DrawWireSphere(topSphere, radius);
        Gizmos.DrawWireSphere(bottomSphere, radius);

        Gizmos.DrawLine(topSphere + Vector3.forward * radius, bottomSphere + Vector3.forward * radius);
        Gizmos.DrawLine(topSphere - Vector3.forward * radius, bottomSphere - Vector3.forward * radius);
        Gizmos.DrawLine(topSphere + Vector3.right * radius, bottomSphere + Vector3.right * radius);
        Gizmos.DrawLine(topSphere - Vector3.right * radius, bottomSphere - Vector3.right * radius);
    }

    public void Bake() {
        GameObject baked = new GameObject($"{name}_Baked");
        baked.transform.position = gameObject.transform.position;
        baked.transform.SetParent(transform.parent);
        int count = points.Count;

        for (int i = 0; i < count + (loop ? 0 : -1); i++) {
            Vector3 currentPoint = transform.TransformPoint(points[i % count]);
            Vector3 nextPoint = transform.TransformPoint(points[(i + 1) % count]);

            Vector3 dir = (nextPoint - currentPoint).normalized;
            Vector3 offsetDir = Vector3.Cross(dir, Vector3.forward).normalized;
            float radius = width / 2f;

            GameObject capsule = new GameObject($"CornerCollider_{i}");
            capsule.transform.SetParent(baked.transform);
            capsule.transform.position = currentPoint + Vector3.up * (height / 2f - yPosition);

            CapsuleCollider cc = capsule.AddComponent<CapsuleCollider>();
            cc.radius = radius;
            cc.height = height + width;
            cc.direction = 1;
            cc.center = Vector3.zero;
            cc.sharedMaterial = physicsMaterial;

            Vector3 boxStart = currentPoint + offsetDir * radius;
            Vector3 boxEnd = nextPoint + offsetDir * radius;

            GameObject box = new GameObject($"EdgeCollider_{i}");
            box.transform.SetParent(baked.transform);
            box.transform.position = (boxStart + boxEnd) / 2f + Vector3.up * (height / 2f - yPosition) + Vector3.up * (width / 2f);
            box.transform.rotation = Quaternion.LookRotation(boxEnd - boxStart);
            box.transform.localScale = new Vector3(width, height, Vector3.Distance(boxStart, boxEnd));

            BoxCollider bc = box.AddComponent<BoxCollider>();
            bc.sharedMaterial = physicsMaterial;
        }

        if (!loop) {
            GameObject lastCapsule = new GameObject($"CornerCollider_{count}");
            lastCapsule.transform.SetParent(baked.transform);
            Vector3 lastPoint = transform.TransformPoint(points[count - 1]);
            lastCapsule.transform.position = lastPoint + Vector3.up * (height / 2f - yPosition);

            CapsuleCollider cc = lastCapsule.AddComponent<CapsuleCollider>();
            cc.radius = width / 2f;
            cc.height = height + width;
            cc.direction = 1;
            cc.center = Vector3.zero;
            cc.sharedMaterial = physicsMaterial;
        }
    }

    public void Delete()
    {
        DestroyImmediate(gameObject);
    }
}
using UnityEngine;
using System.Collections;

public class BoundsTest : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        Bounds bounds = mesh.bounds;

        print(bounds.size.x);
        print(bounds.size.y);
    }
}
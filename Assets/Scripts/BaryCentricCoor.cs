using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class BaryCentricCoor : MonoBehaviour
{
    [SerializeField] public MeshFilter plane;
    Transform ball;
    Vector3[] verts;
    int[] triangles;
    int triangleIndex;

    [SerializeField] private Vector3 ballPos;
    [SerializeField] private int triangleNr;
    [SerializeField] private Vector3 TriangleNormal;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = plane.sharedMesh;
        verts = mesh.vertices;
        triangles = mesh.triangles;
        ball = GetComponent<Transform>();
        /*
        // Output the vertices and triangles to the console for demonstration
        Debug.Log("Vertices:");
        foreach (Vector3 vertex in verts)
        {
            Debug.Log(vertex);
        }

        Debug.Log("Triangles:");
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int vertexIndex1 = triangles[i];
            int vertexIndex2 = triangles[i + 1];
            int vertexIndex3 = triangles[i + 2];

            Debug.Log($"Triangle {i / 3 + 1}: {vertexIndex1}, {vertexIndex2}, {vertexIndex3}");
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        ballPos = ball.position;
        triangleIndex = FindTriangleContainingPoint(ball.position);
        triangleNr = triangleIndex;
        TriangleNormal = new Vector3(0, 0, 0);
        if (triangleIndex != -1)
        {
            // Get the vertices of the triangle.
            Vector3 A = verts[triangles[triangleIndex * 3]];
            Vector3 B = verts[triangles[triangleIndex * 3 + 1]];
            Vector3 C = verts[triangles[triangleIndex * 3 + 2]];

            // Compute barycentric coordinates.
            Vector3 barycentric = BarycentricCoordinates(ball.position, A, B, C);

            // Calculate the point in barycentric coordinates.
            Vector3 pointInBarycentric = (1 - barycentric.x - barycentric.y) * A +
                                         barycentric.x * B + barycentric.y * C;

            // Now, you have the barycentric coordinates and the point in barycentric coordinates.
            // Debug.Log("Barycentric Coordinates: " + barycentric);
            // Debug.Log("Point in Barycentric Coordinates: " + pointInBarycentric);
        }
        else
        {
            Debug.Log("Position is not inside any triangles");
        }
    }

    int FindTriangleContainingPoint(Vector3 position)
    {
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            Vector3 A = verts[triangles[i * 3]];
            Vector3 B = verts[triangles[i * 3 + 1]];
            Vector3 C = verts[triangles[i * 3 + 2]];

            if (PointInTriangle(position, A, B, C))
            {
                Debug.Log("We are inside this triangle: " + i);
                return i;
            }
        }

        return -1; // Point is not inside any triangle.
    }
    bool PointInTriangle(Vector3 point, Vector3 A, Vector3 B, Vector3 C)
    {
        // Compute barycentric coordinates.
        Vector3 barycentric = BarycentricCoordinates(point, A, B, C);

        // Check if the point is inside the triangle.
        return barycentric.x >= 0 && barycentric.y >= 0 && (barycentric.x + barycentric.y) <= 1;
    }

    Vector3 BarycentricCoordinates(Vector3 point, Vector3 A, Vector3 B, Vector3 C)
    {
        // Compute vectors.
        Vector3 v0 = B - A;
        Vector3 v1 = C - A;
        Vector3 v2 = point - A;

        // Compute dot products.
        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        // Compute barycentric coordinates.
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return new Vector3(u, v, 1 - u - v);
    }

}


using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UIElements;

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

    void Start()
    {
        Mesh mesh = plane.sharedMesh;
        verts = mesh.vertices;
        triangles = mesh.triangles;
        ball = GetComponent<Transform>();
    }

    void Update()
    {
        ballPos = ball.position;
        triangleIndex = FindTriangleContainingPoint(ball.position);
        triangleNr = triangleIndex;
        TriangleNormal = new Vector3(0, 0, 0);
        if (triangleIndex != -1)
        {
            Vector3 A = verts[triangles[triangleIndex * 3]];
            Vector3 B = verts[triangles[triangleIndex * 3 + 1]];
            Vector3 C = verts[triangles[triangleIndex * 3 + 2]];

            // get barycentric coordinates
            Vector3 barycentric = BarycentricCoordinates(ball.position, A, B, C);

            // barycentric coordinates.
            Vector3 pointInBarycentric = (1 - barycentric.x - barycentric.y) * A +
                                         barycentric.x * B + barycentric.y * C;
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
        Vector3 barycentric = BarycentricCoordinates(point, A, B, C);
        return barycentric.x >= 0 && barycentric.y >= 0 && (barycentric.x + barycentric.y) <= 1;
    }

    Vector3 BarycentricCoordinates(Vector3 point, Vector3 A, Vector3 B, Vector3 C)
    {

        Vector3 v0 = B - A;
        Vector3 v1 = C - A;
        Vector3 v2 = point - A;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return new Vector3(u, v, 1 - u - v);
    }

    public Vector3 normalGets(Vector3 sphere)
    {

        Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3();
        Vector3 barycentric = new Vector3(-1.0f, -1.0f, -1.0f);
        Vector2 vec2sphere = new Vector2(sphere.x, sphere.z);

        for (int i = 0; i < triangles.Length / 3; i++)
        {
            Vector3 A = verts[triangles[i * 3]];
            Vector3 B = verts[triangles[i * 3 + 1]];
            Vector3 C = verts[triangles[i * 3 + 2]];

            barycentric = BarycentricCoordinates(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z), new Vector2(p3.x, p3.z), vec2sphere);

            if (barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0)
            {
                if (PointInTriangle(ball.position, A, B, C))
                {
                    Vector3 AB = p2 - p1;
                    Vector3 AC = p3 - p1;
                    Vector3 n = Vector3.Cross(AB, AC);
                    Debug.Log("Normal fra bary AB;AC: " + n);
                    return n;
                }
            }
        }
        return Vector3.zero;
    }

}


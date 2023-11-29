using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Barycentric : MonoBehaviour
{
    [SerializeField] public GameObject plane;
    [SerializeField] public MeshFilter MeshFilter; // USE
    [SerializeField] public Transform SphereInfo; //USE
    Vector3[] meshinfo; // USE
    int[] indexinfo; // USE
    [SerializeField] private int currentTriangle;
    private Vector3 AB;
    private Vector3 AC;
    Vector3 sphere_vec;
    Vector3 n;

    [SerializeField] private Vector3 ballPos;
    [SerializeField] private int triangleNr;
    [SerializeField] private Vector3 TriangleNormal;

    private void Start()
    {
        if (MeshFilter != null ) // FOR TESTING
        {
            Mesh mesh = MeshFilter.sharedMesh;
            meshinfo = mesh.vertices;
            indexinfo = mesh.triangles;
        }
        else
        {
            Debug.Log("Barycentric: Start: MeshFilter is null");
            return;
        }
    }
    private void FixedUpdate()
    {
        ballPos = SphereInfo.position;
        sphere_vec = SphereInfo.position;
     
        // get triangle
        currentTriangle = getTriangle(sphere_vec);
        triangleNr = currentTriangle;
        TriangleNormal = n;
        if (currentTriangle != -1)
        {
            Debug.Log("Barycentric: FixedUpdate: if-statement: currentTriangle is: " + currentTriangle);
        }
        else
        {
            Debug.Log("Barycentric: FixedUpdate: else-statment: currentTriangle is -1");
        }
    }

    int getTriangle(Vector3 sphere)
    {
        Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3(), barycentric = new Vector3();
        n = new Vector3();

        Vector2 vec2sphere = new Vector2(sphere.x, sphere.z);

        for (int i = 0; i < indexinfo.Length / 3; i++)
        {
            p1 = meshinfo[indexinfo[i * 3]];
            p2 = meshinfo[indexinfo[i * 3 + 1]];
            p3 = meshinfo[indexinfo[i * 3 + 2]];

            barycentric = getBaryC(p1, p2, p3, vec2sphere);
            if (barycentric.x >= 0 && barycentric.y >= 0 && (barycentric.x + barycentric.y) <= 1)
            {
                currentTriangle = i;
                AB = p2 - p1;
                AC = p3 - p1;
                n = Vector3.Cross(AB, AC);
                return i;
            }
        }        
        return -1;
    }
    Vector3 getBaryC(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 vec2sphere)
    {
        Vector2 v0 = p2 - p1;
        Vector2 v1 = p3 - p1;
        Vector2 v2 = vec2sphere - p1;

        //Debug.Log("Barry: GetBaryC v0: " + v0 + " v1: " + v1 + " v2: " + v2);

        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1);
        float d02 = Vector2.Dot(v0, v2);
        float d11 = Vector2.Dot(v1, v1);
        float d12 = Vector2.Dot(v1, v2);
        float denom = 1/((d00 * d11) - (d01 * d01));

        //Debug.Log("Barry: GetBaryC denom: " + denom);

        float v = (d11 * d02 - d01 * d12) / denom;
        float w = (d00 * d12 - d01 * d02) / denom;
        float u = (1.0f - v - w);

        Vector3 barycentric3D = new Vector3(u, v, w);
        Debug.Log("Barry: GetBaryC returns: " + barycentric3D);

        return barycentric3D;

    }

    ////////////////////////////////////////////////////////////////////////
    Vector3 baryCentricPosition(Vector3[] vertexInformation, Vector3 sphere)
    {
        Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3();
        Vector3 barycentric = new Vector3(-1.0f, -1.0f, -1.0f);
        Vector2 vec2sphere = new Vector2(sphere.x, sphere.z);

        for (int i = 0; i < indexinfo.Length / 3; i++)
        {
            p1 = vertexInformation[indexinfo[i * 3]];
            p2 = vertexInformation[indexinfo[i * 3 + 1]];
            p3 = vertexInformation[indexinfo[i * 3 + 2]];

            barycentric = getBaryC(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z), new Vector2(p3.x, p3.z), vec2sphere);

            if (barycentric.x >= 0 && barycentric.y >= 0 && (barycentric.x + barycentric.y) <= 1)
            {
                currentTriangle = i;
                // Debug.Log("Current Triangle test: " +  currentTriangle);
                AB = p2 - p1;
                AC = p3 - p1;
                Vector3 n = Vector3.Cross(AB, AC);
                // Debug.Log("Normal fra bary AB;AC: " + n);
                break;
            }
        }

        Vector3 height = (barycentric.x * p1 + barycentric.y * p2 + barycentric.z * p3);

        return height;

    }

    public Vector3 normalGets(Vector3 sphere)
    {

        Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3();
        Vector3 barycentric = new Vector3(-1.0f, -1.0f, -1.0f);
        Vector2 vec2sphere = new Vector2(sphere.x, sphere.z);

        for (int i = 0; i < indexinfo.Length / 3; i++)
        {
            p1 = meshinfo[indexinfo[i * 3]];
            p2 = meshinfo[indexinfo[i * 3 + 1]];
            p3 = meshinfo[indexinfo[i * 3 + 2]];

            barycentric = getBaryC(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z), new Vector2(p3.x, p3.z), vec2sphere);

            if (barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0)
            {
                currentTriangle = i;
                Debug.Log("Current Triangle test: " + currentTriangle);
                AB = p2 - p1;
                AC = p3 - p1;
                Vector3 n = Vector3.Cross(AB, AC);
                Debug.Log("Normal fra bary AB;AC: " + n);
                return n;
            }
        }


        return Vector3.zero;

    }
}

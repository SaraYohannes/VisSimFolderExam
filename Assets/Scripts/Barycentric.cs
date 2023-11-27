using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Barycentric : MonoBehaviour
{
    /*
     * This is to find the center of the sphere using barycentric coordinates
     */

    [SerializeField] public MeshMaker MeshInfo;
    [SerializeField] public MeshFilter MeshFilter; // USE
    [SerializeField] public Transform SphereInfo; //USE
    //[SerializeField] public Transform tester_gizmo;
    List<Vector3> _meshinfo;
    Vector3[] meshinfo; // USE
    List<int> _indexinfo;
    int[] indexinfo; // USE
    [SerializeField] private int currentTriangle;
    public Vector3 AB;
    public Vector3 AC;
    Vector3 sphere_vec;

    private void Start()
    {
        // Vector3 array with vertex information about the mesh
        //_meshinfo = MeshInfo.GetComponent<MeshMaker>().regular_triangulation;
        //_indexinfo = MeshInfo.GetComponent<MeshMaker>().T_list;
        // GameObject with all information about the sphere
        
        // SphereInfo = GetComponent<Transform>(); // DONT NEED THIS WHEN SERIALIZED
        
        // Gizmo to track barycooridinates
        // tester_gizmo = tester_gizmo.GetComponent<Transform>();
        //Converter();
        if (MeshFilter != null ) // FOR TESTING
        {
            meshinfo = MeshFilter.sharedMesh.vertices;
            indexinfo = MeshFilter.sharedMesh.triangles;
        }
        else
        {
            Debug.Log("Barycentric: Start: MeshFilter is null");
            return;
        }
    }
    private void Converter()
    {
        indexinfo = new int[_indexinfo.Count];
        meshinfo = new Vector3[_meshinfo.Count];
        int indexC = 0;
        foreach (int i in _indexinfo)
        {
            indexinfo[indexC] = i;
            indexC++;
        }
        int meshinfoC = 0;
        foreach(var i in _meshinfo)
        {
            meshinfo[meshinfoC] = i;
            meshinfoC++;
        }
    }

    private void FixedUpdate()
    {
        // Debug.Log(sphere_vec);
        sphere_vec = SphereInfo.transform.position;
        Vector3 currentLocaton = baryCentricPosition(meshinfo, sphere_vec);
        Debug.Log("The currentLocation is: " + currentLocaton);
        Debug.Log("The sphere position is: " + sphere_vec);
        // tester_gizmo.transform.position = currentLocaton;
    }

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

            if (barycentric.x >= 0 && barycentric.y >= 0 && barycentric.z >= 0)
            {
                currentTriangle = i;
                Debug.Log("Current Triangle test: " +  currentTriangle);
                AB = p2 - p1;
                AC = p3 - p1;
                Vector3 n = Vector3.Cross(AB, AC);
                Debug.Log("Normal fra bary AB;AC: " + n);
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


    Vector3 getBaryC(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 vec2sphere)
    {
        Vector2 v0 = p2 - p1;
        Vector2 v1 = p3 - p1;
        Vector2 v2 = vec2sphere - p1;

        //Debug.Log("Barry: GetBaryC v0: " + v0 + " v1: " + v1 + " v2: " + v2);

        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1);
        float d11 = Vector2.Dot(v1, v1);
        float d20 = Vector2.Dot(v2, v0);
        float d21 = Vector2.Dot(v2, v1);
        float denom = d00 * d11 - d01 * d01;

        //Debug.Log("Barry: GetBaryC denom: " + denom);

        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = (1.0f - v - w);

        Vector3 barycentric3D = new Vector3(u, v, w);
        Debug.Log("Barry: GetBaryC returns: " + barycentric3D);


        return barycentric3D;

    }

}

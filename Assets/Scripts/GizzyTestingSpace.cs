using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizzyTestingSpace : MonoBehaviour
{
    [SerializeField] public QuadTree.Node rootNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        DrawQuadTreeGizmos(rootNode);
    }

    void DrawQuadTreeGizmos(QuadTree.Node node)
    {
        if (node == null) return;

        Gizmos.DrawWireCube(new Vector3((node.xmin + node.xmax) / 2, 0, (node.zmin + node.zmax) / 2),
                            new Vector3(node.xmax - node.xmin, 0, node.zmax - node.zmin));

        for (int i = 0; i < 4; i++)
        {
            DrawQuadTreeGizmos(node.childrenNodes[i]);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] public float mass = 1.0f;
    [SerializeField] private Vector3 surfaceNormal;
    private void Start()
    {
        surfaceNormal = GetComponent<Barycentric>().normalGets(this.transform.position);
    }
    private void FixedUpdate()
    {
                
    }
}

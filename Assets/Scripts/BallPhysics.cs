using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] public float mass = 1.0f;
    [SerializeField] private Vector3 surfaceNormal;
    
    private void FixedUpdate()
    {
        applyForce();
    }    
    Vector3 applyForce()
    {
        Vector3 velocity = Vector3.zero;
        float totalForce = getTotalForce();
        float a = totalForce / mass;

        velocity += velocity;
        return velocity;
    }
    float getTotalForce()
    {
        surfaceNormal = GetComponent<BaryCentricCoor>().normalGets(this.transform.position);
        // all the forces affecting the object
        
        return 0f;
    }
}

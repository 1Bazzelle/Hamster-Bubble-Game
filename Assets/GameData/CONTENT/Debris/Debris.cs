using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private Vector3 initVelocity;
    [SerializeField] private float bounceFactor;

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = initVelocity;
    }
    
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        List<Vector3> contactPoints = new();

        foreach (ContactPoint contact in collision.contacts)
            contactPoints.Add(contact.point);

        Vector3 sum = Vector3.zero;

        foreach (Vector3 point in contactPoints)
            sum += point;

        // Calculate Average Contact Point
        Vector3 averageContactPoint = sum / contactPoints.Count;

        Vector3 contactPointToThis = (transform.position - averageContactPoint).normalized;
        rb.linearVelocity += contactPointToThis * bounceFactor;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + initVelocity);
        Gizmos.DrawSphere(transform.position + initVelocity, 0.1f);
    }
}

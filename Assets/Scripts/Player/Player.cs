using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerMoveData
{
    public float verticalAccel;
    public float horizontalAccel;
    public float maxVerticalVel;
    public float maxHorizontalVel;

    public float drag;

    public float bounceFactor;
}
public class Player : MonoBehaviour
{
    public int playerIndex;

    #region Movement

    [SerializeField] private PlayerMoveData moveData;
    private PlayerMovement movement;
    private bool movementLocked;

    private Rigidbody rb;

    #endregion

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        movement = new();
        movement.Initialize(playerIndex, moveData, rb);
    }

    private void FixedUpdate()
    {
        if(!movementLocked) movement.Update();

        foreach (var joystick in Input.GetJoystickNames())
        {
            if (joystick == "") Debug.LogWarning("Somthins not connected");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        List<Vector3> contactPoints = new();

        foreach(ContactPoint contact in collision.contacts)
            contactPoints.Add(contact.point);

        Vector3 sum = Vector3.zero;

        foreach(Vector3 point in contactPoints)
            sum += point;

        // Calculate Average Contact Point
        Vector3 averageContactPoint = sum / contactPoints.Count;

        Vector3 contactPointToThis = (transform.position - averageContactPoint).normalized;

        rb.linearVelocity += contactPointToThis * moveData.bounceFactor;
    }

    public void LockMovement()
    {
        rb.linearVelocity = Vector3.zero;

        movementLocked = true;
    }
    public void UnlockMovement()
    {
        rb.linearVelocity = Vector3.zero;

        movementLocked = false;
    }
}

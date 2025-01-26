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

    public float appliedGravity;

    public float animationSpeed;
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

    [SerializeField] private Animator animator;

    [Header("Hampter")]
    [SerializeField] private GameObject hampter;
    private Material material;

    [Header("Boioioing")]
    [SerializeField] private GameObject bubble;
    [SerializeField] private float initBoioioingFactor;
    private float boioioingFactor;
    [SerializeField] private float boioioingSpeed;
    [SerializeField] private float boioioingFalloff;
    private float boioioingTimeOffset;
    
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        movement = new();
        movement.Initialize(playerIndex, moveData, rb, animator);

        animator.speed = 0;
    }
    private void Update()
    {
        Boioioing();
    }
    private void FixedUpdate()
    {
        if (!movementLocked) movement.Update();

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

        boioioingFactor = initBoioioingFactor;
        boioioingTimeOffset = Mathf.PI / 2; // Start sine wave at peak (+1)
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

    private void Boioioing()
    {
        if (boioioingFactor <= 0)
        {
            boioioingFactor = 0;
            bubble.transform.localScale = Vector3.one * 2; // Reset to default size
            return;
        }

        // Oscillate the bubble's size around its default scale (2, 2, 2)
        float scaleOffset = Mathf.Sin(Time.time * boioioingSpeed + boioioingTimeOffset) * boioioingFactor;
        bubble.transform.localScale = Vector3.one * (2 + scaleOffset);

        // Gradually decrease the boioioing effect
        boioioingFactor -= Time.deltaTime * boioioingFalloff;
    }

    public void ChangeHampterMaterial(Material hampterMaterial)
    {
        material = hampterMaterial;
        hampter.GetComponent<Renderer>().material = hampterMaterial;
    }
    public Material GetMaterial()
    {
        return material;
    }
}

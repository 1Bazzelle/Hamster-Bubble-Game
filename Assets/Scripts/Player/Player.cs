using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerMoveData
{
    [Header("Regular Movement")]
    public float verticalAccel;
    public float horizontalAccel;
    public float maxVerticalVel;
    public float maxHorizontalVel;

    [Header("Dash")]
    public int maxDashCharges;
    public float dashVelPerSec;
    public float dashDuration;

    [Header("Other Stuff")]

    public float drag;

    public float bounceFactor;

    public float appliedGravity;

    public float animationSpeed;
}
public class Player : MonoBehaviour
{
    public int playerIndex;

    public bool debug;

    #region Movement

    [SerializeField] private PlayerMoveData moveData;
    private PlayerMovement movement;
    private bool movementLocked;

    private Rigidbody rb;

    #endregion

    public bool hasFinished;

    [HideInInspector]
    public int dashCharges;
    private List<GameObject> debugSpheres;

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

    [Header("Particles")]
    [SerializeField] private ParticleSystem dashParticles;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        debugSpheres = new();

        movement = new();
        movement.Initialize(playerIndex, moveData, rb, animator);

        dashCharges = moveData.maxDashCharges;

        UpdateDashDebug();

        animator.speed = 0;

        dashParticles.Stop();

        rb.angularVelocity = Vector3.zero;

        hasFinished = false;
    }
    private void Update()
    {
        Boioioing();
        //Debug.Log(dashCharges);
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

        foreach (ContactPoint contact in collision.contacts)
        {
            contactPoints.Add(contact.point);
        }

        // Let the movement script know
        Vector3 normal = collision.contacts[0].normal;
        movement.OnCollision(normal);

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
        animator.speed = 0;
        movementLocked = true;
    }
    public void UnlockMovement()
    {
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

    public void UpdateDashParticles(bool dashing, Vector3 dashDirec)
    {
        if (!dashing)
        {
            dashParticles.Stop();
            return;
        }

        dashParticles.transform.LookAt(transform.position - dashDirec);

        if(!dashParticles.isPlaying) dashParticles.Play();
    }

    public void AddCharge()
    {
        dashCharges++;
        UpdateDashDebug();
    }
    public void UpdateDashDebug()
    {
        return;
        Debug.Log("Charges: " + dashCharges);
        foreach(GameObject sphere in debugSpheres)
        {
            Destroy(sphere);
        }
        debugSpheres.Clear();
        for (int i = 0; i < dashCharges; i++)
        {
            debugSpheres.Add(DisplayDebugSphere(transform, new Vector3(-0.5f, 0, 0) + new Vector3(0.5f * i, 1.5f, 0), Color.blue , 0.3f));
        }
    }
    public PlayerMoveData GetMoveData()
    {
        return moveData;
    }

    public GameObject DisplayDebugSphere(Transform transform, Vector3 offset, Color color, float scale)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.transform.position = transform.position + offset;
        sphere.transform.SetParent(transform, true);
        Destroy(sphere.GetComponent<SphereCollider>());
        return sphere;
    }
}

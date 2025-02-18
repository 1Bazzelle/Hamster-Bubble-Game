using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement
{
    private Player player;
    private PlayerMoveData moveData;

    // Input System
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction dash;


    // Others stuff
    private int playerIndex;
    private Rigidbody rb;
    private Animator animator;

    private Vector3 dashDirec;
    public bool dashing { get; set; }
    private float dashTimer;
    private bool dashButtonPressed;

    public void Enable(Player pplayer, PlayerMoveData pMoveData, Rigidbody rigidbody, Animator animation)
    {
        if(playerControls == null) playerControls = new();

        move = playerControls.Player.Move;
        move.Enable();
        dash = playerControls.Player.Dash;
        dash.Enable();

        moveData = pMoveData;

        rb = rigidbody;
        animator = animation;

        dashing = false;
        dashTimer = 0;

        player = pplayer;
    }
    public void Disable()
    {
        move.Disable();
        dash.Disable();
    }
    public void Update()
    {
        if (playerIndex == 0) return;

        #region Buttons
        KeyCode buttonA     = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button0");
        KeyCode buttonB     = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button1");
        KeyCode buttonX     = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button2");
        KeyCode buttonY     = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button3");
        KeyCode buttonLB    = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button4");
        KeyCode buttonRB    = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button5");
        KeyCode buttonBack  = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button6");
        KeyCode buttonStart = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button7");
        KeyCode buttonL3    = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button8");
        KeyCode buttonR3    = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button9");
        KeyCode buttonGuide = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Joystick{playerIndex}Button10");
        #endregion

        #region Regular Movement

                Vector2 moveDirection = move.ReadValue<Vector2>();

                if ((moveDirection.x >= 0 && rb.linearVelocity.x < moveData.maxHorizontalVel) || 
                    (moveDirection.x <= 0 && rb.linearVelocity.x > -moveData.maxHorizontalVel))
                {
                    rb.linearVelocity += new Vector3( moveDirection.x * moveData.horizontalAccel, 0, 0 );
                    if (animator.speed == 0) animator.speed = moveData.animationSpeed;
                }

                if ((moveDirection.y >= 0 && rb.linearVelocity.y < moveData.maxVerticalVel) || 
                    (moveDirection.y <= 0 && rb.linearVelocity.y > -moveData.maxVerticalVel))
                {
                    rb.linearVelocity += new Vector3( 0, moveDirection.y * moveData.verticalAccel, 0 );
                    if (animator.speed == 0) animator.speed = moveData.animationSpeed;
                }

                if (moveDirection.x == 0 && moveDirection.y == 0)
                {
                    animator.speed = 0;
                }

        #endregion

        #region Dash

        if (!dashing && player.dashCharges > 0 && moveDirection.x != 0 && moveDirection.y != 0)
        {
            //Debug.Log("CAN DASH");
        }

        if (Input.GetKey(buttonA) && !dashButtonPressed && !dashing && player.dashCharges > 0 && moveDirection.x != 0 && moveDirection.y != 0)
        {
            player.dashCharges--;
            player.UpdateDashDebug();


            dashDirec = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;

            dashTimer = moveData.dashDuration;
            dashing = true;
        }
        if(!Input.GetKey(buttonA) && dashButtonPressed)
        {
            dashButtonPressed = false;
        }

        if (dashing)
        {
            if (dashTimer > 0)
            {
                rb.linearVelocity += moveData.dashVelPerSec * dashDirec;
                dashTimer -= Time.deltaTime;
            }
            else if (dashTimer < 0)
            {
                dashing = false;
            }
        }
        player.UpdateDashParticles(dashing, dashDirec);
        #endregion

        #region Drag
        if (rb.linearVelocity.x > 0)
        {
            rb.linearVelocity = new Vector3(Mathf.Max(0, rb.linearVelocity.x - moveData.drag * Time.deltaTime), rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (rb.linearVelocity.x < 0)
        {
            rb.linearVelocity = new Vector3(Mathf.Min(0, rb.linearVelocity.x + moveData.drag * Time.deltaTime), rb.linearVelocity.y, rb.linearVelocity.z);
        }

        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Max(0, rb.linearVelocity.y - moveData.drag * Time.deltaTime), rb.linearVelocity.z);
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Min(0, rb.linearVelocity.y + moveData.drag * Time.deltaTime), rb.linearVelocity.z);
        }
        #endregion
    }
    public void OnCollision(Vector3 normal)
    {
        normal.Normalize();

        if (dashing)
        {
            Vector3 mirroredVector = dashDirec - 2 * Vector3.Dot(dashDirec, normal) * normal;
            dashDirec = mirroredVector;
        }
    }
}

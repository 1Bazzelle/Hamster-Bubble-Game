using UnityEditor;
using UnityEngine;

public class PlayerMovement
{
    private PlayerMoveData moveData;
    private int playerIndex;
    private Rigidbody rb;
    private Animator animator;
    public void Initialize(int joystickIndex, PlayerMoveData pMoveData, Rigidbody rigidbody, Animator animation)
    {
        moveData = pMoveData;
        playerIndex = joystickIndex;
        rb = rigidbody;
        animator = animation;
    }
    public void Update()
    {
        if (playerIndex == 0) return;

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

        string leftStickHorizontal = $"Horizontal{playerIndex}";
        string leftStickVertical = $"Vertical{playerIndex}";

        float horizontalInput = Input.GetAxis(leftStickHorizontal);
        float verticalInput = Input.GetAxis(leftStickVertical);

        if ((horizontalInput >= 0 && rb.linearVelocity.x < moveData.maxHorizontalVel) || 
            (horizontalInput <= 0 && rb.linearVelocity.x > -moveData.maxHorizontalVel))
        {
            rb.linearVelocity += new Vector3( horizontalInput * moveData.horizontalAccel, 0, 0 );
            if (animator.speed == 0) animator.speed = moveData.animationSpeed;
        }

        if ((verticalInput >= 0 && rb.linearVelocity.y < moveData.maxVerticalVel) || 
            (verticalInput <= 0 && rb.linearVelocity.y > -moveData.maxVerticalVel))
        {
            rb.linearVelocity += new Vector3( 0, verticalInput * moveData.verticalAccel, 0 );
            if (animator.speed == 0) animator.speed = moveData.animationSpeed;
        }

        if (horizontalInput == 0 && verticalInput == 0)
        {
            animator.speed = 0;
        }

        if (moveData.appliedGravity > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - moveData.appliedGravity, rb.linearVelocity.z);
        }

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
}

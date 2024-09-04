// This code is based on and online YouTube Tutorial Series
// Produced by a YouTuber named Dave / GameDevelopment
// Channel: https://www.youtube.com/@davegamedevelopment
// Specific tutorials: https://youtu.be/f473C43s8nE?feature=shared
// https://youtu.be/xCxSjgYTw9c?feature=shared

using UnityEngine;
using System;

// Forces Rigibody if object does not have Rigibody
[RequireComponent(typeof(Rigidbody))]
public class MovePlayer : MonoBehaviour
{
    // Assignables
    [Header("Assignables")]
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private LayerMask groundLayer;

    // Player Settings
    [Header("Speed Settings")]
    [SerializeField]
    [Range(1, 100)]
    private int walkSpeed = 10;
    [SerializeField]
    [Range(1, 100)]
    private int sprintSpeed = 20;
    [SerializeField]
    private int movementSpeedMultiplier = 20;
    private int movementSpeed = 10;

    [Header("Ground control")]
    [SerializeField]
    private float groundDrag = 10.0f;
    [SerializeField]
    private float groundSensitivity = 0.1f;
    [SerializeField]
    private float playerHeight = 2.0f;

    [Header("Jump Settings")]
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    [Tooltip("Multiplier applied to force when airborne")]
    private float jumpMultiplier = 0.5f;
    [SerializeField]
    [Tooltip("Cooldown on how long to ready next jump")]
    private float jumpCooldown = 0.1f;

    [Header("Crouch Settings")]
    [SerializeField]
    private int crouchSpeed = 10;
    [SerializeField]
    private float crouchHeightScale = 0.5f;
    [SerializeField]
    private float crouchDownForce = 5f;
    private float startHeightScale;

    [Header("Slope Handling")]
    [SerializeField]
    private float maxSlopeAngle = 45f;
    [SerializeField]
    private float slopeSensitivity = 0.3f;
    private RaycastHit slopeHit;
    private bool exitingSlope = false;

    // Inputs
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    // Checks
    private bool onGround = false;
    private bool isJumpReady = true;

    //  Other
    private Rigidbody rb;

    private moveStates state;

    private enum moveStates
    {
        walking,
        sprinting,
        crouching,
        midair
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startHeightScale = transform.localScale.y;
    }

    private void Update()
    {
        GetInput();
        ApplyDrag();
        ControlSpeed();
        StateHandler();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && isJumpReady == true && onGround == true)
        {
            isJumpReady = false;
            Jump();
            Invoke(nameof(ReadyJump), jumpCooldown);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeightScale, transform.localScale.z);
            rb.AddForce(Vector3.down * crouchDownForce, ForceMode.Impulse);
        }

        if (Input.GetButtonUp("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, startHeightScale, transform.localScale.z);
        }
    }

    private void Movement()
    {

        moveDirection = verticalInput * orientation.forward + horizontalInput * orientation.right;

        if (SlopeCheck() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * movementSpeed * movementSpeedMultiplier, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (onGround)
        {
            moveDirection = moveDirection.normalized * movementSpeed * movementSpeedMultiplier;
            rb.AddForce(moveDirection, ForceMode.Force);
        }
        else if (!onGround)
        {
            moveDirection = moveDirection.normalized * movementSpeed * jumpMultiplier * movementSpeedMultiplier;
            rb.AddForce(moveDirection, ForceMode.Force);
        }
        rb.useGravity = !SlopeCheck();
    }

    private void ApplyDrag()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + groundSensitivity, groundLayer); // Checks ground

        if (onGround)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0.0f;
        }
    }

    private void ControlSpeed()
    {
        if (SlopeCheck() && !exitingSlope)
        {
            if (rb.velocity.magnitude > movementSpeed)
            {
                rb.velocity = rb.velocity.normalized * movementSpeed;
            }
        }
        else
        {
            Vector3 flatHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatHorizontalVelocity.magnitude > movementSpeed)
            {
                Vector3 limitVelocity = flatHorizontalVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ReadyJump()
    {
        exitingSlope = false;
        isJumpReady = true;
    }

    private void StateHandler()
    {
        if (Input.GetButton("Crouch"))
        {
            movementSpeed = crouchSpeed;
            state = moveStates.crouching;
        }
        else if (Input.GetButton("Sprint") && onGround)
        {
            state = moveStates.sprinting;
            movementSpeed = sprintSpeed;
        }
        else if (onGround)
        {
            state = moveStates.walking;
            movementSpeed = walkSpeed;
        }
        else if (!onGround)
        {
            state = moveStates.midair;
        }
    }

    private bool SlopeCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight / 2) + slopeSensitivity))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void ChangeMovementSpeedMultiplier(int speed_multiplier, int sprint_speed)
    {
        movementSpeedMultiplier = speed_multiplier;
        sprintSpeed = sprint_speed; 
    }
}
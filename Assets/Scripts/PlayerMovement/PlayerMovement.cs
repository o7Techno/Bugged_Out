using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    Vector3 moveDirection;

    public float maxSlopeAngle;
    bool exitingSlope;
    RaycastHit slopeHit;

    Rigidbody rb;
    public float groundDrag;
    public float playerHeight;
    bool grounded;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftControl;
    public KeyCode crouchKey = KeyCode.LeftShift;
    // Start is called before the first frame update
    public MovementState state;
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }
   
    private void StateHandler()
    {
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else if (!grounded)
        {
            state = MovementState.air;
        }
        rb.useGravity = !OnSlope();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            playerHeight /= 2;
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            playerHeight = 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.25f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}

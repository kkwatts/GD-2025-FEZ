using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private KeyCode moveLeft = KeyCode.LeftArrow;
    private KeyCode moveRight = KeyCode.RightArrow;
    private KeyCode jump = KeyCode.Space;

    public float gravity;
    public float accelSpeed;
    public float speedLimit;
    public float friction;
    public float turnTimeLimit;
    public float fullJumpHeight;
    public float halfJumpHeight;
    public float jumpDecay;
    public float jumpBufferTimeLimit;
    public float halfJumpTimeLimit;
    public float coyoteTimeLimit;

    public float xVel;
    public bool canTurn;
    public bool canMove;

    private CharacterController controller;
    private LayerMask groundLayer;
    private GameObject occlusion;

    private float fallSpeed;
    private float turnTimer;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float halfJumpTimer;
    public float jumpForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        controller = GetComponent<CharacterController>();
        groundLayer = LayerMask.GetMask("Ground");
        occlusion = transform.GetChild(0).gameObject;

        fallSpeed = 0f;
        jumpForce = 0f;
        turnTimer = 0f;
        jumpBufferTimer = jumpBufferTimeLimit + 1f;
        coyoteTimer = coyoteTimeLimit + 1f; ;
        halfJumpTimer = halfJumpTimeLimit + 1f;
        canTurn = true;
        canMove = true;
    }

    // Update is called once per frame
    void Update() {
        if (canMove) {
            Fall();
            Movement();
            Jump();
            CheckForCeiling();

            controller.Move(new Vector3(xVel, fallSpeed + jumpForce, 0f) * Time.deltaTime);
        }
    }

    // Apply Gravity
    private void Fall() {
        if (!controller.isGrounded) {
            fallSpeed -= gravity * Time.deltaTime;
            coyoteTimer += Time.deltaTime;
            jumpBufferTimer += Time.deltaTime;
        }
        else {
            fallSpeed = -1f;
            jumpForce = 0f;
            coyoteTimer = 0f;
        }
    }

    // X Movement
    private void Movement() {
        if (Input.GetKey(moveLeft) && !Input.GetKey(moveRight)) {
            xVel -= accelSpeed * Time.deltaTime;
            canTurn = true;
            turnTimer = 0f;
            if (Mathf.Abs(xVel) >= speedLimit || (xVel >= speedLimit / 2f && controller.isGrounded)) {
                xVel = -speedLimit;
            }
        }
        else if (Input.GetKey(moveRight) && !Input.GetKey(moveLeft)) {
            xVel += accelSpeed * Time.deltaTime;
            canTurn = true;
            turnTimer = 0f;
            if (xVel >= speedLimit || (xVel <= speedLimit / -2f && controller.isGrounded)) {
                xVel = speedLimit;
            }
        }
        else if (Input.GetKey(moveLeft) && Input.GetKey(moveRight) && canTurn && controller.isGrounded) {
            canTurn = false;
            if (xVel < 0f) {
                xVel = speedLimit;
                GetComponent<PlayerAnimation>().InitiateTurn();
            }
            else if (xVel > 0f) {
                xVel = -speedLimit;
                GetComponent<PlayerAnimation>().InitiateTurn();
            }
        }
        else {
            if (controller.isGrounded) {
                xVel *= friction * (1 - Time.deltaTime);
            }

            if (!canTurn && turnTimer <= turnTimeLimit) {
                xVel = Mathf.Sign(xVel) * speedLimit;
                GetComponent<PlayerAnimation>().InitiateTurn();
            }
            if (Mathf.Abs(xVel) <= 0.001f) {
                xVel = 0;
            }
            if (!canTurn) {
                turnTimer += Time.deltaTime;
            }
            else {
                turnTimer = 0f;
            }
        }

        if (controller.velocity.x == 0f && Mathf.Abs(xVel) == speedLimit) {
            xVel = 0f;
        }
    }

    // Jump Mechanics
    private void Jump() {
        if ((Input.GetKeyDown(jump) && (controller.isGrounded || coyoteTimer <= coyoteTimeLimit)) || (jumpBufferTimer <= jumpBufferTimeLimit && controller.isGrounded)) {
            jumpForce = fullJumpHeight;
            coyoteTimer = coyoteTimeLimit + 1f;
            halfJumpTimer = 0f;
        }
        if (jumpForce > 0f) {
            jumpForce -= jumpDecay * Time.deltaTime;
            if (jumpForce < 0f) {
                jumpForce = 0f;
            }
        }
        if (Input.GetKeyDown(jump)) {
            jumpBufferTimer = 0f;
        }
        if (Input.GetKey(jump)) {
            halfJumpTimer += Time.deltaTime;
        }
        if (!Input.GetKey(jump) && halfJumpTimer <= halfJumpTimeLimit) {
            jumpForce = halfJumpHeight;
            halfJumpTimer = halfJumpTimeLimit + 1f;
        }
    }

    // Ceiling Collision
    private void CheckForCeiling() {
        float offset = 0.2f;
        float distance = 0.4f;
        Debug.DrawRay(transform.position + new Vector3(offset, 0f, 0f), Vector3.up * distance, Color.red);
        Debug.DrawRay(transform.position - new Vector3(offset, 0f, 0f), Vector3.up * distance, Color.red);

        RaycastHit leftHit, rightHit;
        if (Physics.Raycast(transform.position + new Vector3(offset, 0f, 0f), Vector3.up, out rightHit, distance, groundLayer) ||
            Physics.Raycast(transform.position - new Vector3(offset, 0f, 0f), Vector3.up, out leftHit, distance, groundLayer)) {
            jumpForce = 0f;
        }
    }

    // Depth Position Adjustments
    public void AdjustDepth(float depth) {
        if (canMove) {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, depth);
            controller.enabled = true;
        }
    }

    // Disable Movement
    public void DisableMovement() {
        controller.enabled = false;
        canMove = false;
    }

    // Enable Movement
    public void EnableMovement() {
        controller.enabled = true;
        canMove = true;
    }
}

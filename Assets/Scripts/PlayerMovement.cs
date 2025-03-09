using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private KeyCode moveLeft = KeyCode.LeftArrow;
    private KeyCode moveRight = KeyCode.RightArrow;
    private KeyCode jump = KeyCode.Space;

    private Rigidbody rb;
    private LayerMask groundLayer;
    private BoxCollider col;

    public Vector3 gravity;
    public float accelSpeed;
    public float friction;
    public float speedLimit;
    public float turnTimeLimit;
    public float detectWallDistance;
    public float detectFloorDistance;
    public float detectCeilingDistance;
    public float jumpHeight;
    public float coyoteTimeLimit;

    private float xVel;
    private float moveLeftTimer;
    private float moveRightTimer;
    public float coyoteTimer;
    private float jumpForce;
    private bool isGrounded;
    private bool isJumping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");
        col = GetComponent<BoxCollider>();

        xVel = 0f;
        moveLeftTimer = 0f;
        moveRightTimer = 0f;
        coyoteTimer = coyoteTimeLimit + 1f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update() {
        Physics.gravity = gravity;

        // X Movement
        if (Input.GetKey(moveLeft) && !Input.GetKey(moveRight) && moveRightTimer <= 0f) {
            xVel -= accelSpeed;
            moveLeftTimer = turnTimeLimit;
        }
        else if (Input.GetKey(moveRight) && !Input.GetKey(moveLeft) && moveLeftTimer <= 0f) {
            xVel += accelSpeed;
            moveRightTimer = turnTimeLimit;
        }
        else {
            xVel *= friction;
            moveLeftTimer -= Time.deltaTime;
            moveRightTimer -= Time.deltaTime;

            if (xVel == 0f) {
                moveLeftTimer = 0f;
                moveRightTimer = 0f;
            }
        }

        if (Mathf.Abs(xVel) >= speedLimit) {
            xVel = Mathf.Sign(xVel) * speedLimit;
        }
        else if (Mathf.Abs(xVel) < 0.001f) {
            xVel = 0f;
        }

        // Coyote Time
        if (rb.linearVelocity.y != 0f) {
            coyoteTimer += Time.deltaTime;
        }
        else if (isGrounded) {
            coyoteTimer = 0f;
            jumpForce = 0f;
        }

        // Jumping
        if (Input.GetKeyDown(jump) && (isGrounded || (coyoteTimer <= coyoteTimeLimit && rb.linearVelocity.y < 0f))) {
            jumpForce = jumpHeight;
            coyoteTimer = 0f;
            isGrounded = false;
            isJumping = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        if (jumpForce > 0f) {
            jumpForce -= Time.deltaTime;
            CheckForCeiling();
        }
        else if (jumpForce < 0f) {
            jumpForce = 0f;
        }

        CheckForCeiling();
    }
    
    private void FixedUpdate() {
        // Wall Collision
        RaycastHit hit;

        if (xVel > 0f && Physics.Raycast(transform.position, Vector3.right, out hit, detectWallDistance, groundLayer)) {
            Debug.DrawRay(transform.position, Vector3.right * detectWallDistance, Color.red);
            xVel = 0f;
        }
        else if (xVel < 0f && Physics.Raycast(transform.position, Vector3.left, out hit, detectWallDistance, groundLayer)) {
            Debug.DrawRay(transform.position, Vector3.left * detectWallDistance, Color.red);
            xVel = 0f;
        }

        // Move Rigidbody
        rb.MovePosition(transform.position + new Vector3(xVel, jumpForce, 0f) * Time.fixedDeltaTime);
        CheckIfGrounded();
    }

    private void CheckIfGrounded() {
        RaycastHit left, right;
        float leftOffset = col.size.x * 2f;
        float rightOffset = col.size.x * 3f;

        if ((Physics.Raycast(transform.position - new Vector3(leftOffset, 0f, 0f), Vector3.down, out left, detectFloorDistance, groundLayer) ||
            Physics.Raycast(transform.position + new Vector3(rightOffset, 0f, 0f), Vector3.down, out right, detectFloorDistance, groundLayer)) &&
            rb.linearVelocity.y <= 0f) {

            Debug.DrawRay(transform.position - new Vector3(leftOffset, 0f, 0f), Vector3.down * detectFloorDistance, Color.blue);
            Debug.DrawRay(transform.position + new Vector3(rightOffset, 0f, 0f), Vector3.down * detectFloorDistance, Color.blue);

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            isGrounded = true;
            isJumping = false;
            jumpForce = 0f;
            coyoteTimer = 0f;
        }
        else {
            isGrounded = false;
        }
    }

    private void CheckForCeiling() {
        RaycastHit left, right;
        float leftOffset = col.size.x * 2.4f;
        float rightOffset = col.size.x * 3f;

        Debug.DrawRay(transform.position - new Vector3(leftOffset, 0f, 0f), Vector3.up * detectCeilingDistance, Color.green);
        Debug.DrawRay(transform.position + new Vector3(rightOffset, 0f, 0f), Vector3.up * detectCeilingDistance, Color.green);

        if ((Physics.Raycast(transform.position - new Vector3(leftOffset, 0f, 0f), Vector3.up, out left, detectCeilingDistance, groundLayer) ||
            Physics.Raycast(transform.position + new Vector3(rightOffset, 0f, 0f), Vector3.up, out right, detectCeilingDistance, groundLayer)) &&
            rb.linearVelocity.y <= 0f) {

            jumpForce = 0f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -1f, rb.linearVelocity.z);
        }
    }
}

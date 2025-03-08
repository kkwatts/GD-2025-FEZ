using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private KeyCode moveLeft = KeyCode.LeftArrow;
    private KeyCode moveRight = KeyCode.RightArrow;
    private KeyCode jump = KeyCode.Space;

    private Rigidbody rb;

    public float accelSpeed;
    public float friction;
    public float speedLimit;
    public float turnTimeLimit;
    public float jumpHeight;
    public float coyoteTimeLimit;
    public float gravityFactor;

    private float xVel;
    private float moveLeftTimer;
    private float moveRightTimer;
    private float coyoteTimer;
    private float jumpForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody>();

        xVel = 0f;
        moveLeftTimer = 0f;
        moveRightTimer = 0f;
    }

    // Update is called once per frame
    void Update() {
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
        else if (rb.linearVelocity.y == 0f) {
            coyoteTimer = 0f;
            jumpForce = 0f;
        }

        // Jumping
        if (Input.GetKeyDown(jump) && (rb.linearVelocity.y == 0f || (coyoteTimer <= coyoteTimeLimit && rb.linearVelocity.y < 0f))) {
            jumpForce = jumpHeight;
            coyoteTimer = 0f;
        }

        if (jumpForce > 0f) {
            jumpForce -= Time.deltaTime;
        }
        else if (jumpForce < 0f) {
            jumpForce = 0f;
        }
    }
    
    private void FixedUpdate() {
        rb.MovePosition(transform.position + new Vector3(xVel, 0f, 0f) * Time.fixedDeltaTime);

        // Falling
        if (rb.linearVelocity.y < 0f) {
            rb.AddForce(new Vector3(0f, gravityFactor, 0f));
        }
    }
}

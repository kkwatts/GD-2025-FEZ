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
    public float jumpHeight;
    public float jumpDecay;
    public float coyoteTimeLimit;

    private CharacterController controller;

    private float fallSpeed;
    private float xVel;
    private float turnTimer;
    private float coyoteTimer;
    private float jumpForce;
    private bool canTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        controller = GetComponent<CharacterController>();

        fallSpeed = 0f;
        jumpForce = 0f;
        turnTimer = 0f;
        canTurn = true;
    }

    // Update is called once per frame
    void Update() {
        Fall();
        Movement();
        Jump();

        controller.Move(new Vector3(xVel, fallSpeed + jumpForce, 0f) * Time.deltaTime);
    }

    // Apply gravity
    private void Fall() {
        if (!controller.isGrounded) {
            fallSpeed -= gravity * Time.deltaTime;
            coyoteTimer += Time.deltaTime;
        }
        else {
            fallSpeed = -1f;
            jumpForce = 0f;
            coyoteTimer = 0f;
        }
    }

    // X movement
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
            }
            else if (xVel > 0f) {
                xVel = -speedLimit;
            }
        }
        else {
            xVel *= friction * (1 - Time.deltaTime);

            if (!canTurn && turnTimer <= turnTimeLimit) {
                xVel = Mathf.Sign(xVel) * speedLimit;
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

    // Jump mechanics
    private void Jump() { 
        if (Input.GetKeyDown(jump) && (controller.isGrounded || coyoteTimer <= coyoteTimeLimit)) {
            jumpForce = jumpHeight;
            coyoteTimer = coyoteTimeLimit + 1f;
        }
        if (jumpForce > 0f) {
            jumpForce -= jumpDecay * Time.deltaTime;
            if (jumpForce < 0f) {
                jumpForce = 0f;
            }
        }
    }

    private void CheckIfBlocked() {
        float offset = 0.4f;

        Debug.DrawRay(transform.position + new Vector3(offset, 0f, 0f), Vector3.forward * -3, Color.red);
        Debug.DrawRay(transform.position - new Vector3(offset, 0f, 0f), Vector3.forward * -3, Color.red);
    }
}

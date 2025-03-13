using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private KeyCode lookUp;
    private KeyCode lookDown;
    private KeyCode lookLeft;
    private KeyCode lookRight;
    private GameObject player;

    public float standardSpeed;
    public float lookSpeed;
    public float lookDistance;
    public float depth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lookUp = KeyCode.I;
        lookDown = KeyCode.K;
        lookLeft = KeyCode.J;
        lookRight = KeyCode.L;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        Vector3 targetPosition = player.transform.position;
        Vector3 offset = new Vector3(0f, 0f, 0f);
        float speed = standardSpeed;

        if (Input.GetKey(lookUp) && !Input.GetKey(lookDown) && player.GetComponent<CharacterController>().velocity.x == 0f && player.GetComponent<CharacterController>().velocity.y == 0f && player.GetComponent<CharacterController>().isGrounded) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            offset = new Vector3(offset.x, lookDistance, offset.z);
            speed = lookSpeed;
        }
        if (Input.GetKey(lookDown) && !Input.GetKey(lookUp) && player.GetComponent<CharacterController>().velocity.x == 0f && player.GetComponent<CharacterController>().velocity.y == 0f && player.GetComponent<CharacterController>().isGrounded) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            offset = new Vector3(offset.x, -lookDistance, offset.z);
            speed = lookSpeed;
        }
        if (Input.GetKey(lookLeft) && !Input.GetKey(lookRight) && player.GetComponent<CharacterController>().velocity.x == 0f && player.GetComponent<CharacterController>().velocity.y == 0f && player.GetComponent<CharacterController>().isGrounded) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            offset = new Vector3(-lookDistance, offset.y, offset.z);
            speed = lookSpeed;
        }
        if (Input.GetKey(lookRight) && !Input.GetKey(lookLeft) && player.GetComponent<CharacterController>().velocity.x == 0f && player.GetComponent<CharacterController>().velocity.y == 0f && player.GetComponent<CharacterController>().isGrounded) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            offset = new Vector3(lookDistance, offset.y, offset.z);
            speed = lookSpeed;
        }
        if (!Input.GetKey(lookUp) && !Input.GetKey(lookDown) && !Input.GetKey(lookLeft) && !Input.GetKey(lookRight) && player.GetComponent<PlayerMovement>().canMove) {
            player.GetComponent<PlayerMovement>().EnableMovement();
        }

        Move(targetPosition + offset, depth, speed);
    }

    private void Move(Vector3 targetPos, float depth, float speed) {
        Vector3 pos = new Vector3(targetPos.x, targetPos.y, depth);
        transform.position = Vector3.Lerp(transform.position, pos, speed);
    }
}
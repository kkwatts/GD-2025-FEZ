using UnityEngine;

public class WorldRotation : MonoBehaviour {
    private KeyCode rotateLeft;
    private KeyCode rotateRight;
    private GameObject player;
    private GameObject playerOcclusion;

    public int direction;
    public float rotateSpeed;

    private Quaternion originalRotation;
    private bool inRotation;
    private char rotateDirection;
    private float counter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rotateLeft = KeyCode.A;
        rotateRight = KeyCode.D;
        player = GameObject.FindGameObjectWithTag("Player");
        playerOcclusion = player.transform.GetChild(0).gameObject;

        direction = 1;
        originalRotation = transform.rotation;
        inRotation = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(rotateLeft) && !Input.GetKeyDown(rotateRight) && !inRotation) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            inRotation = true;
            rotateDirection = 'r';
            counter = 90f;

            direction--;
            if (direction < 1) {
                direction = 4;
            }
        }
        else if (Input.GetKeyDown(rotateRight) && !Input.GetKeyDown(rotateLeft) && !inRotation) {
            player.GetComponent<PlayerMovement>().DisableMovement();
            inRotation = true;
            rotateDirection = 'l';
            counter = 90f;

            direction++;
            if (direction > 4) {
                direction = 1;
            }
        }

        if (inRotation) { 
            if (rotateDirection == 'l') {
                transform.Rotate(new Vector3(0f, rotateSpeed, 0f));
                counter -= rotateSpeed;
            }
            else {
                transform.Rotate(new Vector3(0f, -rotateSpeed, 0f));
                counter -= rotateSpeed;
            }

            if (counter <= 0) {
                player.GetComponent<PlayerMovement>().EnableMovement();
                playerOcclusion.GetComponent<OcclusionDetection>().canFix = false;
                inRotation = false;
            }
        }
    }
}

using UnityEngine;

public class WorldRotation : MonoBehaviour {
    private KeyCode rotateLeft;
    private KeyCode rotateRight;

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

        direction = 1;
        originalRotation = transform.rotation;
        inRotation = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(rotateLeft) && !Input.GetKeyDown(rotateRight) && !inRotation) {
            inRotation = true;
            rotateDirection = 'r';
            counter = 90f;

            direction--;
            if (direction < 1) {
                direction = 4;
            }
        }
        else if (Input.GetKeyDown(rotateRight) && !Input.GetKeyDown(rotateLeft) && !inRotation) {
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
                inRotation = false;
            }
        }
    }
}

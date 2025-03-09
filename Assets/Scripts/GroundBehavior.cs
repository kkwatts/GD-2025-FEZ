using UnityEngine;

public class GroundBehavior : MonoBehaviour {
    private Renderer render;
    private LayerMask groundLayer;
    private BoxCollider col;
    private GameObject player;
    private GameObject controller;

    private bool visible;
    private bool active;
    private int direction;
    private float originalXSize;
    private float originalYSize;
    private float originalZSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        render = GetComponent<Renderer>();
        groundLayer = LayerMask.GetMask("Ground");
        col = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GameObject.FindGameObjectWithTag("Ground Controller");

        originalXSize = col.size.x;
        originalYSize = col.size.y;
        originalZSize = col.size.z;
    }

    // Update is called once per frame
    void Update() {
        direction = controller.GetComponent<WorldRotation>().direction;
        col.center = new Vector3(0f, 0f, 0f);

        // Determine Activity
        if (visible) {
            Vector3 playerPos = player.transform.position - new Vector3(0f, player.GetComponent<Renderer>().bounds.size.y / 3f, 0f);
            Vector3 groundPos = transform.position + new Vector3(0f, render.bounds.size.y / 2f, 0f);

            if (playerPos.y > groundPos.y) {
                active = true;
            }
            else {
                active = false;
            }
        }

        if (visible && active) {
            col.enabled = true;
        }
        else {
            col.enabled = false;
        }

        // Adjust Colliders
        if (visible && active && (direction == 1 || direction == 3)) {
            col.size = new Vector3(originalXSize, originalYSize, Vector3.Distance(transform.position, player.transform.position) * 3f);
        }
        else if (visible && active && (direction == 2 || direction == 4)) {
            col.size = new Vector3(Vector3.Distance(transform.position, player.transform.position) * 3f, originalYSize, originalZSize);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Determine Visibility
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, render.bounds.size.y / 2.05f, 0f), Vector3.forward * -1f, out hit, Mathf.Infinity, groundLayer)) {
            visible = false;
            active = false;
        }
        else {
            visible = true;
        }
    }
}

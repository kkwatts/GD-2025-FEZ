using UnityEngine;

public class OcclusionDetection : MonoBehaviour {
    private LayerMask occlusionLayer;
    private GameObject player;

    public bool canFix;

    private bool occluded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        occlusionLayer = LayerMask.GetMask("Occlusion");
        player = transform.parent.gameObject;

        occluded = false;
        canFix = true;
    }

    // Update is called once per frame
    void Update() {
        if (player.GetComponent<PlayerMovement>().canMove) {
            CheckIfBlocked();
            if (!occluded) {
                CheckIfGrounded();
            }
        }
    }

    // Check if Blocked by Ground
    public void CheckIfBlocked() {
        float offset = 0.4f;
        float playerOffset = 0.5f;
        float distance = -(Camera.main.transform.position.z - player.transform.position.z);
        Vector3 rightPosition = new Vector3(transform.position.x + offset, transform.position.y, Camera.main.transform.position.z);
        Vector3 leftPosition = new Vector3(transform.position.x - offset, transform.position.y, Camera.main.transform.position.z);

        Debug.DrawRay(rightPosition, Vector3.forward * distance, Color.green);
        Debug.DrawRay(leftPosition, Vector3.forward * distance, Color.green);

        RaycastHit leftHit, rightHit;
        
        if (Physics.Raycast(rightPosition, Vector3.forward, out rightHit, distance, occlusionLayer)) {
            occluded = true;
            if (canFix) {
                player.GetComponent<PlayerMovement>().AdjustDepth(rightHit.transform.position.z - (rightHit.transform.localScale.z / 2f) - playerOffset);
                occluded = false;
            }
        }
        else if (Physics.Raycast(leftPosition, Vector3.forward, out leftHit, distance, occlusionLayer)) {
            occluded = true;
            if (canFix) {
                player.GetComponent<PlayerMovement>().AdjustDepth(leftHit.transform.position.z - (leftHit.transform.localScale.z / 2f) - playerOffset);
                occluded = false;
            }
        }
        else {
            canFix = true;
            occluded = false;
        }
    }

    // Check if Depth Aligns with Ground
    public void CheckIfGrounded() {
        float offset = 1f;
        float playerOffset = 0.5f;

        Debug.DrawRay(transform.position, Vector3.down * offset, Color.blue);
        Debug.DrawRay(transform.position - new Vector3(0f, offset, 0f), Vector3.forward * 3f, Color.blue);

        RaycastHit downHit, forwardHit, backwardHit;
        if (!Physics.Raycast(transform.position, Vector3.down, out downHit, offset, occlusionLayer) && player.GetComponent<CharacterController>().isGrounded) {
            if (Physics.Raycast(transform.position - new Vector3(0f, offset, 0f), Vector3.forward * -1f, out forwardHit, Mathf.Infinity, occlusionLayer)) {
                player.GetComponent<PlayerMovement>().AdjustDepth(forwardHit.transform.position.z + (forwardHit.transform.localScale.z / 2f) - playerOffset);
            }
            else if (Physics.Raycast(transform.position - new Vector3(0f, offset, 0f), Vector3.forward, out backwardHit, Mathf.Infinity, occlusionLayer)) {
                player.GetComponent<PlayerMovement>().AdjustDepth(backwardHit.transform.position.z - (backwardHit.transform.localScale.z / 2f) + playerOffset);
            }
        }
    }
}

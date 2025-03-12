using UnityEngine;

public class OcclusionDetection : MonoBehaviour {
    private LayerMask occlusionLayer;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        occlusionLayer = LayerMask.GetMask("Occlusion");
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update() {
        CheckIfBlocked();
    }

    // Check if Blocked by Ground
    public void CheckIfBlocked() {
        float offset = 0.4f;
        float playerOffset = 0.5f;

        Debug.DrawRay(transform.position + new Vector3(offset, 0f, 0f), Vector3.forward * -3f, Color.green);
        Debug.DrawRay(transform.position - new Vector3(offset, 0f, 0f), Vector3.forward * -3f, Color.green);

        RaycastHit leftHit, rightHit, adjustHit;

        if (Physics.Raycast(transform.position + new Vector3(offset, 0f, 0f), Vector3.forward * -1f, out rightHit, Mathf.Infinity, occlusionLayer)) {
            if (Physics.Raycast(transform.position + new Vector3(offset, 0f, Camera.main.transform.position.z + 1f), Vector3.forward, out adjustHit, Mathf.Infinity, occlusionLayer)) {
                player.GetComponent<PlayerMovement>().AdjustDepth(adjustHit.transform.position.z - (adjustHit.transform.localScale.z / 2f) - playerOffset);
            }
        }
        else if (Physics.Raycast(transform.position - new Vector3(offset, 0f, 0f), Vector3.forward * -1f, out leftHit, Mathf.Infinity, occlusionLayer)) {
            if (Physics.Raycast(transform.position - new Vector3(offset, 0f, Camera.main.transform.position.z + 1f), Vector3.forward, out adjustHit, Mathf.Infinity, occlusionLayer)) {
                player.GetComponent<PlayerMovement>().AdjustDepth(adjustHit.transform.position.z - (adjustHit.transform.localScale.z / 2f) - playerOffset);
            }
        }
    }
    // if sending raycast from within a collider is a problem, maybe just send raycasts from the camera distance by default and compare the hit objects z-position to player
    // z-position and see if adjustment is necessary
}

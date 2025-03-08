using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    Animator anim;

    public float idleTimerMin;
    public float idleTimerMax;

    private bool isIdle;
    private float idleTimer;
    private float idleActionThreshold;
    private int idleAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();

        isIdle = true;

        idleTimer = 0f;
        idleActionThreshold = Random.Range(idleTimerMin, idleTimerMax);
        idleAction = 0;
    }

    // Update is called once per frame
    void Update() {
        // Idle Animations
        if (isIdle) {
            anim.SetBool("Idle", true);
            anim.SetInteger("Idle Action", idleAction);

            if (idleAction == 0) {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleActionThreshold) {
                    idleAction = Random.Range(1, 6);
                    idleActionThreshold = Random.Range(idleTimerMin, idleTimerMax);
                }
            }
        }
        else if (!isIdle) {
            anim.SetBool("Idle", false);
            idleTimer = 0f;
            idleAction = 0;
        }
    }

    void ResetIdleAnimation() {
        idleTimer = 0f;
        idleAction = 0;
    }
}

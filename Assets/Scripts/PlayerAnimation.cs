using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer render;

    public bool paused;

    public float idleTimerMin;
    public float idleTimerMax;

    private bool isIdle;
    private float idleTimer;
    private float idleActionThreshold;
    private int idleAction;
    private bool isRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        isIdle = true;
        isRunning = false;

        idleTimer = 0f;
        idleActionThreshold = Random.Range(idleTimerMin, idleTimerMax);
        idleAction = 0;

        if (paused) {
            isIdle = false;
            isRunning = false;
        }

        anim.SetBool("Slowing", false);
        anim.SetBool("Turning", false);
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Sprite Direction
        if (GetComponent<PlayerMovement>().xVel < 0f) {
            render.flipX = true;
        }
        else if (GetComponent<PlayerMovement>().xVel > 0f) {
            render.flipX = false;
        }

        // Determine State
        if (GetComponent<PlayerMovement>().xVel == 0f) {
            isIdle = true;
            isRunning = false;
        }
        else {
            isIdle = false;
            isRunning = true;
        }

        // Idle Animations
        if (isIdle)
        {
            anim.SetBool("Idle", true);
            anim.SetInteger("Idle Action", idleAction);

            if (idleAction == 0)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleActionThreshold)
                {
                    idleAction = Random.Range(1, 6);
                    idleActionThreshold = Random.Range(idleTimerMin, idleTimerMax);
                }
            }
        }
        else
        {
            anim.SetBool("Idle", false);
            idleTimer = 0f;
            idleAction = 0;
        }

        // Running Animations
        if (isRunning) {
            anim.SetBool("Running", true);

            if (Mathf.Abs(GetComponent<PlayerMovement>().xVel) < GetComponent<PlayerMovement>().speedLimit) {
                anim.SetBool("Slowing", true);
            }
            else {
                anim.SetBool("Slowing", false);
            }

            if (GetComponent<PlayerMovement>().canTurn && anim.GetBool("Turning")) {
                anim.SetBool("Turning", false);
            }
        }
        else {
            anim.SetBool("Running", false);
            anim.SetBool("Slowing", false);
            anim.SetBool("Turning", false);
        }
    }

    public void ResetIdleAnimation() {
        idleTimer = 0f;
        idleAction = 0;
    }

    public void InitiateTurn() {
        anim.SetBool("Turning", true);
    }

    public void EndTurning() {
        anim.SetBool("Turning", false);
    }
}
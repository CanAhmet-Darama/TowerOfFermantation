using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseWalkSpeed;
    public float baseMouseSensitivity;
    public float baseJumpPower;
    public Rigidbody playerRb;
    public Transform cameraPivot;
    public bool isGrounded;
    public bool jumped;
    public float maxSlopeAngle;
    public Animator animator;
    [HideInInspector]public Vector3 groundSlope;
    bool sprinting;
    //[HideInInspector]public bool impulsedAway;
    Vector2 moveDirection;
    float mouseX;
    float mouseY;

    [Header("Unstable System")]
    public static float currentUnstability;
    public float minCooldown;
    public float maxCooldown;
    public float minMultiplier;
    public float maxMultiplier;
    public UI_Manager canvas;
    
    void Start()
    {
        StartCoroutine(Unstabilize());
        Physics.gravity = new Vector3(0,-25,0);
        isGrounded = true;
        jumped = false;

        animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        InputManage();
        AnimationManage();

        float forwardSpeed = baseWalkSpeed * currentUnstability * Time.deltaTime * (sprinting ? 2 : 1) * ((transform.forward * moveDirection.y + transform.right * moveDirection.x).normalized).z;
        float rightSpeed = baseWalkSpeed * currentUnstability * Time.deltaTime * (sprinting ? 2 : 1) * ((transform.forward * moveDirection.y + transform.right * moveDirection.x).normalized).x;
        Vector3 unprojectedVelocity = new Vector3(rightSpeed,playerRb.velocity.y, forwardSpeed);
        playerRb.velocity = Vector3.ProjectOnPlane(unprojectedVelocity, groundSlope);
    }
    private void LateUpdate()
    {
        if (moveDirection.x == 0 && moveDirection.y == 0 && isGrounded && !jumped) playerRb.velocity = Vector3.zero;
    }
    void InputManage()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y = 1;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y = -1;
        }
        else
        {
            moveDirection.y = 0;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x = -1;
        }
        else
        {
            moveDirection.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
        }

        mouseX += Input.GetAxis("Mouse X") * baseMouseSensitivity * currentUnstability;
        mouseY += Input.GetAxis("Mouse Y") * baseMouseSensitivity * currentUnstability;
        mouseY = Mathf.Clamp(mouseY, -45, 45);
        cameraPivot.localRotation = Quaternion.Euler(-mouseY,0,0);
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            float unstableJumpMultiplier = 1 + ((currentUnstability - minMultiplier) / (maxMultiplier - minMultiplier));
            playerRb.AddForce(0, baseJumpPower* unstableJumpMultiplier, 0);
            jumped = true;
            animator.SetTrigger("Jump");
        }
    }
    void AnimationManage()
    {
        if (moveDirection.y > 0) {
            animator.SetBool("Forward", true);
        }
        else animator.SetBool("Forward", false);

        if (sprinting)
        {
            animator.SetBool("Running", true);
        }
        else animator.SetBool("Running", false);

        if (playerRb.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool("Idle", false);
        }
        else animator.SetBool("Idle", true);

        animator.SetBool("Grounded", isGrounded);
    }
    IEnumerator Unstabilize()
    {
        canvas.UnstableWarning();
        currentUnstability = Random.Range(minMultiplier, maxMultiplier);
        Debug.Log("New Unstability : " + currentUnstability);
        UnstableObstacle.obstacleUnstability = Random.Range(0, 2);
        yield return new WaitForSeconds(Random.Range(minCooldown,maxCooldown));
        StartCoroutine(Unstabilize());
    }
}

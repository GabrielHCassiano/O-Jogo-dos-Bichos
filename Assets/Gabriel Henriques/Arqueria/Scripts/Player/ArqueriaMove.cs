using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ArqueriaMove : MonoBehaviour
{
    private ArqueriaPlayerPhysics playerPhysical;
    private InputManager inputManager;
    [SerializeField] private SpriteRenderer sprite;

    private float playerDirection = 1;

    private bool inWallJump;

    private bool canDash = true;
    private bool doDash = false;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashDuration = 1f;
    private bool lastFramePressedDash;

    [Header("CoyoteTime")]
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private Animator animator;
    private ParticleSystem dustParticle;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysical = GetComponent<ArqueriaPlayerPhysics>();
        inputManager = GetComponentInChildren<InputManager>();
        animator = GetComponent<Animator>();
        dustParticle = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        SystemPlayer();
        FlipLogic();
        DashInput();
        Animations();
    }

    private void FixedUpdate()
    {
        MoveLogic();
        JumpLogic();
        DashLogic();
    }

    public float PlayerDirection
    {
        get { return playerDirection; } 
        set { playerDirection = value; }
    }

    public int ScoreValue
    {
        get { return inputManager.playerData.playerScore; }
        set { inputManager.playerData.playerScore = value; }
    }

    public void FlipLogic()
    {
        if (inputManager != null && inputManager.moveDir.x > 0)
            playerDirection = 1;
        else if (inputManager != null && inputManager.moveDir.x < 0)
            playerDirection = -1;

        sprite.transform.localScale = new Vector3(playerDirection, transform.localScale.y, transform.localScale.z);
    }

    public void SystemPlayer()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        if (inputManager != null && inputManager.canInput == false)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
            return;
    }

    public void MoveLogic()
    {
        if (inputManager != null)
            playerPhysical.PlayerMove(inputManager.moveDir.normalized);
    }

    public void JumpLogic()
    {
        if (playerPhysical.InWall() && inputManager != null && inputManager.xPressed == true && inWallJump == false)
        {
            StartCoroutine(CooldownWallJump());
        }

        if (playerPhysical.InGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (inputManager != null && inputManager.xPressed == true && coyoteTimeCounter > 0)
        {
            playerPhysical.PlayerJump();
        }
       
    }

    public IEnumerator CooldownWallJump()
    {
        inWallJump = true;
        playerPhysical.Gravity = 0;
        playerPhysical.WallJump(playerPhysical.LaterDirection.x * -1);
        yield return new WaitForSeconds(0.4f);
        playerPhysical.Gravity = 2.5f;
        inWallJump = false;
    }

    public void DashInput()
    {
        if (inputManager != null && inputManager.squarePressed == true && inputManager.moveDir != Vector2.zero && canDash && inputManager.squarePressed != lastFramePressedDash)
        {
            doDash = true;
        }

        if (inputManager != null)
            lastFramePressedDash = inputManager.squarePressed;
    }

    public void DashLogic()
    {
        if (playerPhysical.InGround() == true)
        {
            canDash = true;
        }

        if (doDash && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        print("Dash");
        doDash = false;
        canDash = false;
        playerPhysical.CanMove = false;

        playerPhysical.ResetVelocity();

        playerPhysical.Gravity = 0;
        playerPhysical.Rigidbody2D.velocity = (inputManager.moveDir * dashSpeed);

        yield return new WaitForSeconds(dashDuration);

        playerPhysical.Rigidbody2D.velocity = Vector2.zero;
        playerPhysical.Gravity = 2.5f;

        playerPhysical.CanMove = true;
    }

    public void ResetDash()
    {
        StopAllCoroutines();

        playerPhysical.Rigidbody2D.velocity = Vector2.zero;
        playerPhysical.Gravity = 2.5f;

        playerPhysical.CanMove = true;

        inWallJump = false;
    }

    void Animations()
    {
        if (animator == null)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
        {
            if (inputManager != null  && inputManager.playerData.playerScoreIndex >= 3)
                animator.SetBool("lose", true);
            else
                animator.SetBool("win", true);
        }

        if (inputManager != null)
            animator.runtimeAnimatorController = inputManager.playerData.animatorController;
        animator.SetBool("Arqueria", true);
        animator.SetFloat("Horizontal", playerPhysical.Rigidbody2D.velocity.x);
        animator.SetFloat("Vertical", playerPhysical.Rigidbody2D.velocity.y);
        animator.SetBool("InGround", playerPhysical.InGround());

        //animator.SetBool("isWalking", GetComponent<Rigidbody2D>().velocity.x != 0);
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ArqueriaMove : MonoBehaviour
{
    private ArqueriaPlayerPhysics playerPhysical;
    private InputManager inputManager;
    private Transform sprite;

    private float playerDirection = 1;

    private bool canDash = true;
    private bool doDash = false;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashDuration = 1f;
    private bool lastFramePressedDash;

    [Header("CoyoteTime")]
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysical = GetComponent<ArqueriaPlayerPhysics>();
        inputManager = GetComponentInChildren<InputManager>();
        sprite = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        SystemPlayer();
        FlipLogic();
        DashInput();
    }

    private void FixedUpdate()
    {
        MoveLogic();
        JumpLogic();
        DashLogic();
    }

    public void FlipLogic()
    {
        if (inputManager.moveDir.x > 0)
            playerDirection = 1;
        else if (inputManager.moveDir.x < 0)
            playerDirection = -1;

        sprite.localScale = new Vector3(playerDirection, transform.localScale.y, transform.localScale.z);
    }

    public void SystemPlayer()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        //Animations();

        if (inputManager.canInput == false)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
            return;
    }

    public void MoveLogic()
    {
        playerPhysical.PlayerMove(inputManager.moveDir.normalized);
    }

    public void JumpLogic()
    {
        if (playerPhysical.InGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (inputManager.xPressed == true && coyoteTimeCounter > 0)
        {
            playerPhysical.PlayerJump();
        }
    }

    public void DashInput()
    {
        if (inputManager.squarePressed == true && inputManager.moveDir != Vector2.zero && canDash && inputManager.squarePressed != lastFramePressedDash)
        {
            doDash = true;
        }

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

        playerPhysical.Gravity = 0;
        playerPhysical.Velocity += (inputManager.moveDir * dashSpeed);

        yield return new WaitForSeconds(dashDuration);

        playerPhysical.Velocity = Vector2.zero;
        playerPhysical.Gravity = -9.81f;

        playerPhysical.CanMove = true;
    }

    public void ResetDash()
    {
        StopAllCoroutines();

        playerPhysical.Velocity = Vector2.zero;
        playerPhysical.Gravity = -9.81f;

        playerPhysical.CanMove = true;
    }
}

/*    [Header("Options")]
    public float moveSpeed = 2.5f;
    [Space]
    [SerializeField] private Transform checkGround;
    [SerializeField] private Transform checkWall;
    [SerializeField] private float jumpForce;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    [Space]
    public float dashCooldown = 1f;
    public float currentDashCooldown;
    [Space]
    public float dashSpeed = 4f;
    public float dashDuration = 1f;

    [Header("Debug")]
    public bool canMove = true;
    [Space]
    public bool lastFramePressedDash;
    public bool canDash = true;
    public bool doDash = false;
    [Space]
    [SerializeField] Vector2 moveDir = Vector2.zero;
    [SerializeField] int spriteDir = 1;
    [Space]

    Rigidbody2D rb;
    InputManager inputManager;
    Transform sprite;
    ParticleSystem dustParticle;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>().transform;
        dustParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // Enquanto o inputmanager não existir, retornar
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        //Animations();

        if (inputManager.canInput == false)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
            return;

        JumpLogic();
        DashInput();
    }

    private void FixedUpdate()
    {
        if (inputManager == null)
            return;
        if (inputManager.canInput == false)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
            return;
        Move();
        Dash();
    }

    public bool InGround()
    {
        return Physics2D.OverlapCircle(checkGround.position, 0.3f, LayerMask.GetMask("Ground"));
    }

    public bool InWall()
    {
        return Physics2D.OverlapCircle(checkWall.position, 0.2f, LayerMask.GetMask("Wall"));
    }

    void Move()
    {
        if (canMove)
        {
            // pegando a informação de direção do inputManager
            // e movendo com ela
            moveDir = inputManager.moveDir;
            rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);

            // Dando flip no sprite
            if (moveDir.x > 0)
                spriteDir = 1;
            else if (moveDir.x < 0)
                spriteDir = -1;

            sprite.localScale = new Vector2(spriteDir, sprite.localScale.y);
        }
    }

    public void JumpLogic()
    {
        if (InGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (inputManager.squarePressed == true)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f == true && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (InWall() == true && jumpBufferCounter > 0f)
        {
            StartCoroutine(InWallJump());
        }
    }

    void DashInput()
    {
        // pegando a informação do botão sul (X do PlayStation) do inputManager
        // pra iniciar a sequencia do dash
        if (inputManager.xPressed && moveDir != Vector2.zero && canDash && inputManager.xPressed != lastFramePressedDash)
        {
            doDash = true;
        }

        lastFramePressedDash = inputManager.xPressed;
    }

    void Dash()
    {
        if (InGround())
        {
            canDash = true;
        }

        if (doDash && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator InWallJump()
    {
        canMove = false;

        print("wall");
        rb.velocity = new Vector2(sprite.localScale.x * -1 * jumpForce, jumpForce);

        yield return new WaitForSeconds(0.2f);

        canMove = true;
    }

    IEnumerator DashRoutine()
    {
        doDash = false;
        canDash = false;
        canMove = false;

        rb.gravityScale = 0;
        rb.velocity += (moveDir * dashSpeed);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 2;

        canMove = true;
    }

    //------------------------------Visual-----------------------------//

    void Animations()
    {
        if (animator == null)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
        {
            if (inputManager.playerData.playerScoreIndex >= 3)
                animator.SetBool("lose", true);
            else
                animator.SetBool("win", true);
        }

        animator.runtimeAnimatorController = inputManager.playerData.animatorController;
        animator.SetBool("isWalking", rb.velocity.magnitude != 0);
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }
}*/

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
    private bool inDash = false;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashDuration = 1f;
    private bool lastFramePressedDash;

    [Header("CoyoteTime")]
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private Animator animator;
    private ParticleSystem dustParticle;

    [SerializeField] private SpriteRenderer uiDash;
    [SerializeField] private SpriteRenderer[] ghosting;

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
        FlipLogic();
        DashInput();
        Animations();
        UIDash();
    }

    private void FixedUpdate()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        if (inputManager.canInput == false)
            return;
        if (SceneManager.GetActiveScene().name == "FinishScene")
            return;
        MoveLogic();
        JumpLogic();
        DashLogic();
    }

    public float PlayerDirection
    {
        get { return playerDirection; } 
        set { playerDirection = value; }
    }

    public void FlipLogic()
    {
        if (inputManager != null && inputManager.moveDir.x > 0)
            playerDirection = 1;
        else if (inputManager != null && inputManager.moveDir.x < 0)
            playerDirection = -1;

        sprite.transform.localScale = new Vector3(playerDirection, transform.localScale.y, transform.localScale.z);
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
        if (inputManager != null && inputManager.rtPressed == true && inputManager.moveDir != Vector2.zero && canDash && inputManager.squarePressed != lastFramePressedDash)
        {
            doDash = true;
        }

        if (inputManager != null)
            lastFramePressedDash = inputManager.rtPressed;
    }

    public void DashLogic()
    {
        if (playerPhysical.InGround() == true)
        {
            canDash = true;
        }

        if (doDash && canDash)
        {
            StartCoroutine(GhostingAnim());
            StartCoroutine(DashRoutine());
        }

    }

    IEnumerator DashRoutine()
    {
        doDash = false;
        canDash = false;
        inDash = true;
        playerPhysical.CanMove = false;

        playerPhysical.ResetVelocity();

        playerPhysical.Gravity = 0;
        playerPhysical.Rigidbody2D.velocity = (inputManager.moveDir * dashSpeed);
        yield return new WaitForSeconds(dashDuration);

        playerPhysical.Rigidbody2D.velocity = Vector2.zero;
        playerPhysical.Gravity = 2.5f;

        playerPhysical.CanMove = true;
        inDash = false;
    }

    IEnumerator GhostingAnim()
    {
        for (int i = 0; i < ghosting.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ghosting[i].sprite = sprite.sprite;
            ghosting[i].transform.localScale = new Vector3(playerDirection, transform.localScale.y, transform.localScale.z);
            ghosting[i].gameObject.SetActive(true);
            ghosting[i].transform.parent = null;
        }
        ResetGhosting();
    }

    public void ResetGhosting()
    {
        for (int i = 0; i < ghosting.Length; i++)
        {
            ghosting[i].transform.parent = transform;
            ghosting[i].transform.position = transform.position;
            ghosting[i].gameObject.SetActive(false);
        }
    }

    public void ResetDash()
    {
        StopAllCoroutines();

        playerPhysical.Rigidbody2D.velocity = Vector2.zero;
        playerPhysical.Gravity = 2.5f;

        playerPhysical.CanMove = true;

        inWallJump = false;
        //ResetGhosting();
    }
    
    public void UIDash()
    {
        if (canDash == true)
        {
            uiDash.gameObject.SetActive(true);
            uiDash.color = Color.yellow;
        }
        else if (inDash == true)
        {
            uiDash.gameObject.SetActive(false);
        }
        else
        {
            uiDash.gameObject.SetActive(true);
            uiDash.color = Color.gray;
        }
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
        animator.SetBool("WallJump", playerPhysical.InWall() && !playerPhysical.InGround());
        animator.SetBool("InGround", playerPhysical.InGround());

        //animator.SetBool("isWalking", GetComponent<Rigidbody2D>().velocity.x != 0);
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }

}
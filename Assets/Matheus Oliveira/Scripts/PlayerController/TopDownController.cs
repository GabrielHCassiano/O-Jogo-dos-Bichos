using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    [Header("Options")]
    public float moveSpeed = 2.5f;
    [Space]
    public float dashCooldown = 1f;
    public float currentDashCooldown;
    [Space]
    public float dashSpeed = 4f;
    public float dashDuration = 1f;

    [Header("Debug")]
    public bool canInput = true;
    public bool canMove = true;
    [Space]
    public bool lastFramePressedDash;
    public bool canDash = true;
    public bool doDash = false;
    [Space]
    [SerializeField] Vector2 moveDir = Vector2.zero;
    [SerializeField] int spriteDir = 1;

    Rigidbody2D rb;
    InputManager inputManager;
    Transform sprite;
    ParticleSystem dustParticle;
    Animator animator;

    bool hasSprite = false;

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
        if (canInput == false)
            return;

        DashInput();
        Animations();
    }

    private void FixedUpdate()
    {
        if (inputManager == null || canInput == false)
            return;
        Move();
        Dash();
    }

    void Move()
    {
        if (canMove)
        {
            // pegando a informação de direção do inputManager
            // e movendo com ela
            moveDir = inputManager.moveDir;
            rb.velocity = (moveDir * moveSpeed * 100) * Time.deltaTime;

            // Dando flip no sprite
            if (moveDir.x > 0)
                spriteDir = 1;
            else if (moveDir.x < 0)
                spriteDir = -1;

            sprite.localScale = new Vector2(spriteDir, sprite.localScale.y);
        }
    }

    void DashInput()
    {
        // pegando a informação do botão sul (X do PlayStation) do inputManager
        // pra iniciar a sequencia do dash
        if(inputManager.xPressed && moveDir != Vector2.zero && canDash && inputManager.xPressed != lastFramePressedDash)
        {
            doDash = true;
        }

        lastFramePressedDash = inputManager.xPressed;
    }

    void Dash()
    {
        // Cooldown simples, o jogo tira 1 de um valor a cada segundo, e se esse valor
        // ultrapassar -0 o cooldown acaba
        if (!canDash)
        {
            currentDashCooldown -= Time.deltaTime;
            if(currentDashCooldown <= 0)
            {
                currentDashCooldown = dashCooldown;
                canDash = true;
            }
        }

        if (doDash && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        canMove = false;
        rb.velocity = (moveDir * dashSpeed * 100) * Time.deltaTime;

        yield return new WaitForSeconds(dashDuration);

        doDash = false;

        canMove = true;

        canDash = false;
        currentDashCooldown = dashCooldown;

    }

    //------------------------------Visual-----------------------------//

    void Animations()
    {
        if (animator == null)
            return;

        animator.SetBool("isWalking", rb.velocity.magnitude != 0);
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }
}

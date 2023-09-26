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
    bool doDash = false;

    [Header("Debug")]
    public bool canInput = true;
    public bool canMove = true;
    public bool canDash = true;
    [Space]
    [SerializeField] Vector2 moveDir = Vector2.zero;

    Rigidbody2D rb;
    InputManager inputManager;
    Transform sprite;
    int spriteDir = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>().transform;
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
        if(inputManager.xPressed && moveDir != Vector2.zero)
        {
            doDash = true;
        }
    }

    void Dash()
    {
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

    //----------------------------------Pegar os inputs, o GameManager vai usar essa função ao uma cena iniciar----------------------------------//
}

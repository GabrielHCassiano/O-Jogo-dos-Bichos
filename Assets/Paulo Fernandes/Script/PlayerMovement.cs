using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
public class PlayerMovement : MonoBehaviour
{
    
    //[header("MovementVariables")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpDistance;
    [SerializeField] private bool isFacingRight;

    //[header("Groundcheck")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundSpot;
    [SerializeField] private LayerMask groundLayer;

    //[header("DashVariables")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private float waitTime;
    
    <!-- Variáveis ligadas à gravidade e pelo rastro do Dash
    Problema,não tem gravidade nem rastro -->
    private TrailRenderer dashTrail;
    [SerializeField] private float dashGravity;
    private float normalGravity;
    private float waitTime;
    

    // Awake starts the components the first frame
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //dashTrail = GetComponent<TrailRenderer>();
        //normalGravity = rb.gravityScale;
        isFacingRight = true;
        canDash = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //dashTrail = GetComponent<TrailRenderer>();
        //normalGravity = rb.gravityScale;
        isFacingRight = true;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing){
            return;
        }
    }

    public void Move(InputAction.CallbackContext context){
        moveVector = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed){
            rb.velocity = new Vector2(rb.velocity.x, jumpDistance);
        }
    }

    public void Dash(InputAction.CallbackContext context){
        if(context.performed && canDash){
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);

        if(isDashing){
            return;
        }
    }

    IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        //float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower,0);
        //dashTrailemitting = true;
        yield return new WaitForSeconds(dashTime);
        //dashTrailemitting = false;
        //rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}*/

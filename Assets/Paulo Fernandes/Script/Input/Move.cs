using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[header("CampoSerialized")]
    [SerializeField] private Rigidbody2D rb;
    
    //[header("MovimentaçãoBásica")]
    [SerializeField] public Vector2 moveVector;
    [SerializeField] private float speed = 8f;
    [SerializeField] private bool isFacingRight;

    //[header("MovimentaçãoDash")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashPower = 16f;
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private float dashCooldown = 1f;

    //[header("MovimentaçãoAttack")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] public PowerUps powerUps;
    public int attackDemage = 1;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        powerUps = GetComponent<PowerUps>();
        //anim = GetComponent<Animator>();
        isFacingRight = true;
        canDash = true;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing == true)
        {
            return;
        }

        FixedUpdate();
    }

    private void FixedUpdate() 
    {
        if(isDashing == true)
        {
            return;
        }

        rb.velocity = new Vector2(moveVector.x * speed,moveVector.y * speed);

        if(!isFacingRight && moveVector.x > 0f){
            Flip();
        }
        else if(isFacingRight && moveVector.x < 0f){
            Flip();
        }
    }

    private void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context){
        moveVector = context.ReadValue<Vector2>();
    }

    public void Bola(InputAction.CallbackContext context){
        if(context.performed && canDash){
            StartCoroutine(Dash());
        }
    }

    public void Xis(InputAction.CallbackContext context){
        if(context.performed){
            Attack();
        }
    }

    void Attack(){

        //(Opicional) Play an attack animation
        anim.SetTrigger("Attack");

        //Detect enemy range
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //detect damage
        foreach(Collider2D enemy in hitEnemy)
        {
            if(powerUps.is_rock == true){
                if(enemy.GetComponent<PowerUps>().is_scissors == true){
                    enemy.GetComponent<Life>().vida -= attackDemage;
                }
            }

            //Se o Jogador escolher papel
            if(powerUps.is_paper == true){
                if(enemy.GetComponent<PowerUps>().is_rock == true){
                    enemy.GetComponent<Life>().vida -= attackDemage;
                }
            }

            //Se o jogador escolher tesoura
            if(powerUps.is_scissors == true){
                if(enemy.GetComponent<PowerUps>().is_paper == true){
                    enemy.GetComponent<Life>().vida -= attackDemage;
                }
            }
        }
    }

    void OnDrawGizmosSelected(){

        if(attackPoint == null){
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveVector.x * dashPower,moveVector.y * dashPower);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAndAttackControl : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Rigidbody2D rb;

    public bool getBall = false;
    private bool canAttack;
    private bool attack;

    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetBallLogic();
        AttackLogic();
    }

    public void GetBallLogic()
    {
        if (getBall == true)
        {
            ball.transform.parent = transform;
            ball.transform.position = transform.position;
        }
    }

    public void AttackLogic()
    {
        inputManager = GetComponentInChildren<InputManager>();
        if (inputManager != null && inputManager.squarePressed == true && getBall == true)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        getBall = false;
        ball.transform.parent = null;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x * 5, rb.velocity.y * 5);
        yield return new WaitForSeconds(0.5f);
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball") && getBall == false)
        {
            ball = collision.gameObject;
            getBall = true;
        }
    }
}

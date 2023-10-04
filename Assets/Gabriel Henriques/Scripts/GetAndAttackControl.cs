using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetAndAttackControl : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Rigidbody2D rb;

    private bool getBall = false;
    private bool canAttack;
    private bool attack;


    [SerializeField] private GameObject ball;

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
        ball.tag = "AttackBall";
        ball.GetComponentInParent<BallControl>().damageValue = 1;
        getBall = false;
        ball.transform.parent = null;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x * 8, rb.velocity.y * 8);
        yield return new WaitForSeconds(0.5f);
        ball.tag = "Ball";
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ball") && getBall == false)
        {
            ball = collider.gameObject;
            getBall = true;
        }
        if (collider.CompareTag("AttackBall") )
        {
            GetComponent<StatusPlayer>().lifeValue -= collider.GetComponentInParent<BallControl>().damageValue;
            collider.GetComponentInParent<BallControl>().damageValue = 0;
        }
        if (collider.CompareTag("GetBall") && inputManager.circlePressed == true)
        {
            collider.GetComponentInParent<BallControl>().tag = "Ball";
        }
    }
}

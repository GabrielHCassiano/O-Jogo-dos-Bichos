using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetAndAttackControl : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Rigidbody2D rb;

    private bool getBall = false;

    [SerializeField] private GameObject ball;

    private float force = 0;
    private bool isLow;

    private float cont = 0;
    private bool time = false;
    private Vector2 ballDirection;
    private Vector2 AttackDirection;

    [SerializeField] private GameObject arrow;
    private bool inArrow;

    private Knockback knockback;
    private bool knock;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();

        arrow.transform.parent = transform;
        arrow.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DirectionMove();
        GetBallLogic();
        AttackLogic();
        AttackTime();
        ContLogic();
    }

    private void FixedUpdate()
    {
        if (knockback.KBCountLogic < 0)
        {
            GetComponent<TopDownController>().enabled = true;
        }
        else
        {
            knockback.KnockLogic();
            GetComponent<TopDownController>().enabled = false;
        }
    }

    public int ScoreValue
    {
        get { return inputManager.playerData.playerScore; }
        set { inputManager.playerData.playerScore = value; }
    }


    public GameObject ArrowValue
    {
        get { return arrow; }
        set { arrow = value; }
    }

    public bool getBallValue
    {
        get { return getBall; }
        set { getBall = value; }
    }

    public Vector2 AttackDirectionValue
    {
        get { return AttackDirection; }
        set { AttackDirection = value; }
    }

    public bool knockValue
    {
        get { return knock; }
        set { knock = value; }
    }

    public float contValue
    {
        get { return cont; }
        set { cont = value; }
    }

    public float forceValue
    {
        get { return force; }
        set { force = value; }
    }

    public void DirectionMove()
    {
        if (inputManager != null && inputManager.moveDir != Vector2.zero && arrow != null)
        {
            ballDirection = new Vector2(inputManager.moveDir.x, inputManager.moveDir.y);

            arrow.transform.right = ballDirection;
        }
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
            ForceContLogic();
            inArrow = true;
            arrow.SetActive(true);
        }
        if (inArrow == true && inputManager.squarePressed == false)
        {
            inArrow = false;
            arrow.SetActive(false);
            InAttack();
        }
    }

    void InAttack()
    {
        ball.tag = "AttackBall";
        ball.GetComponentInParent<BallControl>().damageValue = 1;
        getBall = false;
        ball.transform.parent = null;
        AttackDirection = ballDirection;
        time = true;
        cont = force/2;
        if (force >= 80)
        {
            ball.GetComponent<BallControl>().damageValue = 3;
            ball.GetComponent<BallControl>().fireBallValue = true;
        }
    }

    public void AttackTime()
    {
        if (ball != null && ball.tag == "AttackBall")
        {
            ball.GetComponent<Rigidbody2D>().velocity = new Vector2(AttackDirection.x * cont, AttackDirection.y * cont);
        }
        if (ball != null && ball.tag == "AttackBall" && ball.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            force = 0;
            ball.tag = "Ball";
            ball = null;
        }
    }

    public void ForceContLogic()
    {
        if (force < 100 && isLow == true)
        {
            force += 200f * Time.deltaTime;
        }
        if(force >= 100)
        {
            isLow = false;
        }
        if (force > 0 && isLow == false)
        {
            force -= 200f * Time.deltaTime;
        }
        if (force <= 0)
        {
            isLow = true;
        }
    }

    public void ContLogic()
    {
        /*if (ball != null && cont <= 0 && time == false)
        {
            //cont = 35;
        }*/
        if (time == true)
        {
            cont -= 50 * Time.deltaTime;
        }
        if (time == true && cont <= 0)
        {
            cont = 0;
            time = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ball") && getBall == false)
        {
            ball = collider.gameObject;
            getBall = true;
        }

        if (collider.CompareTag("AttackBall"))
        {
            knock = true;
            collider.GetComponent<BallControl>().playerValue.GetComponent<GetAndAttackControl>().contValue = 0;
            collider.GetComponent<Knockback>().Knocking(GetComponent<Collider2D>());
            if (inputManager != null)    //Apenas correção de error  
                knockback.Knocking(collider);
            GetComponent<StatusPlayer>().lifeValue -= collider.GetComponentInParent<BallControl>().damageValue;
            collider.GetComponentInParent<BallControl>().damageValue = 0;
            collider.GetComponent<BallControl>().playerValue = null;
        }
        if (collider.CompareTag("GetBall") && inputManager != null && inputManager.circlePressed == true)
        {
            collider.GetComponentInParent<BallControl>().tag = "Ball";
        }
    }
}

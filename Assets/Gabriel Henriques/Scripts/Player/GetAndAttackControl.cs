using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetAndAttackControl : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Rigidbody2D rb;

    private bool getBall = false;
    private bool inGetBall;
    private bool inputGetBall;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject[] attackBall = new GameObject[20];
    private int idBall = 0;

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
        arrow.transform.position = transform.position + new Vector3(0, 0.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        DirectionMove();
        GetBallLogic();
        AttackLogic();
        AttackTime();
        ContLogic();
        /*if (inputManager.trianglePressed == true && ball != null)
        {
            ball.transform.parent = null;
            ball.GetComponent<BallControl>().playerValue = null;
            force = 0;

            ball.tag = "Ball";
            ball.tag = "Ball";
            getBall = false;
            inGetBall = false;
            inputGetBall = false;
            ball = null;
        }*/
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

    public InputManager inputManagereValue
    {
        get { return inputManager; }
        set { inputManager = value; }
    }

    public int ScoreValue
    {
        get { return inputManager.playerData.playerScore; }
        set { inputManager.playerData.playerScore = value; }
    }

    public Sprite SpriteUIValue
    {
        get { return inputManager.playerData.playerSprite; }
        set { inputManager.playerData.playerSprite = value; }
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

    public bool inGetBallValue
    {
        get { return inGetBall; }
        set { inGetBall = value; }
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
        if (ball != null && GetComponent<StatusPlayer>().loseValue == true)
        {
            ball.transform.parent = null;
            force = 0;
            //ball.GetComponent<BallControl>().fireBallValue = false;
            ball = null;
        }

        if (ball != null && getBall == true)
        {
            ball.transform.parent = transform;
            ball.transform.position = transform.position + new Vector3(0, 0.8f, 0);
        }
        if (ball != null && ball.GetComponent<BallControl>().playerValue != null && ball.GetComponent<BallControl>().playerValue.GetComponentInParent<GetAndAttackControl>().gameObject != gameObject)
            ball.GetComponent<BallControl>().playerValue.GetComponentInParent<GetAndAttackControl>().cont = 0;


        if (inputManager != null && inputManager.circlePressed == true && inputGetBall == false && getBall == false)
        {
            inputGetBall = true;
            if (inGetBall == false)
            {
                StartCoroutine(GetBallCooldown());
            }
        }
        if (inputManager != null && inputManager.circlePressed == false)
            inputGetBall = false;
    }

    IEnumerator GetBallCooldown()
    {
        inGetBall = true;
        yield return new WaitForSeconds(1.5f);
        inGetBall = false;
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
            attackBall[idBall] = ball;
            ball = null;
            idBall += 1;
            /*ball.transform.parent = null;
            //ball.GetComponent<BallControl>().playerValue = null;
            //force = 0;

            //ball.tag = "Ball";
            getBall = false;
            inGetBall = false;
            /*inputGetBall = false;
            ball = null;*/

            inArrow = false;
            arrow.SetActive(false);
            InAttack();
        }
    }

    void InAttack()
    {
        attackBall[idBall - 1].tag = "AttackBall";
        attackBall[idBall - 1].GetComponentInParent<BallControl>().damageValue = 1;
        getBall = false;
        attackBall[idBall - 1].transform.parent = null;
        AttackDirection = ballDirection;

        time = true;
        cont = force/2 + 20;
        if (force >= 97)
        {
            attackBall[idBall - 1].GetComponent<BallControl>().damageValue = 3;
            attackBall[idBall - 1].GetComponent<BallControl>().fireBallValue = true;
        }
    }

    public void AttackTime()
    {
        if (idBall - 1 >= 0 && attackBall[idBall - 1] != null && attackBall[idBall - 1].tag == "AttackBall")
        {
            attackBall[idBall - 1].GetComponent<Rigidbody2D>().velocity = new Vector2(AttackDirection.x * cont, AttackDirection.y * cont);
        }
        if (idBall - 1 >= 0 && attackBall[idBall - 1] != null && attackBall[idBall - 1].tag == "AttackBall" && attackBall[idBall - 1].GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            force = 0;
            attackBall[idBall - 1].tag = "Ball";
            attackBall[idBall - 1] = null;
            idBall -= 1;
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
            force = 0;
            time = false;
            if (idBall -1 >= 0 && attackBall[idBall - 1] != null)
            {
                attackBall[idBall - 1].tag = "Ball";
                attackBall[idBall - 1].GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
                attackBall[idBall - 1].GetComponent<BallControl>().fireBallValue = false;
                attackBall[idBall - 1] = null;
                idBall -= 1;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ball") && getBall == false)
        {
            /*if (ball != null)
            {
                ball.transform.parent = null;
                ball.GetComponent<BallControl>().playerValue = null;
                getBall = false;
                inGetBall = false;
            }*/
            ball = collider.gameObject;
            getBall = true;
            collider.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            //collider.GetComponent<CircleCollider2D>().isTrigger = true;
        }

        if (collider.CompareTag("AttackBall"))
        {
            knock = true;
            collider.GetComponent<BallControl>().playerValue.GetComponentInParent<GetAndAttackControl>().contValue = 0;
            collider.GetComponent<Knockback>().Knocking(GetComponent<Collider2D>());
            if (inputManager != null)    //Apenas corre��o de error  
                knockback.Knocking(collider);
            GetComponent<StatusPlayer>().lifeValue -= collider.GetComponentInParent<BallControl>().damageValue;
            collider.GetComponentInParent<BallControl>().damageValue = 0;
            collider.GetComponent<BallControl>().playerValue = null;
        }
        if (collider.CompareTag("GetBall") && inputManager != null && inGetBall == true)
        {
            collider.GetComponentInParent<BallControl>().tag = "Ball";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BallControl : MonoBehaviour
{
    private int damage = 0;
    [SerializeField] private GameObject getball;
    private Knockback knockback;

    private bool knock = false;
    private bool colliderPlayer;
    [SerializeField] private GameObject player;

    private Vector3 spawnPos;
    [SerializeField] private bool inStop;
    [SerializeField] private bool fora;

    [SerializeField] private SpriteRenderer fireBall;
    [SerializeField] private SpriteRenderer Ball;
    [SerializeField] private bool fBall;
    [SerializeField] private bool attackBall;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        knockback = GetComponent<Knockback>();
        anim = GetComponentInChildren<Animator>();
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetBallLogic();
        SpecialBall();
        StopBall();
        //BallSprite();
        AnimLogic();
    }

    public void FixedUpdate()
    {
        if (knockback.KBCountLogic < 0)
        {
            if (knock == true)
            {
                //tag = "Ball";
                knock = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            //tag = "AttackBall";
            knock = true;
            knockback.KnockLogic();
        }

    }

    public bool knockValue
    {
        get { return knock; }
        set { knock = value; }
    }

    public int damageValue
    { 
        get { return damage; } 
        set { damage = value; }
    }

    public bool colliderPlayerValue
    { 
        get { return colliderPlayer; } 
        set {  colliderPlayer = value; } 
    }

    public bool fireBallValue
    {
        get { return fBall; }
        set { fBall = value; }
    }

    public GameObject playerValue
    {
        get { return player; }
        set { player = value; }
    }

    public float playerContValue
    {
        get { return player.GetComponentInParent<GetAndAttackControl>().contValue; }
        set { player.GetComponentInParent<GetAndAttackControl>().contValue = value; }
    }

    public void GetBallLogic()
    {
        if (tag == "AttackBall")
        {
            attackBall = true;
            getball.SetActive(true);
        }
        else
        {
            attackBall = false;
            getball.SetActive(false);
        }
    }

    public void AnimLogic()
    {
        anim.SetBool("FireBall", fBall);
        anim.SetBool("AttackBall", attackBall);
        anim.SetFloat("Horizontal", GetComponent<Rigidbody2D>().velocity.x);
        anim.SetFloat("Vertical", GetComponent<Rigidbody2D>().velocity.y);
    }

    public void SpecialBall()
    {
        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero && player == null)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
            fBall = false;
            damage = 0;
        }
        else if (player != null && player.GetComponentInParent<StatusPlayer>().loseValue == true)
        {
            player = null;
        }
        /*else if (player != null && player.GetComponentInParent<GetAndAttackControl>().getBallValue == false)
            GetComponent<CircleCollider2D>().isTrigger = true;*/
        else if (GetComponent<Rigidbody2D>().velocity == Vector2.zero && player != null && player.GetComponentInParent<GetAndAttackControl>().getBallValue == false)
        {
            player = null;
        }
        else
            GetComponent<CircleCollider2D>().isTrigger = true;



        if (fBall == true)
        {
            Ball.enabled = false;
            fireBall.gameObject.SetActive(true);
        }
        else if (fBall == false)
        {
            Ball.enabled = true;
            fireBall.gameObject.SetActive(false);
        }
    }

    public void BallSprite()
    {
        if (player != null)
        {
            if (player.GetComponentInParent<GetAndAttackControl>().AttackDirectionValue.x > 0)
            {
                fireBall.flipX = false;
            }
            if (player.GetComponentInParent<GetAndAttackControl>().AttackDirectionValue.x < 0)
            {
                fireBall.flipX = true;
            }
            if (player.GetComponentInParent<GetAndAttackControl>().AttackDirectionValue.y > 0)
            {
                fireBall.flipY = false;
            }
            if (player.GetComponentInParent<GetAndAttackControl>().AttackDirectionValue.y < 0)
            {
                fireBall.flipY = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("HitBox") /*&& collision.gameObject.GetComponentInParent<GetAndAttackControl>().getBallValue == true)
        {
            player = collision.gameObject;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall") && tag == "AttackBall")
        {
            //knockback.Knocking(collider);
            //player.GetComponentInParent<GetAndAttackControl>().contValue = 0;
            knockback.KnockingWall(collider, player.GetComponentInParent<GetAndAttackControl>().contValue);
            player.GetComponentInParent<GetAndAttackControl>().contValue = 0;
            //StartCoroutine(WallTime());
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall2"))
        {
            StartCoroutine(WallTime());
        }
        if (collider.gameObject.CompareTag("HitBox") && collider.gameObject.GetComponentInParent<GetAndAttackControl>().getBallValue == true)
        {
            player = collider.gameObject;
        }
        if (collider.gameObject.CompareTag("AttackBall"))
        {
            player.GetComponentInParent<GetAndAttackControl>().contValue = 0;
            collider.GetComponent<BallControl>().playerValue.GetComponentInParent<GetAndAttackControl>().contValue = 0;
            knockback.Knocking(collider);
            collider.GetComponent<Knockback>().Knocking(GetComponent<Collider2D>());
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Area") && inStop == false)
        {
            fora = true;
        }
    }

    private void StopBall()
    {
        if (fora == true && inStop == false)
        {
            StartCoroutine(StopBallCont());
        }
    }

    IEnumerator StopBallCont()
    {
        inStop = true;
        if(player != null)
            player.GetComponentInParent<GetAndAttackControl>().contValue = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        transform.position = spawnPos;
        //yield return new WaitForSeconds(0.1f);
        inStop = false;
        fora = false;
    }


    IEnumerator WallTime()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponentInParent<GetAndAttackControl>().contValue = 0;
        yield return new WaitForSeconds(1f);
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
}

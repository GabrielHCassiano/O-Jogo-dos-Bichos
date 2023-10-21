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

    // Start is called before the first frame update
    void Start()
    {
        knockback = GetComponent<Knockback>();
        GetComponent<SpriteRenderer>().color = Color.cyan;
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetBallLogic();
        SpecialBall();
        StopBall();
    }

    public void FixedUpdate()
    {
        if (knockback.KBCountLogic < 0)
        {
            if (knock == true)
            {
                knock = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            knock = true;
            knockback.KnockLogic();
        }

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

    public GameObject playerValue
    {
        get { return player; }
        set { player = value; }
    }

    public float playerContValue
    {
        get { return player.GetComponent<GetAndAttackControl>().contValue; }
        set { player.GetComponent<GetAndAttackControl>().contValue = value; }
    }

    public void GetBallLogic()
    {
        if(tag == "AttackBall")
        {
            getball.SetActive(true);
        }
        else
            getball.SetActive(false);
    }

    public void SpecialBall()
    {
        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero && player == null)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
            GetComponent<SpriteRenderer>().color = Color.cyan;
            damage = 0;
        }
        else if(GetComponent<Rigidbody2D>().velocity == Vector2.zero && player != null && player.GetComponent<GetAndAttackControl>().getBallValue == false)
        {
            player = null;
        }
        else
            GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<GetAndAttackControl>().getBallValue == true)
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            knockback.KnockingForce(collider, player.GetComponent<GetAndAttackControl>().contValue);
            //player.GetComponent<GetAndAttackControl>().contValue = 0;
            //knockback.Knocking(collider);
            //StartCoroutine(WallTime());
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall2"))
        {
            StartCoroutine(WallTime());
        }
        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<GetAndAttackControl>().getBallValue == true)
        {
            player = collider.gameObject;
        }
        if (collider.gameObject.CompareTag("AttackBall"))
        {
            player.GetComponent<GetAndAttackControl>().contValue = 0;
            collider.GetComponent<BallControl>().playerValue.GetComponent<GetAndAttackControl>().contValue = 0;
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
            player.GetComponent<GetAndAttackControl>().contValue = 0;
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
        player.GetComponent<GetAndAttackControl>().contValue = 0;
        yield return new WaitForSeconds(1f);
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BallControl : MonoBehaviour
{
    private int damage = 0;
    [SerializeField] private GameObject getball;
    private Knockback knockback;

    // Start is called before the first frame update
    void Start()
    {
        knockback = GetComponent<Knockback>();
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        GetBallLogic();
        SpecialBall();
    }

    public void FixedUpdate()
    {
        if (knockback.KBCountLogic < 0)
        {
            //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            print("oi");
            knockback.KnockLogic();
        }
    }

    public int damageValue
    { 
        get { return damage; } 
        set { damage = value; }
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
        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            damage = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StartCoroutine(WallTime());
        }
        if (collider.gameObject.CompareTag("Player"))
        {
            //knockback.Knocking(collider);
        }
    }

    IEnumerator WallTime()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        //GetComponent<CircleCollider2D>().isTrigger = true;
    }


}

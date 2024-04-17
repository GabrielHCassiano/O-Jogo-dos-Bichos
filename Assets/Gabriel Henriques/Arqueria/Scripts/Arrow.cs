using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject sprite;
    private Vector3 pos;

    [SerializeField] private ArqueriaCombat arqueriaCombat;
    [SerializeField] private PlayerID player;

    [SerializeField] private Vector2 diretion;
    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        arqueriaCombat = GetComponentInParent<ArqueriaCombat>();
        player = arqueriaCombat.GetComponent<PlayerID>();
        pos = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ArrowLogic();
    }
    public PlayerID Player
    { 
        get { return player; } 
        set { player = value; }
    }

    public void ArrowLogic()
    {
        if (arqueriaCombat != null)
        {
            player = arqueriaCombat.GetComponent<PlayerID>();
            diretion = arqueriaCombat.LaterDirection;
            sprite.transform.right = diretion;
            transform.parent = null;
            arqueriaCombat = null;

            StartCoroutine(ArrowCooldown());
        }
        //else
          //  arqueriaCombat = GetComponentInParent<ArqueriaCombat>();

        if(rb.velocity != Vector2.zero)
        {
            sprite.transform.right = new Vector3(rb.velocity.x, rb.velocity.y);
        }

    }


    public IEnumerator ArrowCooldown() 
    {
        rb.gravityScale = 0f;
        rb.velocity = (diretion * 40);
        yield return new WaitForSeconds(0.1f);
        rb.gravityScale = 3f;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow") == true && rb.velocity != Vector2.zero && collision.GetComponentInParent<Rigidbody2D>().velocity != Vector2.zero && player != null)
        {
            StopAllCoroutines();
            rb.velocity = Vector2.zero;
            rb.gravityScale = 3f;
            collision.GetComponentInParent<Arrow>().StopAllCoroutines();
            collision.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponentInParent<Rigidbody2D>().gravityScale = 3f;

        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopAllCoroutines();
            player = null;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        
        if (collision.gameObject.CompareTag("HitBox"))
        {
            if(player != null && collision.gameObject.GetComponentInParent<PlayerID>().ID != player.ID)
            {
                StopAllCoroutines();
                player = null;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                rb.isKinematic = true;
                transform.parent = collision.gameObject.GetComponent<SpriteRenderer>().transform;
                collision.gameObject.GetComponentInParent<ArqueriaCombat>().LifeDown();
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitBox"))
        {
            for (int i = 0; i < collision.gameObject.GetComponentInParent<ArqueriaCombat>().Arrows.Length; i++)
            {
                if (player == null && collision.gameObject.GetComponentInParent<ArqueriaCombat>().Arrows[i] == null)
                {
                    rb.isKinematic = false;
                    arqueriaCombat = collision.gameObject.GetComponentInParent<ArqueriaCombat>();
                    player = arqueriaCombat.GetComponentInParent<PlayerID>();
                    transform.parent = arqueriaCombat.transform;
                    transform.position = arqueriaCombat.transform.position + pos;
                    gameObject.SetActive(false);

                    arqueriaCombat.Arrows[i] = gameObject;
                    break;
                }
            }
        }
    }
}

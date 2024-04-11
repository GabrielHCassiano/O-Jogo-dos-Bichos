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
        pos = new Vector3(0, 0.99f, 0);
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
        rb.velocity = (diretion * 20);
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = 1f;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow") == true && rb.velocity != Vector2.zero)
        {
            StopAllCoroutines();
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1f;
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopAllCoroutines();
            player = null;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if(player != null && collision.gameObject.GetComponent<PlayerID>().ID != player.ID)
            {
                StopAllCoroutines();
                player = null;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                rb.isKinematic = true;
                transform.parent = collision.gameObject.GetComponentInChildren<SpriteRenderer>().transform;
                collision.gameObject.GetComponent<ArqueriaCombat>().LifeDown();
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < collision.gameObject.GetComponent<ArqueriaCombat>().Arrows.Length; i++)
            {
                if (player == null && collision.gameObject.GetComponent<ArqueriaCombat>().Arrows[i] == null)
                {
                    rb.isKinematic = false;
                    arqueriaCombat = collision.gameObject.GetComponent<ArqueriaCombat>();
                    player = arqueriaCombat.GetComponent<PlayerID>();
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

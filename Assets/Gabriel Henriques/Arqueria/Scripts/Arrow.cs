using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private ArqueriaCombat arqueriaCombat;
    [SerializeField] private GameObject player;

    [SerializeField] private Vector2 diretion;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ArrowLogic();
    }

    public void ArrowLogic()
    {
        if (arqueriaCombat != null)
        {
            player = arqueriaCombat.gameObject;
            diretion = arqueriaCombat.LaterDirection;
            transform.right = diretion;
            transform.parent = null;
            arqueriaCombat = null;

            StartCoroutine(ArrowCooldown());
        }
        else
            arqueriaCombat = GetComponentInParent<ArqueriaCombat>();

        if(rb.velocity != Vector2.zero)
        {
            transform.right = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }


    public IEnumerator ArrowCooldown() 
    {
        //rb.gravityScale = 0f;
        //transform.Translate(diretion * 20 * Time.deltaTime);
        rb.gravityScale = 0f;
        rb.velocity = diretion * 20;
        yield return new WaitForSeconds(0.1f);
        rb.gravityScale = 1f;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            player = null;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;

            //transform.Translate(Vector2.zero);
            print("kk");
        }
    }
}

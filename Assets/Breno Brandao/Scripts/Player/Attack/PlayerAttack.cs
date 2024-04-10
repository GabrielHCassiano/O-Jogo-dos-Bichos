using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private float attackCooldown;
    private Animator anim;
    public Transform balaoPoint;
    public GameObject balaoPrefab;
    public float attackspeed = 10;

    public void Attack()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(balaoPrefab, balaoPoint.position, balaoPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = balaoPoint.right * attackspeed;

        }

        if (Time.time >= attackCooldown)
        {
            attackCooldown = Time.time + 1f;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        Attack();

    }



    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit Enemy");
            anim.Play("BalaoVerde");
            Destroy(collision.gameObject);
        }
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Enemy");
            anim.Play("BalaoVerde");
            Destroy(collision.gameObject);
        }
    }

}

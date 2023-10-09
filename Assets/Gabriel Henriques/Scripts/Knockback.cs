using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float kBForce;
    [SerializeField] private float kBCount;
    [SerializeField] private float kBTime;
    [SerializeField] private bool isKnockRightPlayer;

    [SerializeField] private Vector2 difference;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        kBForce = 5f;
        kBCount = -0.1f;
        kBTime = 0.3f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float KBCountLogic
    {
        get { return kBCount; }
        set { kBCount = value; }
    }


    public void KnockLogic()
    {
        if (isKnockRightPlayer == true)
        {
            rb.velocity = difference;
        }
        kBCount -= Time.deltaTime;
    }

    public void Knocking(Collider2D collider)
    {
        kBCount = kBTime;

        if (collider != null)
        {
            difference = rb.transform.position - collider.transform.position;
            difference = difference.normalized * kBForce;
            isKnockRightPlayer = true;
        }
        else
        {
            isKnockRightPlayer = false;
        }

    }

}

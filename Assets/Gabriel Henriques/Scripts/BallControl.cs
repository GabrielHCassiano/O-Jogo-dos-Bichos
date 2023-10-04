using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private int damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int damageValue
    { 
        get { return damage; } 
        set { damage = value; }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StartCoroutine(WallTime());
        }
    }

    IEnumerator WallTime()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }


}

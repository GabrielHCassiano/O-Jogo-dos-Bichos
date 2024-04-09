using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArqueriaPlayerPhysics : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 currentVelocity;
    private Rigidbody2D rb;

    [Header("Check")]
    [SerializeField] private Transform checkCeiling;
    [SerializeField] private Transform checkGround;
    [SerializeField] private Transform checkWall;
    [SerializeField] private ContactFilter2D filterGround;
    [SerializeField] private ContactFilter2D filterWall;
    private Vector2 colliderGround;
    private Vector2 colliderWall;
    private RaycastHit2D[] results = new RaycastHit2D[1];

    [Header("Move")]
    private bool canMove = true;
    [SerializeField] private Vector2 laterDirection;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityScale;
    [SerializeField] private float maxGravity;

    [Header("Jump")]
    [SerializeField] private float forceJump;
    private bool inJump = false;

    private bool isGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        laterDirection.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        print(InGround());
        //TestMove();
        CheckCollision();
        //PlayerGravity();
    }

    public void FixedUpdate()
    {

    }



    public Rigidbody2D Rigidbody2D 
    {
        get { return rb; }
        set { rb = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    public bool InJump
    {
        get { return inJump; }
        set { inJump = value; }
    }

    public float Gravity
    {
        get { return rb.gravityScale; }
        set { rb.gravityScale = value; }
    }

    public Vector2 LaterDirection
    {
        get { return laterDirection; }
        set { laterDirection = value; }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(checkCeiling.position, new Vector2(0.5f, 0.01f));
        Gizmos.DrawCube(checkWall.position, new Vector2(0.01f, 2f));
        Gizmos.DrawCube(checkGround.position, new Vector2(0.8f, 0.01f));
    }

    public bool InWall()
    {
        if (Physics2D.BoxCast(checkWall.position, new Vector2(0.01f, 2f), 0, Vector2.right, filterWall, results, 0f) > 0)
            return true;
        else
            return false;
    }
    public bool InCeiling()
    {
        if (Physics2D.BoxCast(checkCeiling.position, new Vector2(0.5f, 0.01f), 0, Vector2.up, filterGround, results, 0f) > 0)
            return true;
        else
            return false;
    }
    public bool InGround()
    {
        if (Physics2D.BoxCast(checkGround.position, new Vector2(0.1f, 0.01f), 0, Vector2.down, filterGround, results, 0f) > 0)
            return true;
        else
            return false;
    }

    public void CheckCollision()
    {
        //print("Ceiling " + InCeiling());
        //print("Ground " + InGround());

        if (InWall() == true)
        {
            print("Wall");
            //ColliderMove();
        }
        else colliderWall.x = transform.position.x;



        if (InGround() == true)
        {
            print("Ground");
            //ColliderJump();
        }
        else
            colliderGround.y = transform.position.y;


        if (InCeiling())
        {
            HitCeiling();
        }


    }

    public void PlayerMove(Vector2 direction)
    {
        if (canMove == true)
        {
            if (direction.x != 0)
                laterDirection = direction;

            if (direction.x != 0)
                velocity.x = Mathf.SmoothDamp(velocity.x, maxSpeed * direction.x, ref currentVelocity.x, maxSpeed / acceleration);
            if (direction.x == 0)
                velocity.x = Mathf.SmoothDamp(velocity.x, maxSpeed * direction.x, ref currentVelocity.x, maxSpeed / deceleration);

            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        }
    }

    public void PlayerJump()
    {
        //StartCoroutine(CooldownJump());
        rb.velocity = new Vector2(rb.velocity.x, forceJump);
        //velocity.y = forceJump;
    }

    public IEnumerator CooldownJump()
    {
        inJump = true;
        yield return new WaitForSeconds(0.4f);
        inJump = false;

    }

    public void HitCeiling()
    {
        GetComponent<ArqueriaMove>().ResetDash();
        velocity.y = Mathf.Min(0, velocity.y);
            //Vector2 colliderGround = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.down * 0.5f;
            //transform.position = new Vector3(transform.position.x, colliderGround.y, 0);
    }

    public void ColliderJump()
    {
        velocity.y = 0;
        currentVelocity.y = 0;
        colliderGround = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.up * 0f;
        transform.position = new Vector3(colliderWall.x, colliderGround.y, 0);
    }

    public void ColliderMove()
    {
        velocity.x = 0;
        currentVelocity.x = 0;

        if (laterDirection.x > 0)
        {
            colliderWall = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.left * 0.5f;
            transform.position = new Vector3(colliderWall.x, colliderGround.y, 0);
        }
        if (laterDirection.x < 0)
        {
            colliderWall = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.right * 0.5f;
            transform.position = new Vector3(colliderWall.x, colliderGround.y, 0);

        }
    }

    public void ResetVelocity()
    {
        velocity = Vector2.zero; 
        currentVelocity = Vector2.zero;
    }
}

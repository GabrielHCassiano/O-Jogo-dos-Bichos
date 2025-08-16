using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArqueriaPlayerPhysics : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private Rigidbody2D rb;

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
    [SerializeField] private float gravityScale;

    [Header("Jump")]
    [SerializeField] private float forceJump;
    private bool inJump = false;

    private bool isGround = false;
    // Start is called before the first frame update
    void Start()
    {
        laterDirection.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }

    public void FixedUpdate()
    {

    }



    public Rigidbody2D Rigidbody2D 
    {
        get { return rb; }
        set { rb = value; }
    }

    public Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
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
        Gizmos.DrawCube(checkWall.position, new Vector2(0.1f, 1.5f));
        Gizmos.DrawCube(checkGround.position, new Vector2(0.8f, 0.01f));
    }

    public bool InWall()
    {
        if (Physics2D.BoxCast(checkWall.position, new Vector2(0.1f, 1.5f), 0, Vector2.right, filterWall, results, 0f) > 0)
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
        if (Physics2D.BoxCast(checkGround.position, new Vector2(0.5f, 0.01f), 0, Vector2.down, filterGround, results, 0f) > 0)
            return true;
        else
            return false;
    }

    public void CheckCollision()
    {
        if (InWall() == true)
        {
        }
        else colliderWall.x = transform.position.x;

        if (InWall() == true && InGround() == false && rb.gravityScale < gravityScale)
        {
            rb.gravityScale += 0.05f;
            if (rb.gravityScale + 0.05f >= gravityScale)
            {
                rb.gravityScale = gravityScale;
            }
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        if (InWall() == true && InGround() == false && rb.gravityScale == gravityScale)
        {
            rb.gravityScale = 0f;
        }



        if (InGround() == true)
        {
        }
        else
            colliderGround.y = transform.position.y;


    }

    public void PlayerMove(Vector2 direction)
    {
        if (canMove == true)
        {
            if (direction.x != 0)
                laterDirection = direction;

            if (direction.x != 0)
                velocity.x = Mathf.SmoothDamp(velocity.x, maxSpeed * direction.x, ref currentVelocity.x, maxSpeed / acceleration);
            if (direction.x == 0 && velocity.x != 0)
            {
                velocity.x -= (deceleration * laterDirection.x);
                if (velocity.x - deceleration < 0 && laterDirection.x > 0)
                {
                    velocity.x = 0;
                }
                if (velocity.x + deceleration > 0 && laterDirection.x < 0)
                {
                    velocity.x = 0;
                }
            }

            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        }
    }

    public void PlayerJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, forceJump);
    }

    public void WallJump(float direction)
    {
        GetComponent<ArqueriaMove>().ResetDash();

        rb.velocity = Vector2.zero;
        velocity.x = 0;
        rb.velocity = new Vector2(direction * 26, 12);
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

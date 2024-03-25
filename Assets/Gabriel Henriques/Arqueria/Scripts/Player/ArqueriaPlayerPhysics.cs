using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArqueriaPlayerPhysics : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 currentVelocity;

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
    [SerializeField] private float maxGravity;

    [Header("Jump")]
    [SerializeField] private float forceJump;
    private bool inJump = false;

    private bool isGround = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TestMove();
        CheckCollision();
        PlayerGravity();
    }

    public void FixedUpdate()
    {
        PlayerVelocity();
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
        get { return gravity; }
        set { gravity = value; }
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
        if (Physics2D.BoxCast(checkGround.position, new Vector2(0.8f, 0.01f), 0, Vector2.down, filterGround, results, 0f) > 0)
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
            ColliderMove();
        }
        else colliderWall.x = transform.position.x;



        if (InGround() == true)
        {
            print("Ground");
            ColliderJump();
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
        }
    }

    public void PlayerGravity()
    {
        if (InGround() == false && InWall() == false)
        {
            velocity.y += Physics2D.gravity.y * 1 * Time.deltaTime;
            //velocity.y = Mathf.SmoothDamp(velocity.y, maxGravity * gravity, ref currentVelocity.y, maxGravity / 20);
        }
    }

    public void TestMove()
    {

        velocity.y += Physics2D.gravity.y * 1 * Time.deltaTime;

        if (InWall() == true)
        {
            print("asdsa");
            velocity.x = 0;
            currentVelocity.x = 0;
            if (laterDirection.x > 0 && inJump == false)
            {
                colliderWall = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.left * 0.5f;

                transform.position = new Vector3(colliderWall.x, colliderGround.y, 0);
            }

        }
        else
        {
            colliderWall.x = transform.position.x;
        }




        if (InGround() == true && velocity.y < 0)
        {
            print("jump");
            velocity.y = 0;
            currentVelocity.y = 0;
            colliderGround = Physics2D.ClosestPoint(transform.position, results[0].collider) + Vector2.up * 0.5f;
            transform.position = new Vector3(colliderWall.x, colliderGround.y, 0);
            isGround = true;
        }
        else
            colliderGround.y = transform.position.y;




        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            inJump = true;
            StartCoroutine(CooldownJump());
            velocity.y = Mathf.Sqrt(forceJump * -2 * (Physics2D.gravity.y * 1));
        }
    }
    public void PlayerVelocity()
    {

        transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0);
    }

    public void PlayerJump()
    {
        StartCoroutine(CooldownJump());
        velocity.y = forceJump;
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
        velocity.y = Mathf.Min(0, Velocity.y);
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
}

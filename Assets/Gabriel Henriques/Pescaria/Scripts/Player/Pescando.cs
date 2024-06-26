using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using static KeyboardSplitter;

public class Pescando : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerID playerID;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject anzol;
    private bool inAttack;

    private bool canAttack = true;

    private Anzol enemyAnzol;

    private bool inStun;
    private bool inRio;

    private bool lose;
    private bool inLose = false;

    private Vector2 laterDirection;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInChildren<InputManager>();
        playerID = GetComponent<PlayerID>();

        laterDirection.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        System();
        PescandoLogic();
        LoseLogic();
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }

    public Vector3 LaterDirection
    {
        get { return laterDirection; }
        set { laterDirection = value; }
    }

    public bool CanAttack
    {
        get { return canAttack; }
        set { canAttack = value; }
    }

    public bool InAttack
    {
        get { return inAttack; }
        set { inAttack = value; }
    }

    public bool Lose
    { 
        get { return lose; } 
        set { lose = value; } 
    }

    public void System()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        if (inputManager != null && inputManager.moveDir != Vector2.zero)
            laterDirection = inputManager.moveDir;
    }


    public void LoseLogic()
    {
        
        if (!inRio)
        {
            StopAllCoroutines();
            spriteRenderer.flipX = false;
            GetComponent<Animator>().SetBool("InWater", false);
        }

        if (lose)
        {
            StartCoroutine(DeathCooldown());
        }
    }

    public IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        if (GetComponentInChildren<InputManager>() != null)
            GetComponentInChildren<InputManager>().transform.parent = null;
        gameObject.SetActive(false);
    }

    public void PescandoLogic()
    {
        if(canAttack == true)
            anzol.GetComponent<Anzol>().TimeAnzol = 0;

        if (inputManager != null && inputManager.squarePressed == true && canAttack == true)
        {
            inAttack = true;
            arrowPos.transform.localScale = new Vector3(GetComponent<TopDownController>().transform.localScale.x, 1, 1);

            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);
            arrowPos.SetActive(true);
        }
        if (inputManager != null && inputManager.squarePressed == false && inAttack == true && GetComponent<TopDownController>().doDash == false)
        {
            GetComponent<TopDownController>().canDash = false;
            GetComponent<TopDownController>().doDash = false;
            GetComponent<TopDownController>().StopAllCoroutines();
            inputManager.xPressed = false;
            canAttack = false;
            arrowPos.SetActive(false);
            anzol.SetActive(true);
            anzol.GetComponent<Anzol>().Force = 40;
            anzol.GetComponent<Anzol>().Diference = 8;
            anzol.GetComponent<Anzol>().Distance = 0.4f;
        }
    }

    public void ResetInAttack()
    {
        inAttack = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rio") && inStun)
        {
            inRio = true;
            inStun = false;
            transform.parent = null;
            GetComponent<TopDownController>().canMove = false;
            GetComponent<Animator>().SetBool("InWater", true);
            enemyAnzol.InGet = null;
            StartCoroutine(LoseCooldown());
            enemyAnzol.StopAllCoroutines();
            enemyAnzol.Reset();
            CanAttack = false;
        }

        if (collision.CompareTag("Exit") && inRio)
        {
            inRio = false;
            inStun = false;
            transform.parent = null;
            GetComponent<TopDownController>().canMove = true;
            enemyAnzol.InGet = null;
            enemyAnzol.StopAllCoroutines();
            enemyAnzol.Reset();
            CanAttack = true;
        }
    }

    public IEnumerator LoseCooldown()
    {
        inLose = true;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = false;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.flipX = false;
        lose = true;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Anzol>() != null && (collision.GetComponentInParent<Anzol>().Player.ID == 1 || collision.GetComponentInParent<Anzol>().Player.ID == 3))
        {
            if (collision.CompareTag("Arrow") && collision.gameObject.GetComponentInParent<Anzol>().gameObject != anzol.gameObject && inRio && (playerID.ID == 1 || playerID.ID == 3) && (collision.gameObject.GetComponentInParent<Anzol>().InGet == null || collision.gameObject.GetComponentInParent<Anzol>().InGet == this.gameObject))
            {
                print("case - 4");

                enemyAnzol = collision.gameObject.GetComponentInParent<Anzol>();

                collision.gameObject.GetComponentInParent<Anzol>().InBack = true;

                enemyAnzol.InGet = this.gameObject;

                inStun = true;
                transform.parent = collision.transform;
                transform.position = collision.transform.position - new Vector3(0, 1, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (inAttack && anzol.GetComponentInChildren<LineRenderer>().GetComponentInChildren<Pescando>() != null)
                {
                    print("case - 5");

                    anzol.GetComponentInChildren<Pescando>().transform.parent = null;
                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
                else if (inAttack)
                {
                    print("case - 6");

                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
            }
            if (collision.CompareTag("Arrow") && collision.gameObject.GetComponentInParent<Anzol>().gameObject != anzol.gameObject && !inRio && (playerID.ID == 2 || playerID.ID == 4) && (collision.gameObject.GetComponentInParent<Anzol>().InGet == null || collision.gameObject.GetComponentInParent<Anzol>().InGet == this.gameObject))
            {
                print("case - 1");
                enemyAnzol = collision.gameObject.GetComponentInParent<Anzol>();

                collision.gameObject.GetComponentInParent<Anzol>().InBack = true;

                enemyAnzol.InGet = this.gameObject;

                inAttack = false;
                arrowPos.SetActive(false);

                inStun = true;
                transform.parent = collision.transform;
                transform.position = collision.transform.position - new Vector3(0, 1, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (inAttack && anzol.GetComponentInChildren<LineRenderer>().GetComponentInChildren<Pescando>() != null)
                {
                    print("case - 2");

                    anzol.GetComponentInChildren<Pescando>().transform.parent = null;
                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
                else if(inAttack)
                {
                    print("case - 3");

                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
            }
        }
        if (collision.GetComponentInParent<Anzol>() != null && (collision.GetComponentInParent<Anzol>().Player.ID == 2 || collision.GetComponentInParent<Anzol>().Player.ID == 4) && (collision.gameObject.GetComponentInParent<Anzol>().InGet == null || collision.gameObject.GetComponentInParent<Anzol>().InGet == this.gameObject))
        {
            if (collision.CompareTag("Arrow") && collision.gameObject.GetComponentInParent<Anzol>().gameObject != anzol.gameObject && inRio && (playerID.ID == 2 || playerID.ID == 4))
            {
                print("case - 4b");

                enemyAnzol = collision.gameObject.GetComponentInParent<Anzol>();

                collision.gameObject.GetComponentInParent<Anzol>().InBack = true;

                enemyAnzol.InGet = this.gameObject;

                inStun = true;
                transform.parent = collision.transform;
                transform.position = collision.transform.position - new Vector3(0, 1, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (inAttack && anzol.GetComponentInChildren<LineRenderer>().GetComponentInChildren<Pescando>() != null)
                {
                    print("case - 5b");

                    anzol.GetComponentInChildren<Pescando>().transform.parent = null;
                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
                else if (inAttack)
                {
                    print("case - 6b");

                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
            }
            if (collision.CompareTag("Arrow") && collision.gameObject.GetComponentInParent<Anzol>().gameObject != anzol.gameObject && !inRio && (playerID.ID == 1 || playerID.ID == 3) && (collision.gameObject.GetComponentInParent<Anzol>().InGet == null || collision.gameObject.GetComponentInParent<Anzol>().InGet == this.gameObject))
            {
                print("case - 1b");

                enemyAnzol = collision.gameObject.GetComponentInParent<Anzol>();

                collision.gameObject.GetComponentInParent<Anzol>().InBack = true;

                enemyAnzol.InGet = this.gameObject;

                inAttack = false;
                arrowPos.SetActive(false);

                inStun = true;
                transform.parent = collision.transform;
                transform.position = collision.transform.position - new Vector3(0, 1, 0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (inAttack && anzol.GetComponentInChildren<LineRenderer>().GetComponentInChildren<Pescando>() != null)
                {
                    print("case - 2b");

                    anzol.GetComponentInChildren<Pescando>().transform.parent = null;
                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
                else if (inAttack)
                {
                    print("case - 3b");

                    anzol.GetComponent<Anzol>().StopAllCoroutines();
                    anzol.GetComponent<Anzol>().Reset();
                }
            }
        }
    }
}

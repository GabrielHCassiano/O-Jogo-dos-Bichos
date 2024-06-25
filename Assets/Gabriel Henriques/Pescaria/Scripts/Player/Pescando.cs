using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pescando : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject anzol;
    private bool inAttack1;
    private bool inAttack2;
    private bool inAttack3;
    private bool inAttack4;

    private bool canAttack = true;

    private Vector2 laterDirection;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInChildren<InputManager>();

        laterDirection.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        System();
        PescandoLogic();
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

    public bool InAttack1
    {
        get { return inAttack1; }
        set { inAttack1 = value; }
    }

    public bool InAttack2
    {
        get { return inAttack2; }
        set { inAttack2 = value; }
    }

    public bool InAttack3
    {
        get { return inAttack3; }
        set { inAttack3 = value; }
    }

    public bool InAttack4
    {
        get { return inAttack4; }
        set { inAttack4 = value; }
    }

    public void System()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        GetComponent<TopDownController>().canDash = false;

        if (inputManager != null && inputManager.moveDir != Vector2.zero)
            laterDirection = inputManager.moveDir;
    }


    public void PescandoLogic()
    {
        if (inputManager != null && inputManager.xPressed == true && !inAttack2 && !inAttack3 && !inAttack4 && canAttack == true)
        {
            inAttack1 = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);
        }
        if (inputManager != null && inputManager.xPressed == false && inAttack1 == true)
        {
            canAttack = false;
            arrowPos.SetActive(false);
            anzol.SetActive(true);
            anzol.GetComponent<Anzol>().Force = 80;
            anzol.GetComponent<Anzol>().Diference = 6;
            anzol.GetComponent<Anzol>().Distance = 0.12f;
        }

        if (inputManager != null && inputManager.circlePressed == true && !inAttack1 && !inAttack3 && !inAttack4 && canAttack == true)
        {
            inAttack2 = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);
        }
        if (inputManager != null && inputManager.circlePressed == false && inAttack2 == true)
        {
            canAttack = false;
            arrowPos.SetActive(false);
            anzol.SetActive(true);
            anzol.GetComponent<Anzol>().Force = 40;
            anzol.GetComponent<Anzol>().Diference = 2;
            anzol.GetComponent<Anzol>().Distance = 0.25f;
        }

        if (inputManager != null && inputManager.squarePressed == true && !inAttack1 && !inAttack2 && !inAttack4 && canAttack == true)
        {
            inAttack3 = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);
        }
        if (inputManager != null && inputManager.squarePressed == false && inAttack3 == true)
        {
            canAttack = false;
            arrowPos.SetActive(false);
            anzol.SetActive(true);
            anzol.GetComponent<Anzol>().Force = 80;
            anzol.GetComponent<Anzol>().Diference = 4f;
            anzol.GetComponent<Anzol>().Distance = 0.3f;
        }

        if (inputManager != null && inputManager.trianglePressed == true && !inAttack1 && !inAttack2 && !inAttack3 && canAttack == true)
        {
            inAttack4 = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);
        }
        if (inputManager != null && inputManager.trianglePressed == false && inAttack4 == true)
        {
            canAttack = false;
            arrowPos.SetActive(false);
            anzol.SetActive(true);
            anzol.GetComponent<Anzol>().Force = 40;
            anzol.GetComponent<Anzol>().Diference = 1;
            anzol.GetComponent<Anzol>().Distance = 0.6f;
        }


    }

    public void ResetInAttack()
    {
        inAttack1 = false;
        inAttack2 = false;
        inAttack3 = false;
        inAttack4 = false;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            transform.parent = collision.transform;
        }

        if (collision.CompareTag("Rio"))
        {
            transform.parent = null;
        }
    }
}

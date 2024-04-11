using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArqueriaCombat : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private int life;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private int idArrow;

    private bool inArrow;

    private Vector2 laterDirection;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInChildren<InputManager>();

        life = 3;
        laterDirection.x = 1;

    }

    // Update is called once per frame
    void Update()
    {
        System();
        ArrowAim();   
    }

    public Vector3 LaterDirection
    {
        get { return laterDirection; }
        set { laterDirection = value; }
    }

    public GameObject[] Arrows
    { 
      get { return arrows; } 
      set {  arrows = value; }  
    }

    public void System()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        if (inputManager.moveDir != Vector2.zero)
            laterDirection = inputManager.moveDir;
    }

    public void ArrowAim()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            if (Arrows[i] != null)
            {
                idArrow = i;
                break;
            }
        }


        if (inputManager.circlePressed == true && arrows[idArrow] != null)
        {
            inArrow = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);

        }
        if (inputManager.circlePressed == false && inArrow == true)
        {

            arrowPos.SetActive(false);

            arrows[idArrow].SetActive(true);
            //arrows[0].transform.parent = null;
            arrows[idArrow] = null;
            inArrow = false;

        }
    }

    public void LifeDown()
    {
        if (life < 0)
            life = 0;

        life -= 1;
    }
}

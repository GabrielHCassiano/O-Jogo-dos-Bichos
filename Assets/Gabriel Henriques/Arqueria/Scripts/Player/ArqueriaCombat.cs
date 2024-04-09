using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArqueriaCombat : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject[] arrows;

    private bool inArrow;

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
        if (inputManager.circlePressed == true && arrows[0] != null)
        {
            inArrow = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);

        }
        if (inputManager.circlePressed == false && inArrow == true)
        {
            arrowPos.SetActive(false);

            arrows[0].SetActive(true);
            //arrows[0].transform.parent = null;
            arrows[0] = null;
            inArrow = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerID : MonoBehaviour
{
    [Header("Debug")]
    public int ID = 0;
    [Space]
    public InputManager inputManager;
    public GameObject[] controllers;

    private void Start()
    {
        /*
        controllers = GameObject.FindGameObjectsWithTag("Player");

        if (controllers.Length <= 1)
            this.ID = 1;
        else
        {
            foreach (var player in controllers)
            {
                var id = player.GetComponent<InputManager>().playerID;

                switch (id)
                {
                    case 1:
                        this.ID = 2;
                        break;
                    case 2:
                        this.ID = 3;
                        break;
                    case 3:
                        this.ID = 4;
                        break;
                }
            }
        }
        */
    }

    public void GetInputManager()
    {
        inputManager = GetComponentInChildren<InputManager>();
    }
}

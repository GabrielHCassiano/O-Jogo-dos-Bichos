using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InputManager : MonoBehaviour
{
    // Caso queiram testar se algum botão está pressionado, é com esses valores.
    [Header("Valores Debug")]
    public int playerID = 0;
    [Space]
    public Vector2 moveDir = Vector2.zero;
    [Space]
    public bool squarePressed = false;
    public bool trianglePressed = false;
    public bool circlePressed = false;
    public bool xPressed = false;
    [Space]
    public GameObject[] inputManagers;

    private void Start()
    {

        inputManagers = GameObject.FindGameObjectsWithTag("-PlayerInput-");

        if (inputManagers.Length <=1)
            this.playerID = 1;
        else
        {
            foreach (var player in inputManagers)
            {
                var id = player.GetComponent<InputManager>().playerID;

                switch(id)
                {
                    case 1:
                        this.playerID = 2;
                        break;
                    case 2:
                        this.playerID = 3;
                        break;
                    case 3:
                        this.playerID = 4;
                        break;
                }
            }
        }


    }

    //----------------------Input----------------------//

    // Analógico
    public void OnMove(InputAction.CallbackContext context )
    {
        moveDir = context.ReadValue<Vector2>();
    }

    // Botões pressionados no frame;
    public void OnSquare(InputAction.CallbackContext context)
    {
        squarePressed = context.action.triggered;
    }
    public void OnTriangle(InputAction.CallbackContext context)
    {
        trianglePressed = context.action.triggered;
    }
    public void OnX(InputAction.CallbackContext context)
    {
        xPressed = context.action.triggered;
    }
    public void OnCircle(InputAction.CallbackContext context)
    {
        circlePressed = context.action.triggered;
    }
}

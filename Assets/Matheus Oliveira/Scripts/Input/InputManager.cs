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

    PlayerInput playerInput;

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        // oops
        playerID = playerInput.playerIndex + 1;

        /*
        switch (GameManager.instance.playerCount)
        {
            case 0:
                playerID = 1;
                GameManager.instance.playerCount++;
                break;
            case 1:
                playerID = 2;
                GameManager.instance.playerCount++;
                break;
            case 2:
                playerID = 3;
                GameManager.instance.playerCount++;
                break;
            case 3:
                playerID = 4;
                GameManager.instance.playerCount++;
                break;
            default:
                return;
        }
        */
    }

    private void OnDestroy()
    {
        GameManager.instance.playerCount--;
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

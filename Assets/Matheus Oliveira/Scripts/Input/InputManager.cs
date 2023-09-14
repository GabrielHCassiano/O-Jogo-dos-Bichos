using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Caso queiram testar se algum botão está pressionado, é com esses valores.
    [Header("Valores Debug")]
    public Vector2 moveDir = Vector2.zero;
    [Space]
    public bool squarePressed = false;
    public bool trianglePressed = false;
    public bool circlePressed = false;
    public bool xPressed = false;

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

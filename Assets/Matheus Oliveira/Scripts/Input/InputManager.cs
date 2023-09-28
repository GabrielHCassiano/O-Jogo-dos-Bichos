using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
    public PlayerData playerData;

    PlayerInput playerInput;

    public void Start()
    {
        // Definindo o ID do controle pra ser utilizado no GameManager
        playerInput = GetComponent<PlayerInput>();
        playerID = playerInput.playerIndex + 1;

        if (playerID == 1)
            GameManager.instance.playerOneExists = true;
        GameManager.instance.playerCount++;
        GameManager.instance.inputManagers.Add(this.gameObject);

        CreatePlayerData();
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
    public void OnLost()
    {
        if (playerID == 1)
        {
            GameManager.instance.playerOneExists = false;
        }
        GameManager.instance.playerCount--;
        GameManager.instance.inputManagers.Remove(this.gameObject);
    }
    //--------------------------Data-------------------------//
    void CreatePlayerData()
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.playerName = "Player " + playerID;
    }
}

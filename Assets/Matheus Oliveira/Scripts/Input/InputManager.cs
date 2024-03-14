using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class InputManager : MonoBehaviour
{
    // Caso queiram testar se algum bot�o est� pressionado, � com esses valores.
    [Header("Valores Debug")]
    public int playerID = 0;
    public bool controllerConnected = false;
    [Space]
    public Vector2 moveDir = Vector2.zero;
    [Space]
    public bool canInput;
    [Space]
    public bool squarePressed = false;
    public bool trianglePressed = false;
    public bool circlePressed = false;
    public bool xPressed = false;
    [Space]
    public bool anyPressed = false;
    [Space]
    public PlayerData playerData;
    public string inputName;

    [Header("Buttons IDs Declarations")]
    public List<string> keyboardIDs;
    public List<string> playstationIDs;
    public List<string> xboxIDs;
    public List<string> nintendoIDs;
    [Space]
    [HideInInspector] public string squareId;
    [HideInInspector] public string triangleId;
    [HideInInspector] public string circleId;
    [HideInInspector] public string xId;

    PlayerInput playerInput;

    public void Start()
    {
        // Definindo o ID do controle pra ser utilizado no GameManager
        playerInput = GetComponent<PlayerInput>();
        playerID = playerInput.playerIndex + 1;

        controllerConnected = true;

        if (playerInput.devices[0] is Keyboard)
        {
            if(playerInput.devices[0].name == "Left")
            {
                inputName = "Keyboard Left";
                squareId = keyboardIDs[0];
                triangleId = keyboardIDs[1];
                circleId = keyboardIDs[2];
                xId = keyboardIDs[3];
            }
            else if (playerInput.devices[0].name == "Right")
            {
                inputName = "Keyboard Right";
                squareId = keyboardIDs[4];
                triangleId = keyboardIDs[5];
                circleId = keyboardIDs[6];
                xId = keyboardIDs[7];
            }
        }
        else if (playerInput.devices[0].description.manufacturer != "")
        {
            switch (playerInput.devices[0].description.manufacturer)
            {
                case "Sony Interactive Entertainment":
                    inputName = "Playstation";
                    squareId = playstationIDs[0];
                    triangleId = playstationIDs[1];
                    circleId = playstationIDs[2];
                    xId = playstationIDs[3];
                    break;
                case "Nintendo":
                    inputName = "Nintendo";
                    squareId = nintendoIDs[0];
                    triangleId = nintendoIDs[1];
                    circleId = nintendoIDs[2];
                    xId = nintendoIDs[3];
                    break;
                default:
                    inputName = "Generic";
                    squareId = playstationIDs[0];
                    triangleId = playstationIDs[1];
                    circleId = playstationIDs[2];
                    xId = playstationIDs[3];
                    break;
            }
        }
        else // os controles do xbox n tem a empresa que fez eles, KKKKKKKKKKKKKKKKKKKKKKK
        {
            if (playerInput.devices[0] is XInputController)
            {
                inputName = "Xbox";
                squareId = xboxIDs[0];
                triangleId = xboxIDs[1];
                circleId = xboxIDs[2];
                xId = xboxIDs[3];
            }
            else
            {
                inputName = "Generic";
                squareId = playstationIDs[0];
                triangleId = playstationIDs[1];
                circleId = playstationIDs[2];
                xId = playstationIDs[3];
            }
        }

        if (playerID == 1)
            GameManager.instance.playerOneExists = true;
        GameManager.instance.playerCount++;
        GameManager.instance.inputManagers.Add(this.gameObject);

        CreatePlayerData();
    }


    public void Update()
    {
        if(xPressed || squarePressed || trianglePressed || circlePressed)
            anyPressed = true;
        else
            anyPressed = false;
    }
    //----------------------Input----------------------//

    // Anal�gico
    public void OnMove(InputAction.CallbackContext context )
    {
        moveDir = context.ReadValue<Vector2>();
    }

    // Bot�es pressionados no frame;
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

    public void OnRegained(PlayerInput input)
    {
        if (playerID == 1)
            GameManager.instance.playerOneExists = true;
        GameManager.instance.playerCount++;
        controllerConnected = true;
    }
    public void OnLost(PlayerInput input)
    {
        if (playerID == 1)
        {
            GameManager.instance.playerOneExists = false;
        }
        GameManager.instance.playerCount--;
        controllerConnected = false;
    }

    //--------------------------Data-------------------------//
    void CreatePlayerData()
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.playerName = "Player " + playerID;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Criando uma inst�ncia do GameManager.
    public static GameManager instance;

    [Header("Debug Options")]
    [Tooltip("Esse bool � APENAS para for�ar um pause, em caso de erros.")]
    public bool forcedGamePause = false;
    [Space]
    public List<GameObject> controllers;
    public List<GameObject> inputManagers;
    [Space]
    public bool playerOneExists = false;
    public int playerCount = 0;

    PlayerInputManager playerInputManager;

    private void Awake()
    {
        // Garantindo que o GameManager n�o ser� deletado em transi��o de cena
        // e que se tiver 2 GameManagers, um ser� deletado.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        // No futuro, fazer um check para cer se n�o estamos no Menu
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        for (int i = 1; i <= 4; i++)
        {
            var player = Instantiate(RoomManager.instance.playerControllerPrefab);
            player.GetComponent<PlayerID>().ID = i;
        }
        
        SetControllerParents();
    }

    private void Update()
    {
        // Caso n�o tenha exatamente 4 players (o playerInputManager j� limita em 4), o jogo ter� um pause for�ado.
        // (fazer isso apenas em gameplay, n�o no menu)
        if(playerInputManager.playerCount < 4)
            forcedGamePause = true;
        else
            forcedGamePause = false;

        // DEBUG, Como o normal � testar o controle direto nos jogos e n�o come�ar do Menu, os controles n�o s�o atribuidos automaticamente,
        // pois eles n�o foram conectados ainda.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetControllerParents();
        }
    }

    public void SetControllerParents()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            controllers.Add(player);
        }

        // Conectando os controles e os players de mesmo ID
        foreach (var inputManager in inputManagers)
        {
            foreach (var controller in controllers)
            {
                if (inputManager.GetComponent<InputManager>().playerID == controller.GetComponent<PlayerID>().ID)
                {
                    inputManager.transform.parent = controller.transform;
                    controller.GetComponent<PlayerID>().inputManager = inputManager.GetComponent<InputManager>();
                }
            }
        }
    }
}

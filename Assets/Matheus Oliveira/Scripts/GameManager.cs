using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Criando uma instância do GameManager.
    public static GameManager instance;

    [Header("Debug Options")]
    [Tooltip("Esse bool é APENAS para forçar um pause, em caso de erros.")]
    public bool forcedGamePause = false;
    [Space]
    public GameObject[] controllers;
    public GameObject[] inputManagers;

    PlayerInputManager playerInputManager;

    private void Awake()
    {
        // Garantindo que o GameManager não será deletado em transição de cena
        // e que se tiver 2 GameManagers, um será deletado.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        // Caso não tenha exatamente 4 players (o playerInputManager já limita em 4), o jogo terá um pause forçado.
        if(playerInputManager.playerCount < 4)
            forcedGamePause = true;
        else
            forcedGamePause = false;

        //DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetControllerParents();
        }
    }

    public void SetControllerParents()
    {
        inputManagers = GameObject.FindGameObjectsWithTag("-PlayerInput-");
        controllers = GameObject.FindGameObjectsWithTag("Player");

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

        playerInputManager = GetComponent<PlayerInputManager>();
    }
}

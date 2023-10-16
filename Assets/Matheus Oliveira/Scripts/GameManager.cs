using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

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

    [Header("Game Variables")]
    [HideInInspector] public InputManager firstPlace;
    [HideInInspector] public InputManager secondPlace;
    [HideInInspector] public InputManager thirdPlace;
    [HideInInspector] public InputManager fourthPlace;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        // Caso n�o tenha exatamente 4 players (o playerInputManager j� limita em 4), o jogo ter� um pause for�ado.
        // (fazer isso apenas em gameplay, n�o no menu)
        if (playerInputManager.playerCount < 4)
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        for (int i = 1; i <= 4; i++)
        {
            var player = Instantiate(RoomManager.instance.playerControllerPrefab);
            player.GetComponent<PlayerID>().ID = i;
        }

        SetControllerParents();
    }

    public void SetControllerParents()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        if(controllers.Count == 0)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                controllers.Add(player);
            }
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
                    // controller.GetComponentInChildren<SpriteRenderer>().sprite = inputManager.GetComponent<InputManager>().playerData.playerSprite;
                }
            }
        }
    }
}

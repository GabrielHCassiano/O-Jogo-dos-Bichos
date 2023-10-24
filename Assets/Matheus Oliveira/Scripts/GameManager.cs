using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Criando uma instância do GameManager.
    public static GameManager instance;

    [Header("Debug Options")]
    [Tooltip("Esse bool é APENAS para forçar um pause, em caso de erros.")]
    public bool forcedGamePause = false;
    [Space]
    public List<GameObject> controllers;
    public List<GameObject> inputManagers;
    [Space]
    public bool playerOneExists = false;
    public int playerCount = 0;

    [Header("Game Stuff")]
    public int total_rounds = 3;
    public int rounds;
    public bool gameFinished = false;
    [Space]
    public bool minigameEnded = false; // os minigames tem que usar isso para determinar o fim do minigame;
    [Space]
    public GameObject scoreboardPanel;
    public List<GameObject> UiPlayers;
    List<GameObject> scoreboard;

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
        // Caso não tenha exatamente 4 players (o playerInputManager já limita em 4), o jogo terá um pause forçado.
        if (SceneManager.GetActiveScene().name == "Menu")
            return;
        if (playerInputManager.playerCount < 4)
            forcedGamePause = true;
        else
            forcedGamePause = false;

        // DEBUG, Como o normal é testar o controle direto nos jogos e não começar do Menu, os controles não são atribuidos automaticamente,
        // pois eles não foram conectados ainda.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetControllerParents();
        }

        CheckScores();
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

        scoreboardPanel.SetActive(false);

        SetControllerParents();

        if (SceneManager.GetActiveScene().name == "FinishScene")
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i].GetComponentInChildren<InputManager>() == null)
                    return;
                print(controllers[i].GetComponentInChildren<InputManager>().playerData.playerScoreIndex);
                switch (controllers[i].GetComponentInChildren<InputManager>().playerData.playerScoreIndex)
                {
                    case 1:
                        controllers[i].transform.position = RoomManager.instance.transform.Find("1").position;
                        break;
                    case 2:
                        controllers[i].transform.position = RoomManager.instance.transform.Find("2").position;
                        break;
                    case 3:
                        controllers[i].transform.position = RoomManager.instance.transform.Find("3").position;
                        break;
                    case 4:
                        controllers[i].transform.position = RoomManager.instance.transform.Find("4").position;
                        break;
                    default:
                        Debug.Log("ISSO É PRA SER IMPOSSIVEL");
                        break;
                }
            }
        }
    }

    void CheckScores()
    {
        if (minigameEnded)
        {
            StartCoroutine(MinigameEndSequence());
        }
    }

    IEnumerator MinigameEndSequence()
    {
        scoreboardPanel.SetActive(true);
        scoreboard = inputManagers;

        // achar um jeito melhor de fazer isso, pois no momento, após comparar 2 valores iguais, eles trocam de lugar
        scoreboard.Sort((p1, p2) => p1.GetComponent<InputManager>().playerData.playerScore.CompareTo(p2.GetComponent<InputManager>().playerData.playerScore));
        scoreboard.Reverse();
        for (int i = 0; i < scoreboard.Count; i++)
        {
            scoreboard[i].GetComponent<InputManager>().playerData.playerScoreIndex = scoreboard.IndexOf(scoreboard[i]) + 1;
            UiPlayers[i].GetComponentInChildren<TMP_Text>().text = "Player " + scoreboard[i].GetComponent<InputManager>().playerID + " Score: " + scoreboard[i].GetComponent<InputManager>().playerData.playerScore;
            UiPlayers[i].GetComponentInChildren<Image>().sprite = scoreboard[i].GetComponent<InputManager>().playerData.playerSprite;
        }

        controllers.Clear();
        foreach (GameObject inputs in GameManager.instance.inputManagers)
        {
            inputs.transform.parent = GameManager.instance.transform;
        }

        yield return new WaitForSeconds(5f);

        if(rounds < total_rounds)
        {
            rounds++;
            minigameEnded = false;
            // carregar próximo minigame
        }
        else if(!gameFinished)
        {
            // terminar o jogo
            SceneManager.LoadScene("FinishScene");
            gameFinished = true;
        }
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
                }
            }
        }
    }
}

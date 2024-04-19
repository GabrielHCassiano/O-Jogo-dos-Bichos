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

    [Header("Game Stuff")]
    public int total_rounds = 3;
    public int rounds;
    public bool gameFinished = false;
    [Space]
    public List<int> playedMinigames;
    public List<int> availableMinigames;
    public bool minigameEnded = false; // os minigames tem que usar isso para determinar o fim do minigame;
    bool runOnce = false;
    [Space]
    public TMP_Text startText;
    public GameObject scoreboardPanel;
    public List<GameObject> UiPlayers;
    [Space]
    [SerializeField] List<GameObject> scoreboard;

    PlayerInputManager playerInputManager;

    private string map;
    private bool play = false;

    private bool trueArrow = false;

    private void Awake()
    {
        // Garantindo que o GameManager n�o ser� deletado em transi��o de cena
        // e que se tiver 2 GameManagers, um ser� deletado.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Random.InitState((int)System.DateTime.Now.Ticks);
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
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        if (forcedGamePause)
        {
            for (int i = 0; i < inputManagers.Count ;i++)
            {
                inputManagers[i].GetComponent<InputManager>().canInput = false;
            }
        }
        else
        {
            for (int i = 0; i < inputManagers.Count; i++)
            {
                inputManagers[i].GetComponent<InputManager>().canInput = true;
            }
        }

        // DEBUG, Como o normal � testar o controle direto nos jogos e n�o come�ar do Menu, os controles n�o s�o atribuidos automaticamente,
        // pois eles n�o foram conectados ainda.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetControllerParents();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha9))
        {
            minigameEnded = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha0))
        {
            Destroy(FindAnyObjectByType<GameManager>().gameObject);
            SceneManager.LoadScene("Menu");
        }

        CheckScores();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;

        minigameEnded = false;

        for (int i = 1; i <= 4; i++)
        {
            var player = Instantiate(RoomManager.instance.playerControllerPrefab);
            player.GetComponent<PlayerID>().ID = i;
        }

        scoreboardPanel.SetActive(false);
        runOnce = false;

        SetControllerParents();

        if (SceneManager.GetActiveScene().name == "FinishScene")
        {
            for (int i = 0; i < inputManagers.Count; i++)
            {
                print(controllers[i].GetComponentInChildren<InputManager>().playerData.playerScoreIndex + 1);
                switch (controllers[i].GetComponentInChildren<InputManager>().playerData.playerScoreIndex + 1)
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
                        Debug.Log("ISSO � PRA SER IMPOSSIVEL");
                        break;
                }
            }
        }
        else
        {
            StartCoroutine(MinigameStartRoutine());
        }
    }

    IEnumerator MinigameStartRoutine()
    {
        forcedGamePause = true;
        startText.text = "";
        yield return new WaitForSeconds(1);
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(1);
        startText.text = "";
        forcedGamePause = false;
    }

    void CheckScores()
    {
        if (minigameEnded && !runOnce)
        {
            StartCoroutine(MinigameEndSequence());
            runOnce = true;
        }
    }

    IEnumerator Restart()
    {
        rounds = 0;
        minigameEnded = false;
        yield return new WaitForSeconds(8f);

        foreach (var scene in playedMinigames)
        {
            availableMinigames.Add(scene);
        }
        playedMinigames.Clear();

        for (int i = 0; i < inputManagers.Count; i++)
        {
            inputManagers[i].GetComponent<InputManager>().playerData.animatorController = null;
            inputManagers[i].GetComponent<InputManager>().playerData.playerSprite = null;
            inputManagers[i].GetComponent<InputManager>().playerData.playerScore = 0;
            inputManagers[i].GetComponent<InputManager>().playerData.playerScoreIndex = 0;
        }

        controllers.Clear();

        /*foreach (GameObject inputs in inputManagers)
        {
            inputs.transform.parent = transform;
        }*/

        inputManagers.Clear();

        SceneManager.LoadScene("Menu");
        gameFinished = false;
    }

    IEnumerator MinigameEndSequence()
    {
        rounds++;
        scoreboardPanel.SetActive(true);
        scoreboard = inputManagers;

        // achar um jeito melhor de fazer isso, pois no momento, ap�s comparar 2 valores iguais, eles trocam de lugar
        scoreboard.Sort((p1, p2) => p1.GetComponent<InputManager>().playerData.playerScore.CompareTo(p2.GetComponent<InputManager>().playerData.playerScore));
        scoreboard.Reverse();
        for (int i = 0; i < inputManagers.Count; i++)
        {
            scoreboard[i].GetComponent<InputManager>().playerData.playerScoreIndex = scoreboard.IndexOf(scoreboard[i]);
            UiPlayers[i].GetComponentInChildren<TMP_Text>().text = "Player " + scoreboard[i].GetComponent<InputManager>().playerID + " Score: " + scoreboard[i].GetComponent<InputManager>().playerData.playerScore;
            UiPlayers[i].GetComponentInChildren<Image>().sprite = scoreboard[i].GetComponent<InputManager>().playerData.playerSprite;
        }

        controllers.Clear();

        yield return new WaitForSeconds(5f);

        foreach (GameObject inputs in inputManagers)
        {
            inputs.transform.parent = transform;
        }

        if (rounds < total_rounds)
        {
            if (map == "Random")
            {
                Random.InitState((int)System.DateTime.Now.Ticks);
                int index = Random.Range(0, availableMinigames.Count - 1);
                int theSceneIndex = availableMinigames[index];
                availableMinigames.RemoveAt(index);
                foreach (var scene in playedMinigames)
                {
                    availableMinigames.Add(scene);
                }
                playedMinigames.Clear();
                playedMinigames.Add(theSceneIndex);
                SceneManager.LoadScene(theSceneIndex);
            }
            else 
                SceneManager.LoadScene(map);
        }
        else if (!gameFinished)
        {
            // terminar o jogo
            SceneManager.LoadScene("FinishScene");
            StartCoroutine(Restart());
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

    public string Map
    {
        get { return map; }
        set { map = value; }
    }

    public bool Play
    {
        get { return play; }
        set { play = value; }
    }

    public bool TrueArrow
    { 
        get { return trueArrow;  } 
        set { trueArrow = value; }
    }

}

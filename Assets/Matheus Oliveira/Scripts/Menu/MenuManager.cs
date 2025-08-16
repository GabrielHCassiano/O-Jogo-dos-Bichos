using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Options")]
    public InputManager inputManager;
    [SerializeField] public bool started = false;
    [Space]
    [SerializeField] List<MenuPlayerUImanager> players;

    [Header("Declarations")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] List<GameObject> Menus;
    [Space]
    [SerializeField] TMP_Text returnText;

    public int currentMenu = 0;
    bool[] play = new bool[4];

    float returnConfirmTime = 0;

    public int countPlayer = 0;

    private void Awake()
    {
        instance = this;
        Random.InitState((int)System.DateTime.Now.Ticks);

        foreach (MenuPlayerUImanager player in players)
        {
            player.confirmed = false;
            //play = false;
        }
    }

    void Update()
    {
        InputManager();

        //play = true;

        for(int i = 0; i < 4; i++)
        {
            if (!players[i].ConfirmedColor && play[i])
            {
                play[i] = false;
                countPlayer -= 1;
            }
            else if (players[i].ConfirmedColor && !play[i])
            {
                play[i] = true;
                countPlayer += 1;
            }
        }

        if(countPlayer >= 2 && GameManager.instance.inputManagers.Count == countPlayer && GameManager.instance.Play)
        {
            foreach (GameObject inputs in GameManager.instance.inputManagers)
            {
                inputs.transform.parent = GameManager.instance.transform;
            }
            GameManager.instance.rounds = 0;

            if (GameManager.instance.Map == "Aleatório")
            {
                Random.InitState((int)System.DateTime.Now.Ticks);
                int index = Random.Range(0, GameManager.instance.availableMinigames.Count);
                int theSceneIndex = GameManager.instance.availableMinigames[index];
                GameManager.instance.availableMinigames.RemoveAt(index);
                GameManager.instance.playedMinigames.Add(theSceneIndex);
                SceneManager.LoadScene(theSceneIndex);
            }
            else 
                SceneManager.LoadScene(GameManager.instance.Map);
        }

        if(inputManager != null)
        {
            returnText.text = "Segure <size=60><sprite=" + inputManager.circleId + "></size> para sair";
        }

        if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha1) == true)
            players[0].ConfirmedColor = true;
        if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha2) == true)
            players[1].ConfirmedColor = true;
        if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha3) == true)
            players[2].ConfirmedColor = true;
        if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha4) == true)
            players[3].ConfirmedColor = true;
    }

    void InputManager()
    {
        // pegar o inputmanager uma vez
        if (GameManager.instance.playerOneExists && inputManager == null)
        {
            foreach (GameObject input in GameManager.instance.inputManagers)
            {
                if (input.GetComponent<InputManager>().playerID == 1)
                    inputManager = input.GetComponent<InputManager>();
            }
        }

        // se o controle desconectar, remover ele do controle
        if (!GameManager.instance.playerOneExists)
            inputManager = null;

        // se o controle apertar qualquer botão cardinal, ir para o menu principal,
        // caso contrário voltar para o menu principal
        if (!started && inputManager != null)
        {
            if (inputManager.xPressed)
            {
                Menus[0].SetActive(false);
                SwitchToMenu(1);
                started = true;
            }
        }
        else if (inputManager == null)
        {
            started = false;
            Menus[0].SetActive(true);
        }

        if (inputManager != null && currentMenu == 2)
        {
            if (inputManager.circlePressed)
            {
                returnConfirmTime += Time.deltaTime;
                if(returnConfirmTime >= 1)
                {
                    for (int i = 0; i < 4; i++)
                        FindObjectsOfType<MenuPlayerUImanager>()[i].DelayStartTime = 0.4f;
                    SwitchToMenu(1);
                    //Destroy(FindAnyObjectByType<GameManager>().gameObject);
                    //SceneManager.LoadScene("Start");
                    //Destroy(inputManager);
                }
            }
            else
            {
                returnConfirmTime = 0;
            }
        }
    }

    void SwitchToMenu(int menuIndex)
    {
        foreach(GameObject menu in Menus)
        {
            menu.SetActive(false);
        }
        Menus[menuIndex].SetActive(true);

        if (Menus[menuIndex].GetComponentInChildren<Button>() != null)
            Menus[menuIndex].GetComponentInChildren<Button>().Select();

        currentMenu = menuIndex;
    }

    //-------------------------------Buttons Functions-------------------------------//

    public void Play()
    {
        SwitchToMenu(2);
    }

    public void Options()
    {
        SwitchToMenu(3);
    }

    public void Credits()
    {
        SwitchToMenu(4);
    }

    public void Return()
    {
        SwitchToMenu(1);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        // só um jeito de testar se o botão funciona sem ter que buildar o jogo.
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

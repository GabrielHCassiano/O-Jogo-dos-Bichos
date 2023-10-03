using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Options")]
    public InputManager inputManager;
    [SerializeField] bool started = false;
    [Space]
    [SerializeField] List<MenuPlayerUImanager> players;

    [Header("Declarations")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] List<GameObject> Menus;

    public int currentMenu = 0;

    bool play;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        InputManager();

        play = true;

        foreach (MenuPlayerUImanager player in players)
        {
            if (!player.confirmed)
            {
                play = false;
                break;
            }
        }

        if(play)
        {
            foreach (GameObject inputs in GameManager.instance.inputManagers)
            {
                inputs.transform.parent = GameManager.instance.transform;
            }
            SceneManager.LoadScene("CenaTeste"); // trocar pela lógica de random
        }
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
            if (inputManager.anyPressed)
            {
                SwitchToMenu(1);
                started = true;
            }
        }
        else if (inputManager == null)
        {
            started = false;
            SwitchToMenu(0);
        }

        if (inputManager != null && currentMenu > 1)
        {
            if (inputManager.circlePressed)
                SwitchToMenu(currentMenu - 1);
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

    public void Quit()
    {
        #if UNITY_EDITOR
        // só um jeito de testar se o botão funciona sem ter que buildar o jogo.
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

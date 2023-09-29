using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Options")]
    public InputManager inputManager;
    bool started = false;

    [Header("Declarations")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] List<GameObject> Menus;
    [SerializeField] List<GameObject> StartButtons;

    void Update()
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

        if (!started && inputManager != null)
        {
            if (inputManager.xPressed)
            {
                SwitchToMenu();
                started = true;
            }
        }
        else if (inputManager == null)
        {
            started = false;
            SwitchToStart();
        }
    }

    void SwitchToMenu()
    {
        StartPanel.SetActive(false);
        Menus[0].SetActive(true);
        StartButtons[0].GetComponent<Button>().Select();
    }

    void SwitchToStart()
    {
        StartPanel.SetActive(true);
        foreach (GameObject panel in Menus)
            panel.SetActive(false);
    }

    //-------------------------------Buttons Functions-------------------------------//

    public void Play()
    {
        Menus[0].SetActive(false);
        Menus[1].SetActive(true);
        StartButtons[1].GetComponent<Button>().Select();
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

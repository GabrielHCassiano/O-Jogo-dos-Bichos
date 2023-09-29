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

    void Update()
    {
        if (GameManager.instance.playerOneExists && inputManager == null)
        {
            foreach (GameObject input in GameManager.instance.inputManagers)
            {
                if (input.GetComponent<InputManager>().playerID == 1)
                    inputManager = input.GetComponent<InputManager>();
            }
        }

        if (!GameManager.instance.playerOneExists)
        {
            inputManager = null;
            started = false;
        }

        if (inputManager != null)
            started = true;

        if (started)
            SwitchToMenu();
        else
            SwitchToStart();
    }

    void SwitchToMenu()
    {
        StartPanel.SetActive(false);
        Menus[0].SetActive(true);
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

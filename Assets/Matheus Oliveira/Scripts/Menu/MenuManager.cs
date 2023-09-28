using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Options")]
    public InputManager inputManager;
    bool started = false;

    [Header("Declarations")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] GameObject MenuPanel;

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
            started = false;

        if (inputManager != null)
            started = true;

        if (started)
            SwitchToMenu();
        else
            SwitchToStart();
    }

    void SwitchToMenu()
    {
        MenuPanel.SetActive(true);
        StartPanel.SetActive(false);
    }

    void SwitchToStart()
    {
        StartPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuPlayerUImanager : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] RectTransform noPlayer;
    [SerializeField] RectTransform hasPlayer;

    [SerializeField] InputManager inputManager;
    [SerializeField] int playerID = 0;

    private void Start()
    {
        playerID = Convert.ToInt32(name);

        hasPlayer.Find("Player").GetComponent<TMP_Text>().text = "Player " + playerID;
    }

    void Update()
    {
        if (MenuManager.instance.currentMenu != 2)
            return;

        StartUp();
        Control();
    }

    void StartUp()
    {
   
        if (inputManager == null)
        {
            foreach (GameObject input in GameManager.instance.inputManagers)
            {
                if (input.GetComponent<InputManager>().playerID == playerID)
                {
                    inputManager = input.GetComponent<InputManager>();
                    return;
                }
            }

            noPlayer.gameObject.SetActive(true);
            hasPlayer.gameObject.SetActive(false);
        }
        else
        {
            noPlayer.gameObject.SetActive(false);
            hasPlayer.gameObject.SetActive(true);
        }
    }

    void Control()
    {
        if (inputManager == null)
            return;


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuPlayerUImanager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] RectTransform noPlayer;
    [SerializeField] RectTransform hasPlayer;
    [Space]
    [SerializeField] InputManager inputManager;
    [SerializeField] TMP_Text confirmationText;
    [SerializeField] int playerID = 0;
    [Space]
    [SerializeField] Image playerSprite;
    [SerializeField] List<Sprite> playerSprites;
    [SerializeField] int playerSpriteIndex = 0;
    float lastXinput = 0;
    [Space]
    [SerializeField] float confirmationTime = 0;
    public bool confirmed = false;
    private void Start()
    {
        playerID = Convert.ToInt32(name);

        hasPlayer.Find("Player").GetComponent<TMP_Text>().text = "Player " + playerID;
        playerSprite = hasPlayer.Find("Image").GetComponent<Image>();
        confirmationText = hasPlayer.Find("Confirm").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (MenuManager.instance.currentMenu != 2)
            return;

        StartUp();
        Control();
    }

    private void OnDisable()
    {
        playerSpriteIndex = 0;
        confirmationTime = 0;
        confirmed = false;
        inputManager = null;
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

        if(inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmed && confirmationTime == 0)
        {
            if (playerSpriteIndex <= 0)
                playerSpriteIndex = playerSprites.Count - 1;
            else
                playerSpriteIndex--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmed && confirmationTime == 0)
        {
            if (playerSpriteIndex >= playerSprites.Count - 1)
                playerSpriteIndex = 0;
            else
                playerSpriteIndex++;
        }

        playerSprite.sprite = playerSprites[playerSpriteIndex];
        lastXinput = inputManager.moveDir.x;

        if (inputManager.anyPressed && !confirmed)
        {
            confirmationTime += Time.deltaTime;
            if (confirmationTime > 1)
                confirmed = true;
            else
                confirmed = false;
        }
        else
        {
            confirmationTime = 0;
        }
        if(confirmed && inputManager.trianglePressed)
            confirmed = false;

        if (confirmed)
        {
            inputManager.playerData.playerSprite = playerSprites[playerSpriteIndex];
            confirmationText.text = "Pronto!";
        }
        else
        {
            confirmationText.text = "Segure qualquer botão para confirmar";
        }
    }
}

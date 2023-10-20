using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.Animations;

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
    [SerializeField] List<AnimatorController> animatorControllers;
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
            noPlayer.GetComponentInChildren<TMP_Text>().text = "Aperte qualquer botão\npara entrar";
            hasPlayer.gameObject.SetActive(false);
        }
        else
        {
            if(inputManager.controllerConnected)
            {
                noPlayer.gameObject.SetActive(false);
                hasPlayer.gameObject.SetActive(true);
            }
            else
            {
                noPlayer.gameObject.SetActive(true);
                noPlayer.GetComponentInChildren<TMP_Text>().text = "Reconecte seu controle";
                hasPlayer.gameObject.SetActive(false);
            }
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

        if (inputManager.xPressed && !confirmed)
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
        if(confirmed && inputManager.circlePressed)
            confirmed = false;

        if (confirmed)
        {
            inputManager.playerData.animatorController = animatorControllers[playerSpriteIndex];
            inputManager.playerData.playerSprite = playerSprites[playerSpriteIndex];

            confirmationText.text = "Confirmado!\nAperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationText.text = "Segure <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }
}

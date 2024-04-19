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
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] List<Sprite> playerSprites;
    [SerializeField] List<Sprite> playerUiIcons;
    [SerializeField] List<RuntimeAnimatorController> animatorControllers;
    [SerializeField] int playerSpriteIndex = 0;
    float lastXinput = 0;
    [Space]
    [SerializeField] float confirmationTime = 0;
    public bool confirmed = false;

    [SerializeField] private float delayTime = 0;

    [SerializeField] private GameObject selectMaps;
    [SerializeField] private int mapIndex = 0;
    [SerializeField] private bool confirmedMap = false;
    [SerializeField] float confirmationMapTime = 0;
    [SerializeField] TMP_Text confirmationMapText;
    [SerializeField] private GameObject selectRounds;
    private int rounds = 3;
    [SerializeField] float confirmationRoundTime = 0;
    [SerializeField] TMP_Text confirmationRoundText;
    private bool confirmedRound = false;

    [SerializeField] Sprite[] playerSpriteSecret;
    [SerializeField] RuntimeAnimatorController[] animatorControllerSecret;
    [SerializeField] private string secretCode;
    private bool secretChar;
    private int idSecret;
    private bool canX;
    private bool canO;
    [SerializeField] private GameObject trueArrow;

    private void Start()
    {
        playerID = Convert.ToInt32(name);

        hasPlayer.Find("Player").GetComponent<TMP_Text>().text = "Player " + playerID;
        playerSprite = hasPlayer.Find("Sprite").GetComponent<SpriteRenderer>();
        confirmationText = hasPlayer.Find("Confirm").GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (MenuManager.instance.currentMenu != 2)
            return;

        StartUp();
        Control();
        SecretChar();
    }

    private void OnDisable()
    {
        playerSpriteIndex = 0;
        confirmationTime = 0;
        confirmed = false;
        inputManager = null;
    }

    public void SecretChar()
    {
        if (inputManager != null)
        {
            SecrectCode();

            if (secretChar == false && secretCode == "V \\V > V /V < V \\V > X " /*&& inputManager.moveDir.y == -1 && inputManager.trianglePressed == true && inputManager.squarePressed == true*/)
            {
                idSecret = 0;
                secretChar = true;
                confirmed = true;
            }
            if (secretChar == false && secretCode == "< V A > X ")
            {
                idSecret = 1;
                secretChar = true;
                confirmed = true;
            }
            if (confirmed == false && secretChar == true)
            {
                secretChar = false;
                secretCode = "";
                canX = false;
                canO = false;
            }

            if (secretCode == "< > V V X O ")
            {
                secretCode = "";
                trueArrow.SetActive(GameManager.instance.TrueArrow = !GameManager.instance.TrueArrow);

            }
        }
    }

    public void SecrectCode()
    {
        if (inputManager.xPressed == true && canX == false)
        {
            StopAllCoroutines();
            secretCode += "X ";
            canX = true;
        }

        if (inputManager.circlePressed == true && canO == false)
        {
            StopAllCoroutines();
            secretCode += "O ";
            canO = true;
        }

        if (inputManager.moveDir.x > 0 && inputManager.moveDir.y < 0)
        {
            StopAllCoroutines();
            secretCode += "\\V ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.x > 0 && inputManager.moveDir.y > 0)
        {
            StopAllCoroutines();
            secretCode += "/A ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.x < 0 && inputManager.moveDir.y < 0)
        {
            StopAllCoroutines();
            secretCode += "/V ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.x < 0 && inputManager.moveDir.y > 0)
        {
            StopAllCoroutines();
            secretCode += "\\A ";
        }

        if (inputManager.moveDir.x < 0)
        {
            StopAllCoroutines();
            secretCode += "< ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.x > 0)
        {
            StopAllCoroutines();
            secretCode += "> ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.y < 0)
        {
            StopAllCoroutines();
            secretCode += "V ";
            inputManager.moveDir = Vector2.zero;
        }
        if (inputManager.moveDir.y > 0)
        {
            StopAllCoroutines();
            secretCode += "A ";
            inputManager.moveDir = Vector2.zero;
        }

        if (secretCode != "")
            StartCoroutine(CodeClean());
    }

    public IEnumerator CodeClean()
    {
        yield return new WaitForSeconds(0.3f);
        secretCode = "";
        canX = false;
        canO = false;
    }

    public IEnumerator SecretIcon()
    {
        playerSprite.sprite = playerSpriteSecret[2];
        yield return new WaitForSeconds(1f);
        playerSprite.sprite = playerSpriteSecret[2];

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
            if (inputManager.controllerConnected)
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

        if (playerID == 1)
        {
            GameManager.instance.Play = confirmedRound;

            if (delayTime < 0)
                delayTime = 0;
            if (delayTime > 0)
                delayTime -= Time.deltaTime;

            selectMaps.SetActive(confirmed);
            selectRounds.SetActive(confirmedMap);

            if (confirmed == true)
                SelectMap();
            if (confirmedMap == true)
                SelectRound();
        }

        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmed && confirmationTime == 0)
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
        playerAnimator.runtimeAnimatorController = animatorControllers[playerSpriteIndex];
        playerAnimator.SetBool("isWalking", confirmed);
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

        if (confirmed && inputManager.circlePressed && confirmedMap == false && delayTime == 0)
            confirmed = false;

        if (confirmed)
        {
            inputManager.playerData.animatorController = animatorControllers[playerSpriteIndex];
            inputManager.playerData.playerSprite = playerUiIcons[playerSpriteIndex];

            if (secretChar == true)
            {
                playerSprite.sprite = playerSpriteSecret[idSecret];
                playerAnimator.runtimeAnimatorController = animatorControllerSecret[idSecret];
                inputManager.playerData.animatorController = animatorControllerSecret[idSecret];
                inputManager.playerData.playerSprite = playerSpriteSecret[idSecret];
            }

            confirmationText.text = "Confirmado!\nAperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationText.text = "Segure <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void SelectMap()
    {
        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmedMap && confirmationMapTime == 0)
        {
            if (mapIndex <= 0)
                mapIndex = 2;
            else
                mapIndex--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmedMap && confirmationMapTime == 0)
        {
            if (mapIndex >= 2)
                mapIndex = 0;
            else
                mapIndex++;
        }

        switch (mapIndex)
        {
            case 0:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Random";
                break;
            case 1:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Queimada";
                break;
            case 2:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Arqueria";
                break;
        }

        if (inputManager.xPressed && !confirmedMap)
        {
            confirmationMapTime += Time.deltaTime;
            if (confirmationMapTime > 1)
                confirmedMap = true;
            else
                confirmedMap = false;
        }
        else
        {
            confirmationMapTime = 0;
        }
        if (confirmedMap && inputManager.circlePressed && confirmedRound == false && delayTime == 0)
        {
            delayTime = 0.1f;
            confirmedMap = false;
        }

        if (confirmedMap)
        {
            GameManager.instance.Map = selectMaps.GetComponentInChildren<TextMeshProUGUI>().text;

            confirmationMapText.text = "Aperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationMapText.text = "Segure <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void SelectRound()
    {
        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmedRound && confirmationTime == 0)
        {
            if (rounds <= 1)
                rounds = 5;
            else
                rounds--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmedRound && confirmationTime == 0)
        {
            if (rounds >= 5)
                rounds = 1;
            else
                rounds++;
        }

        selectRounds.GetComponentInChildren<TextMeshProUGUI>().text = rounds.ToString();

        if (inputManager.xPressed && !confirmedRound)
        {
            confirmationRoundTime += Time.deltaTime;
            if (confirmationRoundTime > 1)
                confirmedRound = true;
            else
                confirmedRound = false;
        }
        else
        {
            confirmationRoundTime = 0;
        }
        if (confirmedRound && inputManager.circlePressed)
        {
            delayTime = 0.1f;
            confirmedRound = false;
        }

        if (confirmedRound)
        {
            GameManager.instance.total_rounds = rounds;

            confirmationRoundText.text = "Aperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationRoundText.text = "Segure <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }
}

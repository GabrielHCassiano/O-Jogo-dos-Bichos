using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEditor;
using System.Runtime.CompilerServices;

public class MenuPlayerUImanager : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] RectTransform noPlayer;
    [SerializeField] RectTransform hasPlayer;
    [Space]
    [SerializeField] InputManager inputManager;
    [SerializeField] RawImage charIcon;
    [SerializeField] TMP_Text inputText;
    [SerializeField] private GameObject confirmedUI;
    [SerializeField] private GameObject confirmedColorUI;
    [SerializeField] int playerID = 0;
    [Space]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] List<Sprite> playerSprites;
    [SerializeField] List<Sprite> playerUiIcons;
    [SerializeField] List<Color> playerColors;
    [SerializeField] List<RuntimeAnimatorController> animatorControllers;
    [SerializeField] int playerSpriteIndex = 0;
    float lastXinput = 0;
    [Space]
    public bool confirmed = false;

    [SerializeField] private GameObject menuPlayer;
    private float delayStartTime = 0.4f;

    [SerializeField] private GameObject selectMaps;
    [SerializeField] private int mapIndex = 0;
    [SerializeField] private bool confirmedMap = false;
    [SerializeField] TMP_Text confirmationMapText;
    [SerializeField] private GameObject trailMap;
    [SerializeField] private GameObject selectRounds;
    private int rounds = 3;
    [SerializeField] TMP_Text confirmationRoundText;
    private bool confirmedRound = false;
    [SerializeField] private GameObject trailRound;

    [SerializeField] private Material materialMain;
    [SerializeField] private Material materialDefalt;
    [SerializeField] private Material materialRainbow;
    [SerializeField] private Material materialBrasil;
    [SerializeField] private Material materialBlackWhite;

    private MenuPlayerUImanager[] players;

    [SerializeField] private GameObject colorText;
    [SerializeField] private RawImage colorImage;
    [SerializeField] private int colorIndex = 0;
    private bool startColor = false;
    private bool confirmedColor = false;

    [SerializeField] Sprite[] playerSpriteSecret;
    [SerializeField] RuntimeAnimatorController[] animatorControllerSecret;
    [SerializeField] private string secretCode;
    private bool secretChar;
    private int idSecret;
    private string key;
    private bool canX;
    private bool canO;
    [SerializeField] private GameObject trueArrow;

    private Texture defaltTexture;
    private bool secretColorRainbow = false;
    private bool secretColorBrasil = false;
    private bool secretColorBlackWhite = false;
    private bool specialColor = false;

    private void Start()
    {
        playerID = Convert.ToInt32(name);

        hasPlayer.Find("Player").GetComponent<TMP_Text>().text = "Player " + playerID;
        playerSprite = hasPlayer.Find("Sprite").GetComponent<SpriteRenderer>();
        //confirmationText = hasPlayer.Find("Confirm").GetComponent<TMP_Text>();

        players = FindObjectsOfType<MenuPlayerUImanager>();

        defaltTexture = colorImage.texture;
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

        if (menuPlayer.activeSelf == false)
            return;

        if (delayStartTime < 0)
            delayStartTime = 0;
        if (delayStartTime > 0)
            delayStartTime -= Time.deltaTime;

        //colorText.SetActive(confirmed);

        if (confirmed)
            SelectColor();

        if (playerID == 1)
        {
            GameManager.instance.Play = confirmedRound;

            selectMaps.SetActive(confirmedColor);
            selectRounds.SetActive(confirmedMap);

            if (confirmedColor == true)            
                SelectMap();
            if (confirmedMap == true)
                SelectRound();

            if (selectMaps.activeSelf == true && !confirmedMap)
                trailMap.GetComponent<TrailRenderer>().sortingLayerName = "UI";
            else
                trailMap.GetComponent<TrailRenderer>().sortingLayerName = "Background";

            if (selectRounds.activeSelf == true && !confirmedRound)
                trailRound.GetComponent<TrailRenderer>().sortingLayerName = "UI";
            else
                trailRound.GetComponent<TrailRenderer>().sortingLayerName = "Background";
        }

        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmed)
        {
            if (playerSpriteIndex <= 0)
                playerSpriteIndex = playerSprites.Count - 1;
            else
                playerSpriteIndex--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmed)
        {
            if (playerSpriteIndex >= playerSprites.Count - 1)
                playerSpriteIndex = 0;
            else
                playerSpriteIndex++;
        }

        playerSprite.sprite = playerSprites[playerSpriteIndex];

        charIcon.texture = playerUiIcons[playerSpriteIndex].texture;
        playerAnimator.runtimeAnimatorController = animatorControllers[playerSpriteIndex];
        playerAnimator.SetBool("isWalking", confirmed);
        lastXinput = inputManager.moveDir.x;

        if (inputManager.xPressed && !confirmed && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            confirmed = true;
        }

        if (confirmed && inputManager.circlePressed && confirmedColor == false && delayStartTime == 0)
            confirmed = false;

        inputText.text = "Aperte <size=60><sprite=" + inputManager.xId + "></size> para confirmar " +
                             "\nAperte <size=60><sprite=" + inputManager.circleId + "></size> para voltar";

        confirmedUI.SetActive(confirmed);

        if (confirmed)
        {

            if (!startColor)
            {
                playerSprite.material = materialMain;
                playerSprite.material.SetColor("_OutlineColor", playerColors[colorIndex]);
                GetColor(true);
            }

            inputManager.playerData.animatorController = animatorControllers[playerSpriteIndex];
            inputManager.playerData.playerSprite = playerUiIcons[playerSpriteIndex];

            if (secretChar == true)
            {
                playerSprite.sprite = playerSpriteSecret[idSecret];
                playerAnimator.runtimeAnimatorController = animatorControllerSecret[idSecret];
                inputManager.playerData.animatorController = animatorControllerSecret[idSecret];
                inputManager.playerData.playerSprite = playerSpriteSecret[idSecret];
                charIcon.texture = playerSpriteSecret[idSecret].texture;
            }
            else
            {
                if (inputManager.GetComponent<Red_HoodAttack>() != null)
                {
                    Destroy(inputManager.GetComponent<Red_HoodAttack>());
                }
            }

            //confirmationText.text = "Confirmado!\nAperte <size=60><sprite=" + inputManager.circleId + "></size> para voltar";
        }
        else
        {
            playerSprite.material = materialDefalt;
            startColor = false;

            //confirmationText.text = "Aperte <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void GetColor(bool positive)
    {
        for (int i = 0; i < 4; i++)
        {
            if (colorIndex == players[i].colorIndex && playerID != players[i].playerID && players[i].confirmed && positive)
            {
                if (colorIndex >= playerColors.Count - 1)
                    colorIndex = 0;
                else
                {
                    colorIndex++;
                    GetColor(true);
                }
            }
            else if (colorIndex == players[i].colorIndex && playerID != players[i].playerID && players[i].confirmed && !positive)
            {
                if (colorIndex <= 0)
                    colorIndex = playerColors.Count - 1;
                else
                {
                    colorIndex--;
                    GetColor(false);
                }
            }
            else
                startColor = true;
        }
    }

    public void SelectColor()
    {
        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmedColor)
        {
            if (colorIndex <= 0)
            {
                colorIndex = playerColors.Count - 1;
                GetColor(false);
            }
            else
            {
                colorIndex--;
                GetColor(false);
            }
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmedColor)
        {
            if (colorIndex >= playerColors.Count - 1)
            {
                colorIndex = 0;
                GetColor(true);
            }
            else
            {
                colorIndex++;
                GetColor(true);
            }
        }

        if (secretColorRainbow)
        {
            playerSprite.material = materialRainbow;
            colorImage.material = materialRainbow;
            colorImage.texture = inputManager.playerData.playerSprite.texture;
            colorImage.color = Color.white;
            specialColor = true;
        }

        if (secretColorBrasil)
        {
            playerSprite.material = materialBrasil;
            colorImage.material = materialBrasil;
            colorImage.texture = inputManager.playerData.playerSprite.texture;
            colorImage.color = Color.white;
            specialColor = true;
        }

        if (secretColorBlackWhite)
        {
            playerSprite.material = materialBlackWhite;
            colorImage.material = materialBlackWhite;
            colorImage.texture = inputManager.playerData.playerSprite.texture;
            colorImage.color = Color.white;
            specialColor = true;
        }

        if (!secretColorRainbow && !secretColorBrasil && !secretColorBlackWhite)
        {
            playerSprite.material = materialMain;
            playerSprite.material.SetColor("_OutlineColor", playerColors[colorIndex]);
            colorImage.material = materialDefalt;
            colorImage.texture = defaltTexture;
            colorImage.color = playerColors[colorIndex];
            specialColor = false;
        }

        if (inputManager.xPressed && !confirmedColor && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            confirmedColor = true;
        }

        if (confirmedColor && inputManager.circlePressed && confirmedMap == false && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            secretColorRainbow = false;
            secretColorBrasil = false;
            secretColorBlackWhite = false;
            confirmedColor = false;
        }

        confirmedColorUI.SetActive(confirmedColor);

        if (confirmedColor)
        {
            inputManager.playerData.color = playerColors[colorIndex];
            inputManager.playerData.material = playerSprite.material;
            inputManager.playerData.specialColor = specialColor;
            //confirmationMapText.text = "Aperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            //confirmationMapText.text = "Aperte <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void SelectMap()
    {
        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmedMap)
        {
            if (mapIndex <= 0)
                mapIndex = 3;
            else
                mapIndex--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmedMap)
        {
            if (mapIndex >= 3)
                mapIndex = 0;
            else
                mapIndex++;
        }

        switch (mapIndex)
        {
            case 0:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Aleatório";
                break;
            case 1:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Queimada";
                break;
            case 2:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Arqueria";
                break;
            case 3:
                selectMaps.GetComponentInChildren<TextMeshProUGUI>().text = "Pescaria";
                break;
        }


        if (inputManager.xPressed && !confirmedMap && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            confirmedMap = true;
        }

        if (confirmedMap && inputManager.circlePressed && confirmedRound == false && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            confirmedMap = false;
        }

        if (confirmedMap)
        {
            GameManager.instance.Map = selectMaps.GetComponentInChildren<TextMeshProUGUI>().text;

            confirmationMapText.text = "Aperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationMapText.text = "Aperte <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void SelectRound()
    {
        if (inputManager.moveDir.x == -1 && inputManager.moveDir.x != lastXinput && !confirmedRound)
        {
            if (rounds <= 1)
                rounds = 5;
            else
                rounds--;
        }
        else if (inputManager.moveDir.x == 1 && inputManager.moveDir.x != lastXinput && !confirmedRound)
        {
            if (rounds >= 5)
                rounds = 1;
            else
                rounds++;
        }

        selectRounds.GetComponentInChildren<TextMeshProUGUI>().text = rounds.ToString();


        if (inputManager.xPressed && !confirmedRound && delayStartTime == 0)
        {
            delayStartTime = 0.4f;
            confirmedRound = true;
        }

        if (confirmedRound && inputManager.circlePressed)
        {
            delayStartTime = 0.4f;
            confirmedRound = false;
        }

        if (confirmedRound)
        {
            GameManager.instance.total_rounds = rounds;

            confirmationRoundText.text = "Aperte <size=60><sprite=" + inputManager.circleId + "></size> para desconfirmar";
        }
        else
        {
            confirmationRoundText.text = "Aperte <size=60><sprite=" + inputManager.xId + "></size> para confirmar";
        }
    }

    public void SecretChar()
    {
        if (inputManager != null)
        {
            SecrectCode();

            if (secretChar == false && secretCode == "D DF F D DF F X " && !confirmedColor)
            {
                idSecret = 0;
                inputManager.AddComponent<Red_HoodAttack>();
                secretChar = true;
                confirmed = true;
            }
            if (secretChar == false && secretCode == "B D U F X " && !confirmedColor)
            {
                idSecret = 1;
                secretChar = true;
                confirmed = true;
            }
            if (secretChar == false && secretCode == "D DB B UB U UF F DF D X " && !confirmedColor)
            {
                idSecret = 2;
                secretChar = true;
                confirmed = true;
            }
            if (secretChar == false && secretCode == "U U D D B F B F () X " && !confirmedColor)
            {
                idSecret = 3;
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

            if (secretCode == "B F D D X () ")
            {
                secretCode = "";
                trueArrow.SetActive(GameManager.instance.TrueArrow = !GameManager.instance.TrueArrow);

            }

            if (!secretColorRainbow)
                secretColorRainbow = SecrectRainbowColor();

            if (!secretColorBrasil)
                secretColorBrasil = SecrectBrasilColor();

            if (!secretColorBlackWhite)
                secretColorBlackWhite = SecrectBlackWhiteColor();

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
            secretCode += "() ";
            canO = true;
        }

        if (inputManager.moveDir.x > 0 && inputManager.moveDir.y < 0 && key != "DF ")
        {
            StopAllCoroutines();
            key = "DF ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.x > 0 && inputManager.moveDir.y > 0 && key != "UF ")
        {
            StopAllCoroutines();
            key = "UF ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.x < 0 && inputManager.moveDir.y < 0 && key != "DB ")
        {
            StopAllCoroutines();
            key = "DB ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.x < 0 && inputManager.moveDir.y > 0 && key != "UB ")
        {
            StopAllCoroutines();
            key = "UB ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }

        if (inputManager.moveDir.x < 0 && inputManager.moveDir.y == 0 && key != "B ")
        {
            StopAllCoroutines();
            key = "B ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.x > 0 && inputManager.moveDir.y == 0 && key != "F ")
        {
            StopAllCoroutines();
            key = "F ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.y < 0 && inputManager.moveDir.x == 0 && key != "D ")
        {
            StopAllCoroutines();
            key = "D ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }
        if (inputManager.moveDir.y > 0 && inputManager.moveDir.x == 0 && key != "U ")
        {
            StopAllCoroutines();
            key = "U ";
            inputManager.moveDir = Vector2.zero;
            secretCode += key;
        }

        if (key != "")
            StartCoroutine(KeyClean());

        if (secretCode != "")
            StartCoroutine(CodeClean());
    }

    public IEnumerator KeyClean()
    {
        yield return new WaitForSeconds(0.1f);
        key = "";
    }

    public IEnumerator CodeClean()
    {
        yield return new WaitForSeconds(0.4f);
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

    public bool SecrectRainbowColor()
    {
        if (secretCode == "UB DF X ")
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i].secretColorRainbow && playerID != players[i].playerID)
                {
                    return false;
                }
            }

            confirmedColor = true;
            secretCode = "";
            return true;
        }
        return false;
    }

    public bool SecrectBrasilColor()
    {
        if (secretCode == "U D U D X ")
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i].secretColorBrasil && playerID != players[i].playerID)
                {
                    return false;
                }
            }

            confirmedColor = true;
            secretCode = "";
            return true;
        }
        return false;
    }

    public bool SecrectBlackWhiteColor()
    {
        if (secretCode == "UF DB X ")
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i].secretColorBlackWhite && playerID != players[i].playerID)
                {
                    return false;
                }
            }

            confirmedColor = true;
            secretCode = "";
            return true;
        }
        return false;
    }

    public int PlayerID
    {
        get { return playerID; }
        set { playerID = value; }
    }

    public bool ConfirmedColor
    {
        get { return confirmedColor; }
        set { confirmedColor = value; }
    }

    public int ColorIndex
    {
        get { return colorIndex; }
        set { colorIndex = value; }
    }

    public float DelayStartTime
    { 
        get { return delayStartTime; }
        set { delayStartTime = value; }
    }

    public bool SecretColorRainbow
    {
        get { return secretColorRainbow; }
        set { secretColorRainbow = value; }
    }

    public bool SecretColorBrasil
    {
        get { return secretColorBrasil; }
        set { secretColorBrasil = value; }
    }

    public bool SecretColorBlackWhite
    {
        get { return secretColorBlackWhite; }
        set { secretColorBlackWhite = value; }
    }


}

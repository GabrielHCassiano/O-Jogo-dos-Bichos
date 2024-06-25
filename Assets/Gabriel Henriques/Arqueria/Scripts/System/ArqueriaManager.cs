using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArqueriaManager : MonoBehaviour
{

    private GameObject[] players;
    [SerializeField] private Transform[] spawnPlayers;

    [SerializeField] private Image[] attackUI;
    [SerializeField] private Image[] jumpUI;
    [SerializeField] private Image[] dashUI;

    [SerializeField] private Sprite[] spriteX;
    [SerializeField] private Sprite[] spriteD;
    [SerializeField] private Sprite[] spriteRT;

    [SerializeField] private Image[] life1;
    [SerializeField] private Image[] life2;
    [SerializeField] private Image[] life3;

    [SerializeField] private Material material;

    private bool[] lifeUI = new bool[5];

    [SerializeField] private bool[] lossPlayer;
    private bool[] contLoss = new bool[5];

    [SerializeField] private int lossGame = 0;
    [SerializeField] private bool winGame;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        DebugLoss();
        ManagerUI();
    }

    public void DebugLoss()
    {
        if (players != null)
        {
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha1) == true && players[0] != null)
                players[0].GetComponent<ArqueriaCombat>().Life = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha2) == true && players[1] != null)
                players[1].GetComponent<ArqueriaCombat>().Life = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha3) == true && players[2] != null)
                players[2].GetComponent<ArqueriaCombat>().Life = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha4) == true && players[3] != null)
                players[3].GetComponent<ArqueriaCombat>().Life = 0;
        }
    }

    public void WinLogic(int i)
    {
        if (lossPlayer[i] == true && contLoss[i] == false)
        {
            contLoss[i] = true;
            lossGame += 1;
            if (lossGame == 2)
                players[i].GetComponentInChildren<InputManager>().playerData.playerNewScore += 5;
            if (lossGame == 3)
                players[i].GetComponentInChildren<InputManager>().playerData.playerNewScore += 10;
            if (lossGame == 4)
            {
                players[i].GetComponentInChildren<InputManager>().playerData.playerNewScore += 15;
                FindObjectOfType<GameManager>().minigameEnded = true;
            }
        }
        else if (lossGame == 3 && lossPlayer[i] == false && contLoss[i] == false)
        {
            players[i].GetComponentInChildren<InputManager>().playerData.playerNewScore += 15;
            contLoss[i] = true;
            FindObjectOfType<GameManager>().minigameEnded = true;
        }
    }

    public void ManagerUI()
    {
        if (players != null)
        {
            for (int i = 0; i < 4; i++)
            {
                SpriteButton(i);
                LifeUI(i);
                lossPlayer[i] = players[i].GetComponent<ArqueriaCombat>().Lose;
                WinLogic(i);
            }
        }
    }

    public void LifeUI(int i)
    {
        if (lifeUI[i] == false && players[i].GetComponentInChildren<InputManager>() != null)
        {
            life1[i].sprite = players[i].GetComponentInChildren<InputManager>().playerData.playerSprite;
            life2[i].sprite = players[i].GetComponentInChildren<InputManager>().playerData.playerSprite;
            life3[i].sprite = players[i].GetComponentInChildren<InputManager>().playerData.playerSprite;
            lifeUI[i] = true;
        }

        switch (players[i].GetComponent<ArqueriaCombat>().Life)
        {
            case 0:
                life1[i].gameObject.SetActive(false);
                life2[i].gameObject.SetActive(false);
                life3[i].gameObject.SetActive(false);
                break;
            case 1:
                life1[i].gameObject.SetActive(false);
                life2[i].gameObject.SetActive(false);
                break;
            case 2:
                life1[i].gameObject.SetActive(false);
                break;
        }
    }

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < 4; i++)
        {
            players[i].transform.position = spawnPlayers[i].position;
            if (players[i].GetComponentInChildren<InputManager>() != null)
            {
                players[i].GetComponentInChildren<SpriteRenderer>().material = players[i].GetComponentInChildren<InputManager>().playerData.material;
                if (!players[i].GetComponentInChildren<InputManager>().playerData.specialColor)
                    players[i].GetComponentInChildren<SpriteRenderer>().material.SetColor("_OutlineColor", players[i].GetComponentInChildren<InputManager>().playerData.color);
                life1[i].material = players[i].GetComponentInChildren<SpriteRenderer>().material;
                life2[i].material = players[i].GetComponentInChildren<SpriteRenderer>().material;
                life3[i].material = players[i].GetComponentInChildren<SpriteRenderer>().material;
            }
        }
    }

    public void SpriteButton(int i)
    {
        if (players[i].GetComponentInChildren<InputManager>() != null)
        {
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Keyboard Left")
            {
                attackUI[i].sprite = spriteD[0];
                jumpUI[i].sprite = spriteX[0];
                dashUI[i].sprite = spriteRT[0];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Playstation")
            {
                attackUI[i].sprite = spriteD[1];
                jumpUI[i].sprite = spriteX[1];
                dashUI[i].sprite = spriteRT[1];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Xbox")
            {
                attackUI[i].sprite = spriteD[2];
                jumpUI[i].sprite = spriteX[2];
                dashUI[i].sprite = spriteRT[2];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Nintendo")
            {
                attackUI[i].sprite = spriteD[3];
                jumpUI[i].sprite = spriteX[3];
                dashUI[i].sprite = spriteRT[3];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Generic")
            {
                attackUI[i].sprite = spriteD[4];
                jumpUI[i].sprite = spriteX[4];
                dashUI[i].sprite = spriteRT[4];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Keyboard Right")
            {
                attackUI[i].sprite = spriteD[5];
                jumpUI[i].sprite = spriteX[5];
                dashUI[i].sprite = spriteRT[5];
            }
        }
    }
}

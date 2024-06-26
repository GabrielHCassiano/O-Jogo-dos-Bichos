using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PescariaManager : MonoBehaviour
{
    private GameObject[] players;
    [SerializeField] private Transform[] spawnPlayers;

    private InputManager[] inputPlayers = new InputManager[5];
    [SerializeField] private bool[] lossPlayer;
    private bool endgame = false;

    [SerializeField] private Image[] life;
    private bool[] lifeUI = new bool[5];

    [SerializeField] private Image[] attackUI;
    [SerializeField] private Image[] dashUI;

    [SerializeField] private Sprite[] spriteX;
    [SerializeField] private Sprite[] spriteD;

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
                players[0].GetComponent<Pescando>().Lose = true;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha2) == true && players[1] != null)
                players[1].GetComponent<Pescando>().Lose = true;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha3) == true && players[2] != null)
                players[2].GetComponent<Pescando>().Lose = true;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha4) == true && players[3] != null)
                players[3].GetComponent<Pescando>().Lose = true;
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
                lossPlayer[i] = players[i].GetComponent<Pescando>().Lose;
                WinLogic(i);
            }
        }
    }

    public void LifeUI(int i)
    {
        if (lifeUI[i] == false && players[i].GetComponentInChildren<InputManager>() != null)
        {
            life[i].sprite = players[i].GetComponentInChildren<InputManager>().playerData.playerSprite;
            lifeUI[i] = true;
        }

        if(players[i].GetComponent<Pescando>().Lose)
            life[i].gameObject.SetActive(false);
    }

    public void WinLogic(int i)
    {

        if (lossPlayer[0] && lossPlayer[2] && !endgame)
        {
            endgame = true;
            FindObjectOfType<GameManager>().minigameEnded = true;
            if (inputPlayers[0] != null)
                inputPlayers[0].playerData.playerNewScore += 10;
            if (inputPlayers[2] != null)
                inputPlayers[2].playerData.playerNewScore += 10;
            if (inputPlayers[1] != null)
                inputPlayers[1].playerData.playerNewScore += 30;
            if (inputPlayers[3] != null)
                inputPlayers[3].playerData.playerNewScore += 30;
        }
        else if (lossPlayer[1] && lossPlayer[3] && !endgame)
        {
            endgame = true;
            FindObjectOfType<GameManager>().minigameEnded = true;
            if (inputPlayers[1] != null)
                inputPlayers[1].playerData.playerNewScore += 10;
            if (inputPlayers[3] != null)
                inputPlayers[3].playerData.playerNewScore += 10;
            if (inputPlayers[0] != null)
                inputPlayers[0].playerData.playerNewScore += 30;
            if (inputPlayers[2] != null)
                inputPlayers[2].playerData.playerNewScore += 30;

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
                inputPlayers[i] = players[i].GetComponentInChildren<InputManager>();
                players[i].GetComponent<Pescando>().SpriteRenderer.material = players[i].GetComponentInChildren<InputManager>().playerData.material;
                if (!players[i].GetComponentInChildren<InputManager>().playerData.specialColor)
                    players[i].GetComponent<Pescando>().SpriteRenderer.material.SetColor("_OutlineColor", players[i].GetComponentInChildren<InputManager>().playerData.color);
                life[i].material = players[i].GetComponent<Pescando>().SpriteRenderer.material;
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
                dashUI[i].sprite = spriteX[0];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Playstation")
            {
                attackUI[i].sprite = spriteD[1];
                dashUI[i].sprite = spriteX[1];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Xbox")
            {
                attackUI[i].sprite = spriteD[2];
                dashUI[i].sprite = spriteX[2];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Nintendo")
            {
                attackUI[i].sprite = spriteD[3];
                dashUI[i].sprite = spriteX[3];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Generic")
            {
                attackUI[i].sprite = spriteD[4];
                dashUI[i].sprite = spriteX[4];
            }
            if (players[i].GetComponentInChildren<InputManager>().inputName == "Keyboard Right")
            {
                attackUI[i].sprite = spriteD[5];
                dashUI[i].sprite = spriteX[5];
            }
        }
    }
}

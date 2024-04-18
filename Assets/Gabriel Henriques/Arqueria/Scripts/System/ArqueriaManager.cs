using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static KeyboardSplitter;

public class ArqueriaManager : MonoBehaviour
{

    private GameObject[] players;
    [SerializeField] private Transform[] spawnPlayers;

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
                players[i].GetComponent<ArqueriaMove>().ScoreValue += 10;
            if (lossGame == 3)
                players[i].GetComponent<ArqueriaMove>().ScoreValue += 20;
            if (lossGame == 4)
            {
                players[i].GetComponent<ArqueriaMove>().ScoreValue += 30;
                FindObjectOfType<GameManager>().minigameEnded = true;
            }
        }
        else if (lossGame == 3 && lossPlayer[i] == false && contLoss[i] == false)
        {
            players[i].GetComponent<ArqueriaMove>().ScoreValue += 30;
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
                lossPlayer[i] = players[i].GetComponent<ArqueriaCombat>().Lose;
                WinLogic(i);
            }
        }
    }

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < 4; i++)
        {
            players[i].transform.position = spawnPlayers[i].position;
        }
    }
}

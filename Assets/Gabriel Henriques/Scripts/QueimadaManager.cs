using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QueimadaManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    private GameObject[] player;
    private PlayerID playerID;

    [SerializeField] private Animator[] areas;
    [SerializeField] private Animator area1;


    [SerializeField] private GameObject arrow;

    [SerializeField] private Image[] life1;
    [SerializeField] private Image[] life2;
    [SerializeField] private Image[] life3;

    [SerializeField] private Sprite[] spriteLife;

    [SerializeField] private Slider[] force;
    [SerializeField] private bool[] lossPlayer;
    private bool[] contLoss = new bool[5];

    [SerializeField] private int lossGame = 0;
    [SerializeField] private bool winGame;
    // Start is called before the first frame update


    private bool[] lifeUI = new bool[5];

    void Start()
    {
        StartCoroutine(StarCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        ManagerUI();
        AreasPlayer();
    }

    public void AreasPlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            areas[i].SetBool("Player1", lossPlayer[0]);
            areas[i].SetBool("Player2", lossPlayer[1]);
            areas[i].SetBool("Player3", lossPlayer[2]);
            areas[i].SetBool("Player4", lossPlayer[3]);
        }
    }

    public void WinLogic(int i)
    {
        if (lossPlayer[i] == true && contLoss[i] == false)
        {
            contLoss[i] = true;
            lossGame += 1;
            if (lossGame == 2)
                player[i].GetComponent<GetAndAttackControl>().ScoreValue += 25;
            if (lossGame == 3)
                player[i].GetComponent<GetAndAttackControl>().ScoreValue += 50;
        } 
        else if (lossGame == 3 && lossPlayer[i] == false && contLoss[i] == false)
        {
            player[i].GetComponent<GetAndAttackControl>().ScoreValue += 100;
            contLoss[i] = true;
            FindObjectOfType<GameManager>().minigameEnded = true;
        }
    }

    public void ManagerUI()
    {
        if (player != null)
        {
            for (int i = 0; i < 4; i++)
            {
                LifeUI(i);
                force[i].value = player[i].GetComponent<GetAndAttackControl>().forceValue / 100;
                lossPlayer[i] = player[i].GetComponent<StatusPlayer>().loseValue;
                WinLogic(i);
            }
        }
    }

    public void LifeUI(int i)
    {
        if (lifeUI[i] == false)
        {
            life1[i].sprite = player[i].GetComponent<GetAndAttackControl>().SpriteUIValue;
            life2[i].sprite = player[i].GetComponent<GetAndAttackControl>().SpriteUIValue;
            life3[i].sprite = player[i].GetComponent<GetAndAttackControl>().SpriteUIValue;
            lifeUI[i] = true;
        }
        
        switch (player[i].GetComponent<StatusPlayer>().lifeValue)
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

    IEnumerator StarCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        player = GameObject.FindGameObjectsWithTag("Player");
        if (player != null)
        {
            for (int i = 0; i < 4; i++)
            {
                player[i].AddComponent<PlayerQueimadaControl>();
                playerID = player[i].GetComponent<PlayerID>();
                player[i].transform.position = spawnPos[playerID.ID - 1].position;
                player[i].GetComponent<GetAndAttackControl>().ArrowValue = Instantiate(arrow);
            }
        }
    }
}

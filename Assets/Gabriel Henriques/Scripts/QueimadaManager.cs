using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueimadaManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    private GameObject[] player;
    private PlayerID playerID;

    [SerializeField] private Animator[] areas;
    [SerializeField] private Animator area1;


    [SerializeField] private GameObject arrow;

    [SerializeField] private Sprite[] spriteX;
    [SerializeField] private Sprite[] spriteO;
    [SerializeField] private Sprite[] spriteD;

    [SerializeField] private Image[] life1;
    [SerializeField] private Image[] life2;
    [SerializeField] private Image[] life3;

    [SerializeField] private Sprite[] spriteLife;

    [SerializeField] private Slider[] force;
    [SerializeField] private bool[] lossPlayer;
    private bool[] contLoss = new bool[5];

    [SerializeField] private Toggle[] getBall;
    [SerializeField] private Image[] getBallBack;
    [SerializeField] private Image[] getBallCheck;
    [SerializeField] private Toggle[] inDash;
    [SerializeField] private Image[] inDashBack;
    [SerializeField] private Image[] inDashCheck;
    [SerializeField] private Image[] attackUI;


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
        DebugLoss();
        ManagerUI();
        AreasPlayer();
    }

    public void DebugLoss()
    {
        if(player != null)
        {
            if(Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha1) == true && player[0] != null) 
                player[0].GetComponent<StatusPlayer>().lifeValue = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha2) == true && player[1] != null)
                player[1].GetComponent<StatusPlayer>().lifeValue = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha3) == true && player[2] != null)
                player[2].GetComponent<StatusPlayer>().lifeValue = 0;
            if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKey(KeyCode.Alpha4) == true && player[3] != null)
                player[3].GetComponent<StatusPlayer>().lifeValue = 0;
        }
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
                player[i].GetComponent<GetAndAttackControl>().ScoreValue += 10;
            if (lossGame == 3)
                player[i].GetComponent<GetAndAttackControl>().ScoreValue += 20;
        } 
        else if (lossGame == 3 && lossPlayer[i] == false && contLoss[i] == false)
        {
            player[i].GetComponent<GetAndAttackControl>().ScoreValue += 30;
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
                inDash[i].isOn = player[i].GetComponent<TopDownController>().canDash;
                getBall[i].isOn = !player[i].GetComponent<GetAndAttackControl>().inGetBallValue;
                WinLogic(i);
                SpriteButton(i);
            }
        }
    }

    public void LifeUI(int i)
    {
        if (lifeUI[i] == false && player[i].GetComponent<GetAndAttackControl>().inputManagereValue != null)
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

    public void SpriteButton(int i)
    {
        if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue != null)
        {
            if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue.inputName == "Keyboard")
            {
                attackUI[i].sprite = spriteD[0];
                getBallBack[i].sprite = spriteO[0];
                getBallCheck[i].sprite = spriteO[0];
                inDashBack[i].sprite = spriteX[0];
                inDashCheck[i].sprite = spriteX[0];
            }
            if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue.inputName == "Playstation")
            {
                attackUI[i].sprite = spriteD[1];
                getBallBack[i].sprite = spriteO[1];
                getBallCheck[i].sprite = spriteO[1];
                inDashBack[i].sprite = spriteX[1];
                inDashCheck[i].sprite = spriteX[1];
            }
            if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue.inputName == "Xbox")
            {
                attackUI[i].sprite = spriteD[2];
                getBallBack[i].sprite = spriteO[2];
                getBallCheck[i].sprite = spriteO[2];
                inDashBack[i].sprite = spriteX[2];
                inDashCheck[i].sprite = spriteX[2];
            }
            if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue.inputName == "Nintendo")
            {
                attackUI[i].sprite = spriteD[3];
                getBallBack[i].sprite = spriteO[3];
                getBallCheck[i].sprite = spriteO[3];
                inDashBack[i].sprite = spriteX[3];
                inDashCheck[i].sprite = spriteX[3];
            }
            if (player[i].GetComponent<GetAndAttackControl>().inputManagereValue.inputName == "Generic")
            {
                attackUI[i].sprite = spriteD[4];
                getBallBack[i].sprite = spriteO[4];
                getBallCheck[i].sprite = spriteO[4];
                inDashBack[i].sprite = spriteX[4];
                inDashCheck[i].sprite = spriteX[4];
            }
        }
    }
}

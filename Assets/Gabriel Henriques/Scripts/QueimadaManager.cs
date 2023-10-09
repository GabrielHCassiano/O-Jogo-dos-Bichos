using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QueimadaManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    private GameObject[] player;
    private PlayerID playerID;

    [SerializeField] private GameObject[] areas;
    [SerializeField] private GameObject[] areas2;

    [SerializeField] private GameObject arrow;

    [SerializeField] private TextMeshProUGUI[] life;
    [SerializeField] private Slider[] force;
    // Start is called before the first frame update
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
        if(player[0].GetComponent<StatusPlayer>().loseValue == true && player[1].GetComponent<StatusPlayer>().loseValue == false &&
           player[2].GetComponent<StatusPlayer>().loseValue == false && player[3].GetComponent<StatusPlayer>().loseValue == false)
        {
            areas[0].SetActive(false);
            areas[2].SetActive(false);
            areas2[0].SetActive(true);
        }
        else if(player[0].GetComponent<StatusPlayer>().loseValue == false && player[1].GetComponent<StatusPlayer>().loseValue == true &&
                player[2].GetComponent<StatusPlayer>().loseValue == false && player[3].GetComponent<StatusPlayer>().loseValue == false)
        {
            areas[1].SetActive(false);
            areas[0].SetActive(false);
            areas2[1].SetActive(true);
        }
        else if (player[0].GetComponent<StatusPlayer>().loseValue == false && player[1].GetComponent<StatusPlayer>().loseValue == false &&
                 player[2].GetComponent<StatusPlayer>().loseValue == true && player[3].GetComponent<StatusPlayer>().loseValue == false)
        {
            areas[2].SetActive(false);
            areas[3].SetActive(false);
            areas2[2].SetActive(true);
        }
        else if (player[0].GetComponent<StatusPlayer>().loseValue == false && player[1].GetComponent<StatusPlayer>().loseValue == false &&
                 player[2].GetComponent<StatusPlayer>().loseValue == false && player[3].GetComponent<StatusPlayer>().loseValue == true)
        {
            areas[3].SetActive(false);
            areas[1].SetActive(false);
            areas2[3].SetActive(true);
        }


        if (player[0].GetComponent<StatusPlayer>().loseValue == true && player[1].GetComponent<StatusPlayer>().loseValue == true)
        {
            areas[0].SetActive(true);
            areas[1].SetActive(false);
            areas2[0].SetActive(false);
            areas2[1].SetActive(false);

        }
        else if (player[0].GetComponent<StatusPlayer>().loseValue == true && player[2].GetComponent<StatusPlayer>().loseValue == true)
        {
            areas[2].SetActive(true);
            areas[0].SetActive(false);
            areas2[0].SetActive(false);
            areas2[2].SetActive(false);
        }
        else if (player[1].GetComponent<StatusPlayer>().loseValue == true && player[3].GetComponent<StatusPlayer>().loseValue == true)
        {
            areas[1].SetActive(true);
            areas[3].SetActive(false);
            areas2[1].SetActive(false);
            areas2[3].SetActive(false);
        }
        else if (player[2].GetComponent<StatusPlayer>().loseValue == true && player[3].GetComponent<StatusPlayer>().loseValue == true)
        {
            areas[3].SetActive(true);
            areas[2].SetActive(false);
            areas2[2].SetActive(false);
            areas2[3].SetActive(false);
        }

    }

    public void ManagerUI()
    {
        for (int i = 0; i < 4; i++)
        {
            life[i].text = player[i].GetComponent<StatusPlayer>().lifeValue.ToString();
            force[i].value = player[i].GetComponent<GetAndAttackControl>().forceValue/100;
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
                player[i].AddComponent<StatusPlayer>();
                player[i].AddComponent<GetAndAttackControl>();
                player[i].AddComponent<Knockback>();
                playerID = player[i].GetComponent<PlayerID>();
                player[i].transform.position = spawnPos[playerID.ID - 1].position;
                player[i].GetComponent<GetAndAttackControl>().ArrowValue = Instantiate(arrow);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QueimadaManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    private GameObject[] player;
    private PlayerID playerID;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StarCooldown());
    }

    // Update is called once per frame
    void Update()
    {

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
                playerID = player[i].GetComponent<PlayerID>();
                player[i].transform.position = spawnPos[playerID.ID - 1].position;
            }
        }
    }
}

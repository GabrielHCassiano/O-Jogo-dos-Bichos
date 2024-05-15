using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PescariaManager : MonoBehaviour
{
    private GameObject[] players;
    [SerializeField] private Transform[] spawnPlayers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        
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

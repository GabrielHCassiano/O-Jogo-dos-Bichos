using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    public float min_x, min_y, max_x, max_y;
    public GameObject reset;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn do Objeto
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        var Random_x = UnityEngine.Random.Range(min_x, max_x);
        var Random_y = UnityEngine.Random.Range(min_y, max_y);

        reset.transform.position = new Vector2(Random_x, Random_y);

        //ReSpawn
        //StartCoroutine(ReSpawn());
    }

    public IEnumerator Reset(){
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        var Random_x = UnityEngine.Random.Range(min_x, max_x);
        var Random_y = UnityEngine.Random.Range(min_y, max_y);
        
        reset.transform.position = new Vector2(Random_x, Random_y);
        yield return new WaitForSeconds(5);
        reset.SetActive(true);
    }
}

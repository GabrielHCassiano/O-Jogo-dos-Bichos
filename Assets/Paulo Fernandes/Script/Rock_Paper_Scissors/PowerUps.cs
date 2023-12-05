using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] public bool is_rock = false;
    [SerializeField] public bool is_paper = false;
    [SerializeField] public bool is_scissors = false;
    [SerializeField] public float powerCooldown = 7f;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Rock"){
            is_rock = true;
            is_paper = false;
            is_scissors = false;
            collision.gameObject.SetActive(false);
            StartCoroutine(ResetPower());
            StartCoroutine(collision.gameObject.GetComponentInParent<ReSpawn>().Reset());
        }

        if(collision.tag == "Paper"){
            is_rock = false;
            is_paper = true;
            is_scissors = false;
            collision.gameObject.SetActive(false);
            StartCoroutine(ResetPower());
            StartCoroutine(collision.gameObject.GetComponentInParent<ReSpawn>().Reset());
        }

        if(collision.tag == "Scissors"){
            is_rock = false;
            is_paper = false;
            is_scissors = true;
            collision.gameObject.SetActive(false);
            StartCoroutine(ResetPower());
            StartCoroutine(collision.gameObject.GetComponentInParent<ReSpawn>().Reset());
        }
    }

    private IEnumerator ResetPower(){
        yield return new WaitForSeconds(powerCooldown);
        is_rock = false;
        is_paper = false;
        is_scissors = false;
    }
}

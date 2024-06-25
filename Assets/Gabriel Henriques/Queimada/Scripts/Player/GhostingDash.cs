using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GhostingDash : MonoBehaviour
{

    private TopDownController controller;
    [SerializeField] private SpriteRenderer[] ghosting;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TopDownController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.doDash && controller.canDash)
        {
           StartCoroutine(GhostingAnim());
        }
    }

    IEnumerator GhostingAnim()
    {
        for (int i = 0; i < ghosting.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            ghosting[i].sprite = GetComponentInChildren<SpriteRenderer>().sprite;
            ghosting[i].material = GetComponentInChildren<SpriteRenderer>().material;
            ghosting[i].material.SetColor("_OutlineColor", GetComponentInChildren<InputManager>().playerData.color);
            ghosting[i].transform.localScale = new Vector3(GetComponentInChildren<SpriteRenderer>().transform.localScale.x, transform.localScale.y, transform.localScale.z);
            ghosting[i].gameObject.SetActive(true);
            ghosting[i].transform.parent = null;
        }
        ResetGhosting();
    }

    public void ResetGhosting()
    {
        for (int i = 0; i < ghosting.Length; i++)
        {
            ghosting[i].transform.parent = transform;
            ghosting[i].transform.position = transform.position;
            ghosting[i].gameObject.SetActive(false);
        }
    }
}

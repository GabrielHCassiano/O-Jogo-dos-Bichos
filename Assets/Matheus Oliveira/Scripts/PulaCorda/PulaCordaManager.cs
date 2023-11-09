using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PulaCordaManager : MonoBehaviour
{
    public float speedModifier = 2;
    public bool rotate = false;
    [Space]
    [SerializeField] GameObject rope;
    [SerializeField] GameObject ropeShadow;
    [Space]
    [SerializeField] BoxCollider2D ropeColl;
    [Space]
    public int lossCount = 0;

    public List<GameObject> shadows;

    [SerializeField] float angleSoFar = 0;
    float angleLastFrame;

    void Start()
    {
        for (int i = 0; i < GameManager.instance.controllers.Count; i++)
        {
            GameManager.instance.controllers[i].transform.position = RoomManager.instance.transform.GetChild(0).Find((i + 1).ToString()).transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RopeAnim();
        RopeColl();
    }

    void RopeColl()
    {
        float angle = -rope.transform.localEulerAngles.x;
        angleSoFar += angleLastFrame - angle;
        angleLastFrame = angle;

        if (angleSoFar >= 0 && angleSoFar <= 5 || angleSoFar >= 355 && angleSoFar <= 360)
        {
            ropeColl.enabled = true;
        }
        else
        {
            ropeColl.enabled = false;
        }
    }

    void RopeAnim()
    {
        if (rope.transform.eulerAngles.x < 180)
        {
            rope.GetComponent<LineRenderer>().sortingOrder = 1;
        }
        else
        {
            rope.GetComponent<LineRenderer>().sortingOrder = -1;
        }

        if (!rotate)
            return;
        rope.transform.Rotate(new Vector3(Time.deltaTime * speedModifier, 0, 0));
        ropeShadow.transform.Rotate(new Vector3(Time.deltaTime * -speedModifier, 0, 0));
    }
}

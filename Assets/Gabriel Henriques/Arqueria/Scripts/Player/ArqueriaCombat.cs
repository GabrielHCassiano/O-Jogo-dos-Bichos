using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArqueriaCombat : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private int life;
    private bool shield;
    private bool lose;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private int idArrow;
    private bool inArrow;

    [SerializeField] private Sprite trueArrow;
    [SerializeField] private RuntimeAnimatorController trueArrowAnim;

    private Vector2 laterDirection;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponentInChildren<InputManager>();

        life = 3;
        laterDirection.x = 1;

        if (GameManager.instance.TrueArrow == true)
        {
            arrowPos.GetComponentInChildren<Animator>().runtimeAnimatorController = trueArrowAnim;
            for (int i = 0; i < 3; i++)
                arrows[i].GetComponentInChildren<SpriteRenderer>().sprite = trueArrow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LoseLogic();
        if (inputManager == null)
            return;
        if (inputManager.canInput == false)
            return;
        System();
        ArrowAim();
    }

    public bool Lose
    {
        get { return lose; }
        set { lose = value; }
    }

    public int Life
    {
        get { return life; }
        set { life = value; }
    }

    public Vector3 LaterDirection
    {
        get { return laterDirection; }
        set { laterDirection = value; }
    }

    public GameObject[] Arrows
    { 
      get { return arrows; } 
      set {  arrows = value; }  
    }

    public void System()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        if (inputManager != null && inputManager.moveDir != Vector2.zero)
            laterDirection = inputManager.moveDir;

    }

    public void ArrowAim()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            if (Arrows[i] != null)
            {
                idArrow = i;
                break;
            }
        }


        if (inputManager != null && inputManager.circlePressed == true && arrows[idArrow] != null)
        {
            inArrow = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);

        }
        if (inputManager != null && inputManager.circlePressed == false && inArrow == true)
        {
            if (shield == true)
            {
                StopAllCoroutines();
                shield = false;
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
            arrowPos.SetActive(false);

            arrows[idArrow].SetActive(true);
            //arrows[0].transform.parent = null;
            arrows[idArrow] = null;
            inArrow = false;

        }
    }

    public void LifeDown()
    {
        if (shield == false)
        {
            life -= 1;
            StartCoroutine(DamageAnim());
        }
    }

    public void LoseLogic()
    {
        if (life <= 0)
        {
            life = 0;
            lose = true;
        }
        if (lose == true)
        {
            StartCoroutine(LoseCooldown());
        }
    }

    IEnumerator LoseCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        if (GetComponentInChildren<InputManager>() != null)
            GetComponentInChildren<InputManager>().transform.parent = null;
        gameObject.SetActive(false);
    }

    IEnumerator DamageAnim()
    {
        shield = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.2f);
        GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
        shield = false;
    }

}

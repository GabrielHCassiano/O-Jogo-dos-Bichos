using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ArqueriaCombat : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private int life;
    private bool shield;
    private bool lose;

    [SerializeField] private SpriteRenderer[] uiArrow;
    [SerializeField] private Sprite trueUIArrow;

    [SerializeField] private GameObject arrowPos;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private int idArrow;
    private bool inArrow;

    [SerializeField] private Sprite trueArrow;
    [SerializeField] private RuntimeAnimatorController trueArrowAnim;

    private Vector2 laterDirection;

    [SerializeField] private GameObject[] scoreUp;
    [SerializeField] private GameObject[] scoreUp_2;


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
            {
                arrows[i].GetComponentInChildren<SpriteRenderer>().sprite = trueArrow;
                uiArrow[i].sprite = trueUIArrow;
            }
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
        ScoreUpdate();
        ArrowAim();
        UIArrow();
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

    public bool Shield
    {
        get { return shield; }
        set { shield = value; }
    }

    public Vector3 LaterDirection
    {
        get { return laterDirection; }
        set { laterDirection = value; }
    }

    public GameObject[] Arrows
    {
        get { return arrows; }
        set { arrows = value; }
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
        for (int i = 2; i >= 0; i--)
        {
            if (Arrows[i] != null)
            {
                idArrow = i;
                break;
            }
        }


        if (inputManager != null && inputManager.squarePressed == true && arrows[idArrow] != null)
        {
            inArrow = true;
            arrowPos.transform.right = new Vector2(laterDirection.x, laterDirection.y);

            arrowPos.SetActive(true);

        }
        if (inputManager != null && inputManager.squarePressed == false && inArrow == true)
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

    public void ScoreUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            if (scoreUp[i].activeSelf == true && scoreUp[i].GetComponent<TextMeshProUGUI>().enabled == false)
            {
                scoreUp[i].GetComponent<TextMeshProUGUI>().enabled = true;
                scoreUp[i].SetActive(false);
            }

        }

        for (int i = 0; i < 5; i++)
        {
            if (scoreUp_2[i].activeSelf == true && scoreUp_2[i].GetComponent<TextMeshProUGUI>().enabled == false)
            {
                scoreUp_2[i].GetComponent<TextMeshProUGUI>().enabled = true;
                scoreUp_2[i].SetActive(false);
            }

        }

    }

    public void ScoreUp_2()
    {
        inputManager.playerData.playerScore += 10;
        for (int i = 0; i < 5; i++)
        {
            if (scoreUp_2[i].activeSelf == false)
            {
                scoreUp_2[i].SetActive(true);
                break;
            }

        }

    }

    public void ScoreUp()
    {
        inputManager.playerData.playerScore += 5;
        for(int i = 0; i < 5; i++) 
        {
            if (scoreUp[i].activeSelf == false)
            {
                scoreUp[i].SetActive(true);
                break;
            }

        }

    }

    public void UIArrow()
    {
        for (int i = 2; i >= 0; i--)
        {
            if (arrows[i] == null)
                uiArrow[i].color = Color.black;
            else
                uiArrow[i].color = Color.white;
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
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        for (int i = 0; i < 10; i++)
        {
            GetComponentInChildren<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 1f);
            yield return new WaitForSeconds(0.1f);
            GetComponentInChildren<SpriteRenderer>().color += new Color(0f, 0f, 0f, 1f);
            yield return new WaitForSeconds(0.1f);

        }
        shield = false;
    }

}

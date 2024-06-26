using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KeyboardSplitter;
using UnityEngine.U2D;

public class Anzol : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private GameObject sprite;
    private Vector3 pos;

    private Pescando pescando;
    private PlayerID player;

    [SerializeField] private Vector2 diretion;

    private int force = 10;
    private float diference = 2;
    private float distance = 1;
    private float time;

    private bool canAttack = true;
    private bool canBack = true;
    private bool inBack;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pescando = GetComponentInParent<Pescando>();
        player = pescando.GetComponent<PlayerID>();
        pos = new Vector3(0, 1f, 0);
        lineRenderer = GetComponentInChildren<LineRenderer>();

        transform.position = player.transform.position + pos;

    }

    // Update is called once per frame
    void Update()
    {
        AnzolLogic();
    }

    public PlayerID Player
    {
        get { return player; }
        set { player = value; }
    }

    public int Force
    {
        get { return force; }
        set { force = value; }
    }

    public bool CanBack
    {
        get { return canBack; }
        set { canBack = value; }
    }

    public bool InBack
    {
        get { return inBack; }
        set { inBack = value; }
    }

    public float Diference
    {
        get { return diference; }
        set { diference = value; }
    }
    public float Distance
    {
        get { return distance; }
        set { distance = value; }
    }

    public float TimeAnzol
    { 
        get { return time; } 
        set {  time = value; } 
    }

    public void AnzolLogic()
    {
        //print(time);

        lineRenderer.SetPosition(0, player.transform.position + pos);
        lineRenderer.SetPosition(1, transform.position);

        time += Time.deltaTime;

        if (canAttack == true)
        {
            player = pescando.GetComponent<PlayerID>();
            diretion = pescando.LaterDirection;
            sprite.transform.up = diretion;
            //transform.parent = null;
            StartCoroutine(AnzolCooldown());
        }

        if (inBack && canBack)
        {
            StopAllCoroutines();
            distance = (distance * diference) - ((distance - time) * diference);
            StartCoroutine(AnzolBack());
        }
        //else
        //  arqueriaCombat = GetComponentInParent<ArqueriaCombat>();

        if (rb.velocity != Vector2.zero)
        {
            sprite.transform.up = new Vector3(rb.velocity.x * -1, rb.velocity.y * -1);
        }
    }

    public IEnumerator AnzolCooldown()
    {
        canAttack = false;
        pescando.GetComponent<TopDownController>().canMove = false;
        pescando.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.velocity = (diretion * force);
        yield return new WaitForSeconds(distance);
        rb.velocity = Vector2.zero;
        rb.velocity = ((diretion * -1) * force/diference);
        yield return new WaitForSeconds(distance * diference);
        Reset();
    }

    public void Reset()
    {
        pescando.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        pescando.GetComponent<TopDownController>().canMove = true;
        pescando.CanAttack = true;
        canAttack = true;
        pescando.ResetInAttack();
        transform.position = player.transform.position + pos;
        gameObject.SetActive(false);
    }

    public IEnumerator AnzolBack()
    {
        canBack = false;
        canAttack = false;
        pescando.GetComponent<TopDownController>().canMove = false;
        pescando.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.velocity = ((diretion * -1) * force / diference);
        yield return new WaitForSeconds((distance));
        Reset();
        canBack = true;
        inBack = false;
    }
}

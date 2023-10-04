using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPlayer : MonoBehaviour
{
    [SerializeField] private int life;
    private int damage;
    private int force;
    private bool lose;


    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        LoseLogic();
    }

    public int lifeValue
    { 
        get { return life; } 
        set { life = value; }
    }

    public int damageValue
    {
        get { return damage; }
        set { damage = value; }
    }

    public int forceValue
    {
        get { return force; }
        set { force = value; }
    }

    public bool loseValue
    { 
        get { return lose; } 
        set {  lose = value; } 
    }

    public void LoseLogic()
    {
        if(life <= 0)
        {
            lose = true;
        }
    }

    public void DamageLogic(int force)
    {
        life -= damage * force;
    }

}

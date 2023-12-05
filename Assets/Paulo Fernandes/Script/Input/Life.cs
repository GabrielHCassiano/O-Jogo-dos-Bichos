using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public int vida;
    public int vidaMax;

    public Image[] heart;
    public Sprite full;
    public Sprite empty;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HelthLogic();
    }

    void HelthLogic()
    {
        //Aumento de vida
        if(vida > vidaMax){
            vida = vidaMax;
        }

        //Morte
        if(vida <= 0){
            gameObject.SetActive(false);
        }

        //Lógica de vida
        
        for (int i = 0; i < heart.Length; i++)
        {
            //Visibilidade de Dano
            if(i < vida){
                heart[i].sprite = full;
            }
            else{
                heart[i].sprite = empty;
            }//<!--Vidibilidade de Dano-->

            //Sistema de vida
            if(i < vidaMax){
                heart[i].enabled = true;
            }
            else{
                heart[i].enabled = false;
            }//<!--Sistema de Vida-->
        }//<!--Lógica de Vida-->
    }
}

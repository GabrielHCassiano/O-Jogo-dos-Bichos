using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esse script é um template para criar em cada input a informação do player, como seu bicho e seu nome. (talvez pontuação?)
public class PlayerData : ScriptableObject
{
    public string playerName;
    public Sprite playerSprite; // mudar pra ser um animator com todas as animações do bicho
    public int playerScore = 0;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esse script � um template para criar em cada input a informa��o do player, como seu bicho e seu nome. (talvez pontua��o?)

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")] // S� para debug
public class PlayerData : ScriptableObject
{
    public string playerName;
    // public Sprite playerSprite; // mudar pra ser um animator com todas as anima��es do bicho
    public int playerScore = 0;
}

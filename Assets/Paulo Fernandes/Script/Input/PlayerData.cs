using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esse script � um template para criar em cada input a informa��o do player, como seu bicho e seu nome. (talvez pontua��o?)

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")] // S� para debug
public class PlayerData : ScriptableObject
{
    public string playerName;
    public Sprite playerSprite;
    public RuntimeAnimatorController animatorController;
    public int playerScore = 0;
    public int playerScoreIndex = 0; // posi��o no placar
}
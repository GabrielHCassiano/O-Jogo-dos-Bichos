using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerID : MonoBehaviour
{
    // Script pequeno que vai ser responsável por ter o ID para conectar com
    // o controller e o InputManager para o playerController usar

    [Header("Debug")]
    public int ID = 0;
    [Space]
    public InputManager inputManager;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirection : MonoBehaviour
{
    [SerializeField] public PlayerMovement moves;
    [SerializeField] public Rigidbody2D rb;
    private Vector2 attackDirection;
    private Vector2 powerDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moves = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        DirectionMove();
    }

    void DirectionMove()
    {
        if(moves != null && moves.moveVector != Vector2.zero){
            powerDirection = new Vector2(moves.moveVector.x, moves.moveVector.y);
            attackDirection = powerDirection;
        }
    }
}

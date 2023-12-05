using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move_Test : MonoBehaviour
{
    [Header("CampoSerialized")]
    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Transform groundCheck;
    //[SerializeField] private LayerMask groundLayer

    [Header("Campo_Movimentação")]
    [SerializeField] private float Horizontal;
    [SerializeField] private float Vertical;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float dashPower = 16f;
    [SerializeField] private bool isRight = true;

    // Start is called once
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Horizontal * speed, Vertical * speed);
        
        if(!isRight && Horizontal > 0f){
            Flip();
        }
        else if(isRight && Horizontal < 0f){
            Flip();
        }
    }

    private void Flip(){
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context){
        Horizontal = context.ReadValue<Vector2>().x;
        Vertical = context.ReadValue<Vector2>().y;
    }

    public void Dash(InputAction.CallbackContext context){
        if(context.performed){
            rb.velocity = new Vector2(rb.velocity.x, dashPower);
        }

        if(context.canceled && rb.velocity.y > 0f){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaInputManager : MonoBehaviour
{
    [SerializeField] private float movSpeed = 1.0f;

    private InputPlayerArena input = null;
    private Vector2 movVec;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        input = new InputPlayerArena();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (GetComponent<PlayerID>().inputArena != null)
            input = GetComponent<PlayerID>().inputArena;
        else
            return;

        PlayerInput();
    }

    private void PlayerInput()
    {
        movVec = input.Player.Movement.ReadValue<Vector2>();

    }


    private void Move()
    {

        rb.MovePosition(rb.position + movVec * (movSpeed * Time.fixedDeltaTime));
       
    }


    private void OnEnable()
    {
        input.Enable();
        
    }


    private void OnMovementPerformed(InputAction.CallbackContext value)
    {

        movVec = value.ReadValue<Vector2>();

    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        movVec = Vector2.zero;
    }

}

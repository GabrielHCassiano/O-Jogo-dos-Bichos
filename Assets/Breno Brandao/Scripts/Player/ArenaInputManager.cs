using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaInputManager : MonoBehaviour
{
    [SerializeField] private float movSpeed = 2.0f;

    private ArenaInputSystem input = null;
    private Vector2 movVec;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        input = new ArenaInputSystem();
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
        {
            print("getting player inputarena");
            //  input = GetComponent<PlayerID>().inputArena;
        }
        else
        {
            print("else getting player inputarena");

            return;
        }

       PlayerInput();
    }

    private void PlayerInput()
    {
        movVec = input.Player.Movement.ReadValue<Vector2>();

    }


    private void Move()
    {
            
       rb.velocity = movVec * movSpeed;
    }


    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {

        movVec = value.ReadValue<Vector2>();

    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        movVec = Vector2.zero;
    }

    public void OnMove(InputAction.CallbackContext ctx) => movVec = ctx.ReadValue<Vector2>();


}

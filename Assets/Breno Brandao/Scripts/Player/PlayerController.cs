using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private InputManager inputManager;
    void Update()
    {

        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;


        
        
        transform.Translate(new Vector2(inputManager.moveDir.x, inputManager.moveDir.y) * speed * Time.deltaTime);

        if (inputManager.squarePressed)
        {
            print("quadrado pressionado");
        }

    }
    //public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
    
    
}
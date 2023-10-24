using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishController : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        Animations();
    }

    //------------------------------Visual-----------------------------//

    void Animations()
    {
        if (animator == null)
            return;

        animator.runtimeAnimatorController = inputManager.playerData.animatorController;
    }
}

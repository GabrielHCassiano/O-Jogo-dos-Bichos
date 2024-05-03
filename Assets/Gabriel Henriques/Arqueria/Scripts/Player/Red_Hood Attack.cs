using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_HoodAttack : MonoBehaviour
{
    private Animator animator;
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        animator = GetComponentInParent<Animator>();

        if (animator == null)
            return;

        animator.SetBool("Attack", inputManager.trianglePressed);
    }
}

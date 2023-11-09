using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PulaCordaController : MonoBehaviour
{
    public int lost_pos = 0;

    bool grounded = true;
    bool lastFrameJumped = true;
    bool canLose = true;

    Rigidbody2D rb;
    InputManager inputManager;
    Animator animator;
    ParticleSystem dustParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dustParticle = GetComponentInChildren<ParticleSystem>();

        rb.gravityScale = 9.8f;
    }

    void Update()
    {
        if (GetComponent<PlayerID>().inputManager != null)
            inputManager = GetComponent<PlayerID>().inputManager;
        else
            return;

        Jump();
    }

    void Jump()
    {
        if (inputManager.anyPressed && grounded && inputManager.anyPressed != lastFrameJumped)
        {
            rb.AddForce(new Vector2(0, 1000));

            grounded = false;
        }

        lastFrameJumped = inputManager.anyPressed;
    }

    void Lose()
    {
        RoomManager.instance.GetComponent<PulaCordaManager>().lossCount++;
        lost_pos = RoomManager.instance.GetComponent<PulaCordaManager>().lossCount;
        if (inputManager != null)
        {
            RoomManager.instance.transform.GetChild(0).Find(inputManager.playerID.ToString()).gameObject.SetActive(false);
        }
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        dustParticle.gameObject.SetActive(false);
        canLose = false;
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !grounded)
        {
            grounded = true;
            PlayDustParticle();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Rope" && canLose)
        {
            Lose();
        }
    }
}

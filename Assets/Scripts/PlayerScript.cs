using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    public GameObject normalBody;
    public GameObject bouncyBody;

    public float moveSpeed;

    public float startTime;
    public float timeLeft;

    private float startX;
    private float startY;

    public float jumpForce;
    public float gravityScale;
    public float fallingGravityScale;

    public TimerScript timerScript;

    public bool reverseGravity;

    private bool finished;
    private Vector2 finishPos;

    public LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = startTime;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        startX = transform.position.x;
        startY = transform.position.y;

        timerScript = GameObject.FindWithTag("Canvas").GetComponent<TimerScript>();

        SetGravity(false);
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                DecayDeath();
            }

            // Horizontal movement
            float dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            // Better feeling jumps
            if (!reverseGravity)
            {
                if (rb.velocity.y >= 0)
                {
                    rb.gravityScale = gravityScale;
                }
                else if (rb.velocity.y < 0)
                {
                    rb.gravityScale = fallingGravityScale;
                }
            }
            else
            {
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravityScale;
                }
                else if (rb.velocity.y >= 0)
                {
                    rb.gravityScale = fallingGravityScale;
                }
            }

            // Jump
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && IsGrounded() && !reverseGravity)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && IsGrounded() && reverseGravity)
            {
                rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
            }

            // Fall off side
            if (transform.position.y < -15 || transform.position.y > 15)
            {
                Reset();
            }
        } 
        else
        {
            // Move player to center of finish platform
            transform.position = Vector3.Lerp(transform.position, finishPos, 5 * Time.deltaTime);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycastHit;
        if (!reverseGravity)
        {
            raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
            
        }
        else
        {
            raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.up, 0.1f, platformLayerMask);
        }
        return raycastHit.collider != null;
    }

    public void DecayDeath()
    {
        Instantiate(normalBody, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
    }

    public void SpikeDeath()
    {
        animator.SetTrigger("Spike");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("SpikeDeathWait", 0.5f);
    }

    private void SpikeDeathWait()
    {
        Instantiate(bouncyBody, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
    }

    public void Portal()
    {
        SetGravity(!reverseGravity);
    }

    private void SetGravity(bool reverse)
    {
        if (!reverse)
        {
            Physics2D.gravity = Vector2.down * 9.81f;
            sr.flipY = false;
            reverseGravity = false;
        } else
        {
            Physics2D.gravity = Vector2.up * 9.81f;
            sr.flipY = true;
            reverseGravity = true;
        }
    }

    public void Finish(Vector2 pos)
    {
        animator.SetTrigger("Finished");
        finished = true;
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0f;
        finishPos = pos;
    }

    private void Reset()
    {
        animator.Play("Player_decay", 0, 0.0f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = new Vector3(startX, startY, 0f);
        timeLeft = startTime;
        rb.velocity = Vector3.zero;
        timerScript.Reset();
    }
}

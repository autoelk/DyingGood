using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

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

    public int gravityCount;

    public LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = startTime;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        startX = transform.position.x;
        startY = transform.position.y;

        timerScript = GameObject.FindWithTag("Canvas").GetComponent<TimerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            decayDeath();
        }

        if (gravityCount == 0 || gravityCount == 2)
        {
            float dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

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
            float dirY = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, dirY * moveSpeed);

            if (rb.velocity.x >= 0)
            {
                rb.gravityScale = gravityScale;
            }
            else if (rb.velocity.x < 0)
            {
                rb.gravityScale = fallingGravityScale;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))&& IsGrounded() && gravityCount == 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && IsGrounded() && gravityCount == 1)
        {
            rb.AddForce(Vector2.left * jumpForce, ForceMode2D.Impulse);
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && IsGrounded() && gravityCount == 2)
        {
            rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && IsGrounded() && gravityCount == 3)
        {
            rb.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
        }


        if (transform.position.y < -15 || transform.position.y > 15 || transform.position.x > 26 || transform.position.x < -26)
        {
            Reset();
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(bc.bounds.center, Vector2.down * (bc.bounds.extents.y + 0.1f), rayColor);
        return raycastHit.collider != null;
    }

    public void decayDeath()
    {
        Instantiate(normalBody, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
    }

    public void spikeDeath()
    {
        animator.SetTrigger("Spike");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(spikeDeathWait());
    }

    public void portalDeath()
    {
        // need animation?
        //down-- > right
        if (gravityCount == 0)
        {
            Physics2D.gravity = Vector2.right;
            gravityCount++;
        }
        // right --> up
        else if (gravityCount == 1)
        {
            Physics2D.gravity = Vector2.up;
            gravityCount++;
        }
        // up --> left
        else if (gravityCount == 2)
        {
            Physics2D.gravity = Vector2.left;
            gravityCount++;
        }
        // left --> down
        else if (gravityCount == 3)
        {
            Physics2D.gravity = Vector2.down;
            gravityCount = 0;
        }
        Reset();
    }

    IEnumerator spikeDeathWait()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(bouncyBody, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
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

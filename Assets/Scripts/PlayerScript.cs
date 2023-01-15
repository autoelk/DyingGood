using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;
    public GameObject platform;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    public float moveSpeed;

    public float startTime;
    public float timeLeft;

    private float startX;
    private float startY;

    public float jumpForce;
    public float gravityScale;
    public float fallingGravityScale;

    public LayerMask platformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = startTime;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        startX = transform.position.x;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Death();
        }

        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }

        if (transform.position.y < -15)
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

    public void Death()
    {
        Instantiate(platform, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
    }

    private void Reset()
    {
        animator.Play("Player_death", 0, 0.0f);
        transform.position = new Vector3(startX, startY, 0f);
        timeLeft = startTime;
        rb.velocity = Vector3.zero;
    }
}

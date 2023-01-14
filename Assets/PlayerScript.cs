using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject platform;
    private Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    public float timeLeft;
    public float startX;
    public float startY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Death();
            timeLeft = 10;
        }

        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (transform.position.y < -6)
        {
            Reset();
        }
    }

    void Death()
    {
        Instantiate(platform, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
        Reset();
    }

    private void Reset()
    {
        transform.position = new Vector3(startX, startY, 0f);
        timeLeft = 10;
        rb.velocity = Vector3.zero;
    }
}

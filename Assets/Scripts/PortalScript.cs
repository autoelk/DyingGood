using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private PlayerScript playerScript;
    private SpriteRenderer sprite;

    private Color normalColor;
    public Color reverseColor;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        normalColor = sprite.color;

        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (sprite.color.Equals(normalColor))
            {
                sprite.color = reverseColor;
            }
            else
            {
                sprite.color = normalColor;
            }
            playerScript.Portal();
        }
    }
}

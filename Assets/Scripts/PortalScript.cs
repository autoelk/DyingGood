using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerScript.portalDeath();
        }
    }
}

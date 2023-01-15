using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    public PlayerScript playerScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hi " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            playerScript.Death();
        }
    }
}

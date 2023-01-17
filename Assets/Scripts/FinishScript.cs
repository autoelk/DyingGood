using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    private PlayerScript playerScript;
    private Animator animator;

    private void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            animator.SetTrigger("Finished");
            playerScript.Finish(transform.position);
            Invoke("LoadNext", 1.4f);
        }
    }

    private void LoadNext()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagCollider : MonoBehaviour
{
    [SerializeField] public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isOut", true);
            Invoke("NextLevel", 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isOut", false);
            Invoke("BackIdle", 1f);
        }
    }

    protected void NextLevel()
    {
        SceneManager.LoadScene("Boss");
    }

    protected void BackIdle()
    {
        animator.SetTrigger("flagIdle");
    }
}

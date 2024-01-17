using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Animator playerAnimator;

    private static PlayerController instance;
    [SerializeField] public static PlayerController Instance { get => instance; }

    private float spikeDamage = 10f;
    private float sawDamage = 20f;
    private float backSpeed = -1f;

    private void Awake()
    {
        this.playerAnimator = transform.GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Spike"))
        {
            HealthBar.Instance.DecreaseHP(this.spikeDamage);
        }

        if (collision.gameObject.CompareTag("Saw"))
        {
            HealthBar.Instance.DecreaseHP(this.sawDamage);
        }

        //this.Backward();
    }

    public void Backward()
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(input * this.backSpeed, 0f, 0f);
        transform.Translate(movement);
    }
}


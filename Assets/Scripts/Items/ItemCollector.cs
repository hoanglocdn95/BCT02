using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] public Animator itemAnimator;
    [SerializeField] protected float bananaRegen = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bananas"))
        {
            itemAnimator.SetTrigger("isCollected");
            HealthBar.Instance.IncreaseHP(this.bananaRegen);
        }
    }
}

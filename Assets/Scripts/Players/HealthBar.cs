using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Rigidbody2D playerRB;
    [SerializeField] public Transform player;
    [SerializeField] Image HP;
    [SerializeField] public float currentHP = 50f;
    [SerializeField] public float dangerThreshHold = 30f;
    public float minHP = 0f;
    public float maxHP = 100f;
    private float restartDelayTime = 1.5f;

    [SerializeField] private static HealthBar instance;
    public static HealthBar Instance { get => instance; }

    private void Awake()
    {
        if (HealthBar.instance == null)
        {
            HealthBar.instance = this;
        }
    }

    private void Update()
    {
        this.HP.fillAmount = this.currentHP / this.maxHP;
    }

    public virtual void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public virtual void IncreaseHP (float amount)
    {
        this.currentHP += amount;
    }

    public virtual void DecreaseHP (float amount)
    {
        if (this.currentHP - amount <= this.minHP)
        {
            this.currentHP = this.minHP;
            this.playerAnimator.SetTrigger("dead");
            this.playerRB.bodyType = RigidbodyType2D.Static;
            Invoke("RestartScene", this.restartDelayTime);
            transform.gameObject.SetActive(false);
        } else
        {
            this.currentHP -= amount;
            playerAnimator.SetTrigger("isHitting");
        }
    }
}

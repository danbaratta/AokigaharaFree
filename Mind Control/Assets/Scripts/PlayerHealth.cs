using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    Animator anim;
    AudioSource playerAudio;

    bool isDead;
    bool damaged;

    public GameObject Sheild;

    void Awake()
    {
        if (gameObject && gameObject.transform.childCount != 0)
            Sheild = gameObject.transform.GetChild(1).gameObject;
    }

    void Start()
    {
        if (!Sheild && gameObject.transform.childCount != 0)
            Sheild = gameObject.transform.GetChild(1).gameObject;
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        currentHealth = startingHealth;
    }


    void Update()
    {
        if (damaged)
        {
            //play "Flinch" animation
        }

        damaged = false;
    }

    /// <summary>
    /// AI send message to this script and update damage taken to player
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        if (Sheild && Sheild.activeSelf)
        {
            Sheild.SendMessage("TakeDamage", amount);
        }
        else
        {
            damaged = true;
            currentHealth -= amount;
            healthSlider.value = currentHealth;
            if (currentHealth <= 0)
            {
                Death();
            }
            if (currentHealth > startingHealth)
                currentHealth = startingHealth;
        }
    }


    void Death()
    {
        GetComponent<MorganStateMachine>().SpawnPlayer();
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

}

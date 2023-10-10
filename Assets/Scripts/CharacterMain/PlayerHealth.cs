using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    // Khai b�o m�u 
    [SerializeField]    float           maxHealth;
    private             float           currentHealth;

    // Khai b�o Animator
    [SerializeField]    Animator        animator;

    // Khai b�o bi�n UI
    [SerializeField]    Slider          playerHealthSlider;
    [SerializeField]    TextMeshProUGUI txtBlood;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerHealthSlider.maxValue  = maxHealth;
        playerHealthSlider.value = currentHealth;
        txtBlood.text = $"{currentHealth} / {maxHealth}";      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth -= 10f;
        }

        playerHealthSlider.value = currentHealth;
        txtBlood.text = $"{currentHealth} / {maxHealth}";
    }

    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;

        // Set animation hurt & death
        if (currentHealth > 0 )
        {
            animator.SetTrigger("Hit");
        }
        if (currentHealth <= 0 )
        {
            animator.SetTrigger("Death");

        }

    }
}

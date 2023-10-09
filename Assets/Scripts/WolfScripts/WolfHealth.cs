using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WolfHealth : MonoBehaviour
{
    // Khai bao
    public float maxHealth;
    public float currentHealth;

    public Slider healthSlider;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue= maxHealth;
        healthSlider.value = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        Debug.Log( "Current Health" +currentHealth);
        animator.SetTrigger("Hurt");

        NextScene();
    }

    public void NextScene()
    {
        if (currentHealth <= 0 )
        {
            animator.SetBool("IsDead", true);
            StartCoroutine(LoadAfterDelay(2f));
        }
    }

    IEnumerator LoadAfterDelay(float delaySecond)
    {
        yield return new WaitForSeconds(delaySecond);
        SceneManager.LoadScene("TotalMap");
    }
}

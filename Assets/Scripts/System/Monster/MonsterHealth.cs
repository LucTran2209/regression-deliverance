using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("CurrentHealth" + currentHealth);

        // Set animation here
    }
}

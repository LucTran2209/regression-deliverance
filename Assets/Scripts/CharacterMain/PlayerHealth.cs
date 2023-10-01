using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]    float maxHealth;
    private             float currentHealth;


    // Khai báo biên UI
    [SerializeField] Slider playerHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerHealthSlider.maxValue  = maxHealth;
        playerHealthSlider.value = currentHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    // Khai báo máu 
    [SerializeField]    float           maxHealth;
    private             float           currentHealth;

    // Khai báo component
    [SerializeField] private Animator       m_animator;
    [SerializeField] private Rigidbody2D    m_rigidbody;
    [SerializeField] private Collider2D     m_collider;

    // Khai báo biên UI
    [SerializeField]    Slider          playerHealthSlider;
    [SerializeField]    TextMeshProUGUI txtBlood;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerHealthSlider.maxValue  = maxHealth;
        playerHealthSlider.value = currentHealth;
        txtBlood.text = $"{currentHealth} / {maxHealth}";

        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();
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
            m_animator.SetTrigger("Hit");
        }
        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            m_animator.SetTrigger("Death");
            m_rigidbody.gravityScale = 0;
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            m_collider.enabled = false;
            StartCoroutine(DestroyAfterSeconds(3f));  // Chuyển Scene kết thúc       
        }

    }
    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);      
    }
}

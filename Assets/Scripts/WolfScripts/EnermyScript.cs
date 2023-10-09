using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyScript : MonoBehaviour
{
    // Enermy blood
    public float maxHealth = 700f;
    public float currenthealth;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxHealth;
        animator = GetComponent<Animator>();
    }



    public void TakeDamage(float takeDamage)
    {
        currenthealth -= takeDamage;

        // set hurt animation
        animator.SetBool("Hurt",true);
        Debug.Log("Current health:" + currenthealth);
    }
}

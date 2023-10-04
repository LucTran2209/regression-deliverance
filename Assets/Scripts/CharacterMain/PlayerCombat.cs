using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackRange;
	[SerializeField] private LayerMask enemyLayeres;
	[SerializeField] private float PlayerDamage = 100;


	private float m_timeSinceAttack = 0f;
	private bool m_isRolling = false;
	private int m_currentAttack = 1;

	// Update is called once per frame
	void Update()
    {
		m_timeSinceAttack += Time.deltaTime;
		//Attack
		if (Input.GetKeyDown(KeyCode.J) && m_timeSinceAttack > 0.25f && !m_isRolling)
		{
			Attack();
		}
	}

	private void Attack()
    {
        animator = GetComponent<Animator>();
		if (GetComponent<SpriteRenderer>().flipX)
		{
			attackPoint.position = new Vector3(transform.position.x - attackRange, transform.position.y+0.8f);
		}
		else
		{
			attackPoint.position = new Vector3(transform.position.x + attackRange, transform.position.y + 0.8f);
		}


		m_currentAttack++;

		// Loop back to one after third attack
		if (m_currentAttack > 3)
			m_currentAttack = 1;

		// Reset Attack combo if time since last attack is too large
		if (m_timeSinceAttack > 1.0f)
			m_currentAttack = 1;

		// Call one of three attack animations "Attack1", "Attack2", "Attack3"
		animator.SetTrigger("Attack" + m_currentAttack);
		
		//Calculate Damage Deal
		float damageDeal = 0;
		switch (m_currentAttack)
		{
			case 1:
				damageDeal = Random.Range(PlayerDamage*0.95f, PlayerDamage); 
				break;
			case 2:
				damageDeal = Random.Range(PlayerDamage * 0.78f, PlayerDamage * 0.82f);
				break;
			case 3:
				damageDeal = Random.Range(PlayerDamage * 1.22f, PlayerDamage * 1.18f);
				break;
			default: break;
		}

		// Detect enemies in range of attack
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayeres);

		// Damage them
		foreach(Collider2D enemy in hitEnemies)
		{
			DealDmg(enemy, damageDeal);
		}

		// Reset timer
		m_timeSinceAttack = 0.0f;
	}

	private void DealDmg(Collider2D enemy ,float damage)
	{
		enemy.GetComponent<Health>().TakeDmg(damage);
		Debug.Log(enemy.GetComponent<Health>().current_health);

	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
		{
			return;
		}
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}

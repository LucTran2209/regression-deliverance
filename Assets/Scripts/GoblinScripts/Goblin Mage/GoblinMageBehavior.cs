using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinMageBehavior : EnemyBehavior
{
	#region SerializeField Variables
	[SerializeField] private float quickDistance; // Minium Distance for quick attack
	[SerializeField] private float quickTimer; // Time cooldown quick attack

	[SerializeField] private float chargeDistance;
	[SerializeField] private float chargeTimer;

	[SerializeField] private float summonHP; // Minium Distance for throw attack
	[SerializeField] private List<GameObject> enemySummon;
	[SerializeField] private int summonNumber;
	[SerializeField] private Transform summonPosition;
	#endregion

	#region Private Variables
	private bool quickCooling;
	private float intQuickTimer;

	private bool chargeCooling;
	private float intChargeTimer;

	private bool isSummon;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		intChargeTimer = chargeTimer;
		intQuickTimer = quickTimer;

		foreach (GameObject enemy in enemySummon)
		{
			enemy.GetComponent<EnemyBehavior>().leftLimit = leftLimit;
			enemy.GetComponent <EnemyBehavior>().rightLimit = rightLimit;
		}
	}

	// Update is called once per frame
	protected override void Update()
    {
        base.Update();

		if (quickCooling)
		{
			Cooldown(1);
		}

		if (chargeCooling)
		{
			Cooldown(2);
		}
    }

	#region Enemy Action
	protected override void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);
		float hp = GetComponent<Health>().current_health / GetComponent<Health>().max_health;
		if (!isSummon && !cheackAnimationAttack() && hp <= summonHP)
		{
			if(attackDistance >= distance)
			{
				Attack();
			}
			Summon();
		}

		if (distance > attackDistance && !cheackAnimationAttack() && !cooling)
		{
			Flip();
			int skillNum = Random.Range(1, 3);
			switch (skillNum)
			{
				default:
					break;
				case 1:
					if (distance <= quickDistance && !quickCooling)
					{
						QuickAttack();
						return;
					}
					break;
				case 2:
					if (distance > attackDistance * 2 && distance <= chargeDistance && !chargeCooling)
					{
						ChargeAttack();
						return;
					}
					break;
			}
		}
		if (distance > attackDistance + 2f && !cheackAnimationAttack())
		{
			Flip();
			StopAttack();
		}

		else if (attackDistance >= distance && cooling == false && !cheackAnimationAttack())
		{
			Attack();
		}
		else if (!cheackAnimationAttack())
		{
			Stand();
		}

	}

	private void ChargeAttack()
	{
		AttackMode();
		animator.SetTrigger("Charge");
	}

	private void QuickAttack()
	{
		AttackMode();
		animator.SetTrigger("Quick");
	}

	private void Summon()
	{
		AttackMode();
		animator.SetTrigger("Summon");
	}

	public void SummonEnemy()
	{
		animator.SetBool("Summoning", true);
		StartCoroutine(SpawnEnemy());
	}

	private IEnumerator SpawnEnemy()
	{
		for (int i = 0; i < summonNumber; i++) {
			int index = Random.Range(0, enemySummon.Count);
			Instantiate(enemySummon[index], summonPosition.position, Quaternion.identity);
			yield return new WaitForSeconds(0.5f);
		}
		animator.SetBool("Summoning", false);
		TriggerCooling();
	}
	#endregion


	#region Cooldown
	protected override void Cooldown(int skill)
	{
		switch (skill)
		{
			case 1:
				quickTimer -= Time.deltaTime;
				if(quickTimer <= 0 && quickCooling)
				{
					quickCooling = false;
					quickTimer = 0;
				}
				break;
			case 2:
				chargeTimer -= Time.deltaTime;
				if (chargeTimer <= 0 && chargeCooling)
				{
					chargeCooling = false;
					chargeTimer = 0;
				}
				break;
			default: break;
		}
	}
	public override void TriggerSkill(int skill)
	{
		switch (skill)
		{
			case 1:
				quickCooling = true;
				quickTimer = intQuickTimer;
				break;
			case 2:
				chargeCooling = true;
				chargeTimer = intChargeTimer;
				break;
			case 3:
				isSummon = true;
				break;
			default: break;
		}
	}
	#endregion


	protected override bool cheackAnimationAttack()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Mage_Charge Spell")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Mage_Quick Spell")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Mage_Summon")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Mage_Summoning")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Mage_Attack");
	}
}

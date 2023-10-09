using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSpearBehavior : EnemyBehavior
{
	#region Public Variables
	[SerializeField] protected float rangeDistance; // Minium Distance for range attack
	[SerializeField] protected float rangeTimer;
	[SerializeField] protected Transform rangePosition;
	[SerializeField] GameObject[] spears;
	#endregion

	#region Private Variables
	private bool rangeCooling;
	private float intRangeTimer;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		intRangeTimer = rangeTimer;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (rangeCooling)
		{
			Cooldown(1);
		}
	}

	#region Enemy Action

	protected override void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);
		
		if(rangeDistance >= distance && attackDistance < distance  && !cooling && !cheackAnimationAttack())
		{
			int caseNumber = Random.Range(0, 2);
			switch (caseNumber)
			{
				case 1:
					if (!rangeCooling)
					{
						AttackRange();
						return;
					}
					break;
				default: break;
			}
			
		}
		
		if (distance > 4.5f && !cheackAnimationAttack())
		{
			Flip();
			StopAttack();
		}
		else if (attackDistance >= distance && !cooling)
		{
			Attack();
		}else if(distance <= 4.5f && !cheackAnimationAttack())
		{
			Stand();
		}
	}
	private void AttackRange()
	{
		AttackMode();
		animator.SetTrigger("Range");
	}

	public void thownSpear()
	{
		int index = FindSpear();
		spears[index].transform.position = rangePosition.position;
		spears[index].GetComponent<Projectiles>().SetDirection(target);
		spears[index].GetComponent<Projectiles>().Shoot();
	}

	private int FindSpear()
	{
		for (int i = 0; i <spears.Length; i++)
		{
			if (!spears[i].activeInHierarchy)
			{
				return i;
			}
		}
		return 0;
	}

	#endregion

	#region Cooldown
	protected override void Cooldown(int skill)
	{
		switch (skill)
		{
			default:
				break;
			case 1:
				rangeTimer -= Time.deltaTime;
				if(rangeTimer <= 0 && rangeCooling)
				{
					rangeCooling = false;
					rangeTimer = 0;
				}
				break;
		}
	}
	

	public override void TriggerSkill(int skill)
	{
		switch (skill)
		{
			default:
				break;
			case 1:
				rangeCooling = true;
				rangeTimer = intRangeTimer;
				break;
		}
	}
	#endregion

	protected override bool cheackAnimationAttack()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Spear_Attack Range") 
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Spear_Attack Meele");
	}
}

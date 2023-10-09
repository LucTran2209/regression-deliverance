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
		GameObject shootSpear = Instantiate(AttackMethod[1], rangePosition.position, Quaternion.identity);
		shootSpear.transform.position = rangePosition.position;
		shootSpear.GetComponent<Projectiles>().SetDirection(target);
		shootSpear.GetComponent<Projectiles>().Shoot();
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

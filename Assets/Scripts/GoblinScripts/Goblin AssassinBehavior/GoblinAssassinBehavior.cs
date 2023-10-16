using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAssassinBehavior : EnemyBehavior
{

	#region SerializeField Variables
	[SerializeField] private float quickDistance; // Minium Distance for quick attack
	[SerializeField] private float quickTimer; // Time cooldown quick attack
	[SerializeField] private float runSpeed;

	[SerializeField] private float thrownDistance; // Minium Distance for throw attack
	[SerializeField] private float thrownTimer; //Time cooldown throw attack
	#endregion

	#region Private Variables
	private bool quickCooling;
	private float intQuickTimer;

	private bool thrownCooling;
	private float intThrownTimer;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		intQuickTimer = quickTimer;
		intThrownTimer = thrownTimer;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (quickCooling)
		{
			Cooldown(1);
		}

		if (thrownCooling)
		{
			Cooldown(2);
		}
	}

	#region Enemy Action
	protected override void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Assassin_Run"))
		{
			if(distance <= attackDistance || Mathf.Abs(target.position.y - transform.position.y) >= 4f)
			{
				QuickAttack();
			}
			Run();
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
					if(distance > attackDistance * 2  && distance <= quickDistance && !quickCooling)
					{
						QuickRun();
						return;
					}
					break;
				case 2:
					if (distance <= thrownDistance && !thrownCooling)
					{
						ThrownBoom();
						return;
					}
					break;
			}

		}
		
		if (distance > attackDistance && !cheackAnimationAttack())
		{
			Flip();
			StopAttack();
		}

		else if (attackDistance >= distance && cooling == false && !cheackAnimationAttack())
		{
			Attack();
		}
		else if (attackDistance >= distance && !cheackAnimationAttack() && cooling == true)
		{
			Stand();
		}
	}
	protected override void Move()
	{
		Flip();
		animator.SetBool("Walk", true);
		if (!cheackAnimationAttack())
		{
			Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
			Vector2 direction = (targetPosition - rigi.position).normalized;
			rigi.velocity = new Vector2(direction.x * moveSpeed, rigi.velocity.y);
		}
	}
	private void ThrownBoom()
	{
		AttackMode();
		animator.SetTrigger("Throw");
	}
	private void QuickRun()
	{
		AttackMode();
		animator.SetBool("Run", true);
	}
	private void Run()
	{
		Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
		Vector2 direction = (targetPosition - rigi.position).normalized;
		rigi.velocity = new Vector2(direction.x * moveSpeed * runSpeed, rigi.velocity.y);
	}
	private void QuickAttack()
	{	
		animator.SetBool("Run", false);
		animator.SetTrigger("Quick");
	}

	#endregion

	#region Cooldown
	protected override void Cooldown(int skill)
	{
		switch (skill)
		{
			case 1:
				quickTimer -= Time.deltaTime;
				if (quickTimer <= 0 && quickCooling)
				{
					quickCooling = false;
					quickTimer = 0;
				}
				break;
			case 2:
				thrownTimer -= Time.deltaTime;
				if(thrownTimer <= 0 && thrownCooling)
				{
					thrownCooling = false;
					thrownTimer = 0; 
				}
				break;
			default:
				break;

		}
	}
	public override void TriggerSkill(int skill)
	{
		switch (skill)
		{
			case 1:
				quickCooling = true ; quickTimer = intQuickTimer; break;
			case 2:
				thrownCooling = true ; thrownTimer = intThrownTimer; break;
			default : break;
		}
	}
	#endregion

	protected override bool cheackAnimationAttack()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Assassin_Attack") 
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Assassin_Quick Attack") 
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Assassin_Throw") 
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin Assassin_Run");
	}
}

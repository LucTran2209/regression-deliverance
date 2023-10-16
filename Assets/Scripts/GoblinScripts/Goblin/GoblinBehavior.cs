using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GoblinBehavior : EnemyBehavior
{

	#region Public Variables

	#endregion

	#region Private Variables

	#endregion

	protected override void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);
		if (distance > attackDistance && !cheackAnimationAttack())
		{
			Flip();
			StopAttack();
		}else if(attackDistance >= distance && cooling == false)
		{
			Attack();
		}
		else if (attackDistance >= distance && !cheackAnimationAttack())
		{
			Stand();
		}
	}
	protected override bool cheackAnimationAttack()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack");
	}
}


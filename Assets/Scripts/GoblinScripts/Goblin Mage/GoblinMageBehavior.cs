using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GoblinMageBehavior : EnemyBehavior
{
	#region SerializeField Variables
	[SerializeField] private float quickDistance; // Minium Distance for quick attack
	[SerializeField] private float quickTimer; // Time cooldown quick attack
	[SerializeField] private List<Transform> quickPositions; // Time cooldown quick attack


	[SerializeField] private float chargeDistance;
	[SerializeField] private float chargeTimer;
	[SerializeField] private Transform chargePosition;

	[SerializeField] private float summonHP; // Minium Distance for throw attack
	[SerializeField] private List<GameObject> enemySummon;
	[SerializeField] private int summonNumber;
	[SerializeField] private Transform summonPosition;
	#endregion

	#region Private Variables
	private bool quickCooling;
	private float intQuickTimer;
	private List<GameObject> fireBalls;

	private bool chargeCooling;
	private float intChargeTimer;
	private GameObject thunderBird;

	private bool isSummon;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		intChargeTimer = chargeTimer;
		intQuickTimer = quickTimer;
	}

	private void SetLimits(GameObject enemy)
	{
		enemy.GetComponent<EnemyBehavior>().leftLimit = leftLimit;
		enemy.GetComponent<EnemyBehavior>().rightLimit = rightLimit;
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
		float hp = GetComponent<AttributeManager>().Health / GetComponent<AttributeManager>().GetMaxHealth();
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

	public void CreateThunderBird()
	{
        thunderBird = null;
        thunderBird = Instantiate(AttackMethod[2], chargePosition.position, Quaternion.identity);
        thunderBird.GetComponent<Projectiles>().SetDirection(target);
    }

	public void shootThunderBird()
	{
		try
		{
			thunderBird.GetComponent<Projectiles>().Shoot();

		}
		catch (MissingReferenceException e)
		{

		}
	}

	private void QuickAttack()
	{
		AttackMode();
		animator.SetTrigger("Quick");
/*		StartCoroutine(CreateFireBall(0.25f));
*/	}

	public IEnumerator CreateFireBall(float time)
	{
		fireBalls = new List<GameObject>(); ;
		foreach (Transform quickPosition in quickPositions)
		{
			GameObject fireBall = Instantiate(AttackMethod[1], quickPosition.position, Quaternion.identity);
			fireBall.GetComponent<Projectiles>().SetDirection(target);
			fireBalls.Add(fireBall);
			yield return new WaitForSeconds(time);
		}
	}

	private IEnumerator WaitShoot(float time)
	{
		foreach (GameObject fireBall in fireBalls)
		{
            try
            {
                fireBall.GetComponent<Projectiles>().Shoot();
			}
			catch (MissingReferenceException e)
			{

			}
        yield return new WaitForSeconds(time);
		}

	}

	public void ShootFireBall()
	{

				StartCoroutine(WaitShoot(0.25f));

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
			GameObject enemy = Instantiate(enemySummon[index], summonPosition.position, Quaternion.identity);
			SetLimits(enemy);
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

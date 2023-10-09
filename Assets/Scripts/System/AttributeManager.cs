using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    #region Status
    public float Attack = 100f;
    public float Ammor = 0f;
    public float Health = 500f;
	#endregion

	#region Private Variables
    private float maxHealth;
    private float damageRessitance = 0f;
    private Animator animator;

	private bool isResistance; // Kháng gián đoạn
	private float resistanceDuration = 0f;

	private bool isHit;
	private float hitDuration = 0f;
	private int hitCount = 0;
	private int hitMax = 3;
	#endregion

	// Start is called before the first frame update
	private void Awake()
	{
		maxHealth = Health;
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
		if (isHit)
		{
			hitDuration += Time.deltaTime;
		}

		if (hitDuration >= 0.75f && isHit)
		{
			hitDuration = 0f;
			isHit = false;
		}

		if (resistanceDuration >= 0 && isResistance)
		{
			ResistanceCooldown();
		}
	}

	public void TakeDmg(float damage)
	{
		Health = Mathf.Clamp(Health - (damage - Ammor / 100 * damage) * (100 - damageRessitance) / 100, 0, maxHealth);

		if (Health <= 0)
		{
			//Death
			animator.SetTrigger("Death");
			Destroy(gameObject, 3f);
		}

		if (Health > 0 && !isResistance)
		{
			if (isHit && hitDuration < 2f)
			{
				IncreaseHit();
			}
			//Hit
			animator.SetTrigger("Hit");

			//reset Hit duration
			Hit();
		}
	}

/*	public void DealDamge(GameObject target)
	{
		var atm = target.GetComponent<AttributeManager>();
		if(atm != null)
		{
			atm.TakeDmg(Attack);
		}
	}*/


	private void Hit()
	{
		isHit = true;
		hitDuration = 0f;
	}

	private void IncreaseHit()
	{
		hitCount++;
		if (hitCount >= hitMax)
		{
			hitCount = 0;
			AcitveResistanceTime(2f);
		}
	}

	private void ResistanceCooldown()
	{
		resistanceDuration -= Time.deltaTime;
		if (resistanceDuration <= 0)
		{
			isResistance = false;
			resistanceDuration = 0f;
		}
	}

	public void AcitveResistanceTime(float time)
	{
		isResistance = true;
		resistanceDuration = time;
	}

	public void ActitveDamageResistance(float dmgRes)
	{
		isResistance = true;
		damageRessitance = dmgRes;
	}

	public void AcitveResistance()
	{
		isResistance = true;
	}

	public void UnActiveResistance()
	{
		isResistance = false;
		damageRessitance = 0f;
	}
}

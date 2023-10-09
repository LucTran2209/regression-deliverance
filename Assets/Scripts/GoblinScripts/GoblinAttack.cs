using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
	#region Public Variables
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float atkPoint;
	[SerializeField] private float atkSpeed;
	[SerializeField] private float atkScale;

	#endregion

	#region Private Variables
	private float dmg;
	#endregion

	// Update is called once per frame
	void Update()
	{
		if(atkScale > 0)
		{
			dmg =  atkPoint * atkScale;
		}
	}

	// Deal Damge
	private void OnCollisionEnter2D(Collision2D trig)
	{
        Attack(trig.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D trig)
	{
		Attack(trig.gameObject);
	}

	private void Attack(GameObject trig)
    {
        if (trig.tag == "Player")
		{
			atkSpeed = body.velocity.magnitude;
			trig.GetComponent<PlayerHealth>().TakeDmg(dmg + atkSpeed * 10);
		}
	}
}

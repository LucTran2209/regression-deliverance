using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDeal : MonoBehaviour
{
    // Start is called before the first frame update
    public float AttackBase;
    public float AttackScale;


	private void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.tag == "Player")
		{
			trig.GetComponent<PlayerHealth>().TakeDmg(AttackBase*AttackScale);
		}
	}

}

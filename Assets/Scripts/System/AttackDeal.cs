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

        if(trig.tag == "Monster")
        {
            Physics2D.IgnoreCollision(trig, GetComponent<Collider2D>());
        }
	}

    private void OnCollisionEnter2D(Collision2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            trig.gameObject.GetComponent<PlayerHealth>().TakeDmg(AttackBase * AttackScale);
        }

        if (trig.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(trig.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float max_health = 500f;
    public float current_health { get; private set; }
    
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private Collider2D m_collider;
    private float dmgTake = 0f;
    private float dmgScale = 1f;

    private bool isResistance; // Kháng gián đoạn
    private float resistanceDuration = 0f;



	private bool isHit;
    private float hitDuration = 0f;
    private int hitCount = 0;
    private int hitMax = 3;


	// Start is called before the first frame update
	void Awake()
    {
        current_health = max_health;
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
        if (isHit)
        {
            hitDuration += Time.deltaTime;
        }

        if(hitDuration >= 0.75f && isHit)
        {
            hitDuration = 0f;
            isHit = false;
        } 

        if(resistanceDuration >= 0 && isResistance)
        {
            ResistanceCooldown();
        }
    }

	public void TakeDmg(float dmg)
    {
        dmgTake = dmgScale * dmg;
        if(dmgTake > 0)
        {
            DecreaseHP(dmgTake);
        }
    }

    private void DecreaseHP(float dmg)
    {
        current_health = Mathf.Clamp(current_health - dmg, 0, max_health);
            if (current_health <= 0) {
			    //Death
			    m_animator.SetTrigger("Death");
                m_rigidbody.gravityScale = 0;
                m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
                m_collider.enabled = false;
                StartCoroutine(DestroyAfterSeconds(2f));
		    }

            if(current_health > 0 && !isResistance)
            {
			    if (isHit && hitDuration < 2f)
			    {
                IncreaseHit();
			    }
			    //Hit
			    m_animator.SetTrigger("Hit");

                //reset Hit duration
                Hit();
			}

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



    private void Hit()
    {
        isHit = true;
        hitDuration = 0f;
    }

	public void AcitveResistanceTime(float time)
	{
		isResistance = true;
		resistanceDuration = time;
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

	public void AcitveResistance()
    {
        isResistance = true;
    }

	public void ActitveDamageResistance(float dmgRes)
	{
		isResistance = true;
        dmgScale = dmgRes;
	}

	public void UnActiveResistance()
    {
        isResistance = false;
        dmgScale = 1f;
    }

	IEnumerator DestroyAfterSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}

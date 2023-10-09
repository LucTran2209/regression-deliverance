using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float lifetime;
    
    private Vector3 directionShoot;
    private bool hit;
    private float intLifetime;
    private bool shoot;
    private Animator animator;
    private Collider2D hitBox;



    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        hitBox = GetComponent<Collider2D>();
        hit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            return;
        }
        else
        {
            lifetime -= Time.deltaTime;
        }

        if(!hit && !shoot)
        {

        }

        if(shoot)
        {
            float movermentSpeed = speed * Time.deltaTime;
            transform.position += directionShoot * movermentSpeed;
        }

        if(lifetime < 0)
            {
                DeActive();
            }
    }

    public void Shoot()
    {
        shoot = true;
		directionShoot = (target.position - transform.position).normalized;
    }

    public void SetDirection(Transform _target)
    {
        lifetime = 10f;
        gameObject.SetActive(true);
        hit = false;
        hitBox.enabled = true;
        target = _target;
        
    }

	private void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.tag == "Player")
        {
            hit = true;
            hitBox.enabled = false;
            animator.SetTrigger("Hit");
        }
	}

    private void DeActive()
    {
        gameObject.SetActive (false);
    }
}

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
    private Rigidbody2D rigi;



    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        hitBox = GetComponent<Collider2D>();
        rigi = GetComponent<Rigidbody2D>();
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
            rigi.velocity = directionShoot * speed;
        }

        if (lifetime < 0)
            {
                DeActive();
            }
    }

    public void Shoot()
    {
        shoot = true;
		directionShoot = (target.position - transform.position).normalized;

        float rot = Mathf.Atan2(-directionShoot.y, -directionShoot.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, rot - 180);
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
            rigi.velocity = Vector2.zero;
            hit = true;
            hitBox.enabled = false;
            animator.SetTrigger("Hit");
        }

        if (trig.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
			Physics2D.IgnoreCollision(hitBox, trig.GetComponent<Collider2D>());

		}
	}
	private void DeActive()
    {
        Destroy(gameObject);
    }
}
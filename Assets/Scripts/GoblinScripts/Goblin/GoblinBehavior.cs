using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GoblinBehavior : MonoBehaviour
{

	#region Public Variables
	public Transform scanArea;
	public LayerMask scanMark;
	public float scanWidth;	
	public float scanHeight;	
	public float attackDistance; // Minium Distance for attack
	public float moveSpeed;
	public float timer; // Time cooldown bw attack
	public Transform leftLimit;
	public Transform rightLimit;
	public Transform rayCastGround; //Raycast to checkGround;
	public LayerMask groundMark;
	public float rayCastLengt;

	#endregion

	#region Private Variables
	private Collider2D hit;
	private Transform target;
	private Animator animator;
	private Rigidbody2D rigi;
	private float distance; // The distance bw enemy and player
	private bool attackMode;
	private bool inRange; // Check player in range
	private bool cooling; // Check cooling after attack;
	private float intTimer;
	private SensorPlayer groundSensor;
	private RaycastHit2D hitGround;
	private bool isGround;
	#endregion

	private void Awake()
	{
		SelectTarget();
		intTimer  = timer;
		animator = GetComponent<Animator>();
		rigi = GetComponent<Rigidbody2D>();
		groundSensor = transform.Find("GroundSensor").GetComponent<SensorPlayer>();
	}

	void OnDrawGizmos()
	{
		Vector2 boxSize = new Vector2(scanWidth, scanHeight);

		Vector2 boxPosition = scanArea.position;

		// Vẽ hộp bằng Gizmos
		Gizmos.matrix = Matrix4x4.TRS(boxPosition, transform.rotation, boxSize);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	// Update is called once per frame
	void Update()
    {
		//Check if character just landed on the ground
		if (!isGround && groundSensor.State())
		{
			isGround = true;
			animator.SetBool("Grounded", isGround);
		}

		//Check if character just started falling
		if (isGround && !groundSensor.State())
		{
			isGround = false;
			animator.SetBool("Grounded", isGround);
		}

		//Set AirSpeed in animator when jumping
		animator.SetFloat("Jump", rigi.velocity.y);

		if (!attackMode && !animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack"))
		{
			Move();
		}

		if (!Inside() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack"))
		{
			SelectTarget();
		}

		if (inRange)
		{
			hit = Physics2D.OverlapBox(scanArea.position, new Vector2(scanWidth,scanHeight), 0f, scanMark);
/*			
*/		}

		hitGround = Physics2D.Raycast(rayCastGround.position, Vector2.down, rayCastLengt, groundMark);
		RayCastDeBugger();

		//When Player is detected
		if(hit == null && !Inside()) 
		{
			inRange = false;
		}
		
		if(hit != null)
		{
			EnemyLogic();
		}

		if(!inRange)
		{
			StopAttack();
		}
    }

	private void RayCastDeBugger()
	{
		Debug.DrawRay(rayCastGround.position, Vector2.down * rayCastLengt, Color.red);
	}

	private void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);
		if (distance > attackDistance && !animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack"))
		{
			Flip();
			StopAttack();
		}else if(attackDistance >= distance && cooling == false)
		{
			Attack();
		}

		if (cooling)
		{
			Cooldown();
			animator.SetBool("Attack", false);
		}
	}

	private void Move()
	{
		if (hitGround.collider == null)
		{
			rigi.velocity = new Vector2(0, rigi.velocity.y);
			animator.SetBool("Run", false);
			animator.SetBool("Walk", false);
			return;
		}

		if(target.tag == "Player")
		{
			animator.SetBool("Run", true);
			animator.SetBool("Walk", false);
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack"))
			{
				/*Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
				transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * 2f * Time.deltaTime);*/

				Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
				Vector2 direction = (targetPosition - rigi.position).normalized;
				rigi.velocity = new Vector2(direction.x * moveSpeed * 2.5f, rigi.velocity.y);
			}
		}
		else
		{
			animator.SetBool("Walk", true);
			animator.SetBool("Run", false);
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack"))
			{
				/*				Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
								transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed*Time.deltaTime);*/

				Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
				Vector2 direction = (targetPosition - rigi.position).normalized;
				rigi.velocity = new Vector2(direction.x * moveSpeed, rigi.velocity.y);
			}
		}
	}
	private void Attack()
	{
		timer = intTimer; // Reset Timer when Player enter Attack Range
		attackMode = true; //To check if Enemy can still attack or not
		rigi.velocity = Vector2.zero;
		animator.SetBool("Attack", true);
		animator.SetBool("Walk", false);
		animator.SetBool("Run", false) ;
	}

	private void Cooldown()
	{
		timer -= Time.deltaTime;
		if(timer <= 0 && cooling && attackMode)
		{
			cooling = false;
			timer = intTimer;
		}
	}

	private void StopAttack()
	{
		cooling = false;
		attackMode = false;
		animator.SetBool("Attack", false);
	}



	private void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.gameObject.tag == "Player") { 
			target = trig.transform;
			inRange = true;
			Flip();
		}
	}

	public void TriggerCooling()
	{
		cooling = true;
	}

	private bool Inside()
	{
		return leftLimit.position.x < transform.position.x && transform.position.x < rightLimit.position.x;
	}

	private void SelectTarget()
	{
		float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
		float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

		if(distanceToLeft > distanceToRight && distanceToLeft > 0) 
		{
			target = leftLimit;
		}
		else
		{
			target = rightLimit;
		}
		Flip();
	}

	private void Flip()
	{
		Vector3 rotation = transform.eulerAngles;
		if(transform.position.x > target.position.x)
		{
			rotation.y = 180f;
		}
		else
		{
			rotation.y = 0f;
		}

		transform.eulerAngles = rotation;
	}
}

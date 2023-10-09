using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{

	#region Public Variables
	[SerializeField] protected Transform scanArea;
	[SerializeField] protected LayerMask scanMark;
	[SerializeField] protected float scanWidth;
	[SerializeField] protected float scanHeight;
	[SerializeField] protected float attackDistance; // Minium Distance for attack
	[SerializeField] protected float moveSpeed;
	[SerializeField] protected float timer; // Time cooldown bw attack
	[SerializeField] public Transform leftLimit;
	[SerializeField] public Transform rightLimit;
	[SerializeField] protected Transform rayCastGround; //Raycast to checkGround;
	[SerializeField] protected LayerMask groundMark;
	[SerializeField] protected float rayCastLengt;
	[SerializeField] protected GameObject[] AttackMethod;
	#endregion

	#region Protected Variables
	protected Collider2D hit;
	protected Transform target;
	protected Animator animator;
	protected Rigidbody2D rigi;
	protected float distance; // The distance bw enemy and player
	protected bool attackMode;
	protected bool inRange; // Check player in range
	protected bool cooling; // Check cooling after attack;
	protected float intTimer;
	protected SensorPlayer groundSensor;
	protected RaycastHit2D hitGround;
	protected bool isGround;
	protected Collider2D body;
	private float AttackBase;
	#endregion

	protected virtual void Awake()
	{
/*		SelectTarget();
*/		intTimer = timer;
		animator = GetComponent<Animator>();
		rigi = GetComponent<Rigidbody2D>();
		groundSensor = transform.Find("GroundSensor").GetComponent<SensorPlayer>();
		body = GetComponent<Collider2D>();
		AttackBase = GetComponent<AttributeManager>().Attack;
		foreach(GameObject atk in AttackMethod)
		{
			atk.GetComponent<AttackDeal>().AttackBase = AttackBase;
		}
	}

	private void Start()
	{
		if (leftLimit != null && rightLimit != null) SelectTarget();
	}

	protected virtual void Update()
    {
		//Check on Ground
		CheckGround();

		//Set animation when jumping
		SetJump();

		if (!attackMode && !cheackAnimationAttack())
		{
			//Set move 
			Move();
		}
		
/*		//Check path can move?
		RayCastDeBugger();*/

		if (!checkPath())
		{
			//Set stand in place
			Stand();
		}

		if (!Inside() && !inRange && !cheackAnimationAttack())
		{
			//Change target when player out range
			SelectTarget();
		}

		if (inRange)
		{
			hit = Physics2D.OverlapBox(scanArea.position, new Vector2(scanWidth, scanHeight), 0f, scanMark);
		}

		//Check player out range
		if (hit == null && !Inside())
		{
			inRange = false;
		}

		//When Player is detected
		if (hit != null)
		{
			//Moveset for enemy
			EnemyLogic();
		}

		//Stop attack when player out range
		if (!inRange)
		{
			StopAttack();
		}

		// Cooldown bw attack
		if (cooling)
		{
			Cooldown();
		}
	}

	#region Enemy Action
	protected void Stand()
	{
		rigi.velocity = new Vector2(0, rigi.velocity.y);
		animator.SetBool("Walk", false);
		animator.SetBool("Run", false);
		Flip();
	}
	protected void StopAttack()
	{
		attackMode = false;
		animator.SetBool("Attack", false);
	}
	protected virtual void Move()
	{
		if (target.tag == "Player")
		{
			animator.SetBool("Run", true);
			animator.SetBool("Walk", false);
			if (!cheackAnimationAttack())
			{
				Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
				Vector2 direction = (targetPosition - rigi.position).normalized;
				rigi.velocity = new Vector2(direction.x * moveSpeed * 2.5f, rigi.velocity.y);
			}
		}
		else
		{
			animator.SetBool("Walk", true);
			animator.SetBool("Run", false);
			if (!cheackAnimationAttack())
			{
				Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
				Vector2 direction = (targetPosition - rigi.position).normalized;
				rigi.velocity = new Vector2(direction.x * moveSpeed, rigi.velocity.y);
			}
		}
	}
	protected void Attack()
	{
		attackMode = true; //To check if Enemy can still attack or not
		Stand();
		animator.SetBool("Attack", true);
	}
	private void SetJump()
	{
		animator.SetFloat("Jump", rigi.velocity.y);
	}
	protected void Flip()
	{
		try
		{
			Vector3 rotation = transform.eulerAngles;
			if (transform.position.x > target.position.x)
			{
				rotation.y = 180f;
			}
			else
			{
				rotation.y = 0f;
			}
			//Flip enemy to f2f player
			transform.eulerAngles = rotation;
		}
		catch (Exception e)
		{
			SelectTarget();
		}
		
	}
	protected abstract void EnemyLogic();
	protected void AttackMode()
	{
		attackMode = true;
		Stand() ;
		animator.SetBool("Attack", false);
	}
	#endregion

	#region Check conditions
	private bool Inside()
	{
		//Check moving in range area
		return leftLimit.position.x < transform.position.x && transform.position.x < rightLimit.position.x;
	}
	public void SelectTarget()
	{
		float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
		float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

		if (distanceToLeft > distanceToRight && distanceToLeft > 0)
		{
			target = leftLimit;
		}
		else
		{
			target = rightLimit;
		}
		Flip();
	}
	private void CheckGround()
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
	}
	protected abstract bool cheackAnimationAttack();
	private void OnTriggerEnter2D(Collider2D trig)
	{
		//Set target to Player when ỉnage
		if (trig.gameObject.tag == "Player")
		{
			target = trig.transform;
			inRange = true;
			Flip();
		}
	}
	private void OnTriggerStay2D(Collider2D trig)
	{
		//Set enemies to pass through each other
		if (trig.tag == "Monster")
		{
			Physics2D.IgnoreCollision(body, trig.GetComponent<Collider2D>());
		}
	}
	protected bool checkPath()
	{
		hitGround = hitGround = Physics2D.Raycast(rayCastGround.position, Vector2.down, rayCastLengt, groundMark);
		return hitGround.collider != null;
	}
	#endregion

	/*#region Debug tool
	private void RayCastDeBugger()
	{
		Debug.DrawRay(rayCastGround.position, Vector2.down * rayCastLengt, Color.red);
	}
	private void OnDrawGizmos()
	{
		Vector2 boxSize = new Vector2(scanWidth, scanHeight);

		Vector2 boxPosition = scanArea.position;

		// Vẽ hộp bằng Gizmos
		Gizmos.matrix = Matrix4x4.TRS(boxPosition, transform.rotation, boxSize);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
	#endregion*/

	#region Cooldown
	private void Cooldown()
	{
		timer -= Time.deltaTime;
		if (timer <= 0 && cooling)
		{
			cooling = false;
			timer = 0;
		}
		animator.SetBool("Attack", false);
	}
	protected virtual void Cooldown(int skill) { }
	public void TriggerCooling()
	{
		cooling = true;
		timer = intTimer;
	}
	public virtual void TriggerSkill(int skill) { }
	#endregion
}

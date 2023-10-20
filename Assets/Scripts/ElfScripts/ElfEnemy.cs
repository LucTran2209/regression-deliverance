using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ElfEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    #region Public Variables
    public float attackDistance;
    public float moveSpped;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public bool inRange;
    [HideInInspector] public Transform target;
    public GameObject hotZone;
    public GameObject triggerArea;
    public float Hitpoints;
    public float maxHitpoints;
    public HealthBarBehaviour HealthBar;
    #endregion

    #region Private Variables


    private Animator anim;
    private float distance;
    private bool attackMode;
  
    private bool cooling;
    private float intTimer;
    #endregion
    // Update is called once per frame
    private void Start()
    {
        Hitpoints = maxHitpoints;
        HealthBar.SetHealth(Hitpoints, maxHitpoints);
    }
    public void TakeHit(float damage)
    {
        Hitpoints -= damage;
        HealthBar.SetHealth(Hitpoints, maxHitpoints);
        if(Hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(!attackMode)
        {
            Move();
        }
        if(!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack") ){
            SelectTarget();
        }
       

        if(inRange)
        {
            EnemyLogic();
        }
    }

  
    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position,target.position);
        if(distance > attackDistance)
        {
           
            StopAttack();
        }else if(attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            CoolDown();
            anim.SetBool("Attack", false);
            
        }
    }
    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x,transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpped * Time.deltaTime);

        }
    }
    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }
    void CoolDown()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && cooling  && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }
   
    public void TriggerCooling() {
        cooling = true;
    }
    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }
    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);
        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }
        Flip();
    }
    public void Flip()
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

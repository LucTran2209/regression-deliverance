using UnityEngine;
using System.Collections;


namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] float m_speed = 4.0f;
        [SerializeField] float m_jumpForce = 7.5f;
        [SerializeField] float m_rollForce = 6.0f;
        // [SerializeField] bool m_noBlood = false;
        // [SerializeField] GameObject m_slideDust;

        private Animator m_animator;
        private Rigidbody2D m_body2d;
        private SensorPlayer m_groundSensor;

        private bool m_isGrounded = false;
        private bool m_isRolling = false;
        private int m_facingDirection = 1;
        private float m_delayToIdle = 0.0f;
        private readonly float m_rollDuration = 8.0f / 14.0f;
        private float m_rollCurrentTime;


        // Use this for initialization
        void Start()
        {
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            m_groundSensor = transform.Find("GroundSensor").GetComponent<SensorPlayer>();
        }

        // Update is called once per frame
        void Update()
        {

            // Increase timer that checks roll duration
            if (m_isRolling)
                m_rollCurrentTime += Time.deltaTime;

            // Disable rolling if timer extends duration
            if (m_rollCurrentTime > m_rollDuration)
                m_isRolling = false;

            //Check if character just landed on the ground
            if (!m_isGrounded && m_groundSensor.State())
            {
                m_isGrounded = true;
                m_animator.SetBool("Grounded", m_isGrounded);
            }

            //Check if character just started falling
            if (m_isGrounded && !m_groundSensor.State())
            {
                m_isGrounded = false;
                m_animator.SetBool("Grounded", m_isGrounded);
            }

            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
            }

            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
            }

            // Move
            if (!m_isRolling)
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Set AirSpeed in animator when jumping
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
          


            // Block
            if (Input.GetMouseButtonDown(1) && !m_isRolling)
            {
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
            }

            else if (Input.GetMouseButtonUp(1))
                m_animator.SetBool("IdleBlock", false);

            // Roll
            else if (Input.GetKeyDown("left shift") && !m_isRolling)
            {
                m_isRolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }

            //Jump
            else if ((Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.W)) && m_isGrounded && !m_isRolling)
            {
                m_animator.SetTrigger("Jump");
                m_isGrounded = false;
                m_animator.SetBool("Grounded", m_isGrounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }

            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                // Reset timer
                //m_delayToIdle = 0.05f;
                m_animator.SetInteger("AnimState", 1);
            }

            //Idle
            else
            {
                // Prevents flickering transitions to idle
                m_delayToIdle -= Time.deltaTime;
                if (m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
            }

            if (Input.GetKeyDown(KeyCode.H)){
                this.GetComponent<Health>().TakeDmg(200);
            }

        }


		private void OnTriggerEnter2D(Collider2D collision)
		{
            
			if(collision.tag == "Monster")
            {
                collision.GetComponent<Health>().TakeDmg(200);
                Debug.Log(collision.GetComponent<Health>().current_health);
            }
		}
	}
}


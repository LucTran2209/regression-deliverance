﻿using UnityEngine;
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
        private int m_currentAttack = 0;
        private float m_timeSinceAttack = 0.0f;
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
            // Increase timer that controls attack combo
            m_timeSinceAttack += Time.deltaTime;

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
          
            //Attack
            if (Input.GetKey(KeyCode.J) && m_timeSinceAttack > 0.25f && !m_isRolling)
            {
                m_currentAttack++;

                // Loop back to one after third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);

                // Reset timer
                m_timeSinceAttack = 0.0f;
            }

            // Block
            else if (Input.GetMouseButtonDown(1) && !m_isRolling)
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
        }       
    }
}


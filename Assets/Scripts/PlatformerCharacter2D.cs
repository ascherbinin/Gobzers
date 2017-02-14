using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Gobzers
{
    public class PlatformerCharacter2D : NetworkBehaviour
    {
        // PUBLIC
        public Camera playerCamera;
        
		// PRIVATE
		private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
		private int _fallDamageModify = 0;
		private Animator m_Anim;            // Reference to the player's animator component.
		private Rigidbody2D m_Rigidbody2D;
		private PlayerPosSync syncPos;
		private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private bool fallDamage = false;
		// Whether or not the player is grounded.
		private Transform m_CeilingCheck;   // A position marking where to check for ceilings

		[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
		[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
		[Range(0, 1)] 
		[SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, 1)] 
		[SerializeField] private float m_BackWalkSpeed = .50f;
		[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
		[SerializeField] private Health _health;
		[SerializeField] private bool m_Grounded; 


		//CONSTS

		public const int FALL_DAMAGE_DEF_VALUE = 10;
		const float GROUNDED_RADIUS = .2f; // Radius of the overlap circle to determine if grounded
		const float CEILING_RADIUS = .01f; // Radius of the overlap circle to determine if the player can stand up

		private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            syncPos = GetComponent<PlayerPosSync>();
			_health = GetComponent<Health> ();
        }


        private void FixedUpdate()
        {
            if (!isLocalPlayer)
                return;
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, GROUNDED_RADIUS, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

			Vector3 mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log (mousePos);
			if ((mousePos.x > transform.position.x && !m_FacingRight) || (mousePos.x < transform.position.x && m_FacingRight)) {
				m_FacingRight = !m_FacingRight;
				syncPos.CmdFlipSprite(m_FacingRight);
			}

			if (!m_Grounded) {
				float currentSpeed = Mathf.Abs (m_Rigidbody2D.velocity.y);
				Debug.Log (currentSpeed);
				if (currentSpeed > 30f) {
					fallDamage = true;
					_fallDamageModify = (int)currentSpeed / 10;
					Debug.Log ("FALL: " + _fallDamageModify);
				} else
					fallDamage = false;
			}

			if (m_Grounded && fallDamage) 
			{
				Debug.Log ("DAMAGE MODIFY = " + _fallDamageModify);
				Debug.Log ("DAMAGE = " + FALL_DAMAGE_DEF_VALUE * (int)_fallDamageModify);
				_health.TakeDamage (FALL_DAMAGE_DEF_VALUE * (int)_fallDamageModify);
				fallDamage = false;
				_fallDamageModify = 0;
			}
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, CEILING_RADIUS, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);
				if (m_FacingRight && m_Rigidbody2D.velocity.x < 0 ||
					!m_FacingRight && m_Rigidbody2D.velocity.x > 0) {
					move = move * m_BackWalkSpeed;
				}
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }
}

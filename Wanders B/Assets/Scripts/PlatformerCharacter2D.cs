using System;
using UnityEngine;
using System.Collections.Generic;

namespace UnityStandardAssets._2D {
	public enum InteractionType {
		slingshotProjectile
	}

	public class PlatformerCharacter2D : MonoBehaviour {

		[NonSerialized]
		public static PlatformerCharacter2D Instance;

		[SerializeField]
		private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
		[SerializeField]
		private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
		[Range(0, 1)]
		[SerializeField]
		private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[SerializeField]
		private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
		[SerializeField]
		private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

		private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
		const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool m_Grounded;            // Whether or not the player is grounded.
		private Transform m_CeilingCheck;   // A position marking where to check for ceilings
		const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
		private Animator m_Anim;            // Reference to the player's animator component.
		private Rigidbody2D m_Rigidbody2D;
		public bool m_FacingRight = true;  // For determining which way the player is currently facing.

		/*
		[NonSerialized]
		public Spell current_spell;
		[SerializeField]
		private GameObject fireball;
		[SerializeField]
		private GameObject airBurst;
		[SerializeField]
		private float fireballSpeed = 20;
		[SerializeField]
		private float airBurstSpeed = 25;
		*/
		[SerializeField]
		private GameObject projectile;

		[SerializeField]
		public float knockbackForce = 10;

		private List<string> tagsReactingToAir = new List<string>() { "Normal Object", "Physics Object", "Interactable" };
		private Transform launcher;
		private Transform arms;

		private void Awake() {
			Instance = this;
			// Setting up references.
			m_GroundCheck = transform.Find("GroundCheck");
			m_CeilingCheck = transform.Find("CeilingCheck");
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();

			//current_spell = Spell.Earth;

			foreach (Transform child in transform) {
				if (child.name == "Arms") {
					arms = child;
					launcher = child.GetChild(0);
				}
			}
		}


		private void FixedUpdate() {
			m_Grounded = false;

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders[i].gameObject != gameObject && (colliders[i].gameObject.tag == "Ground" || colliders[i].gameObject.tag == "Physics Object"))
				{
					m_Grounded = true;
					m_Anim.SetBool("Jump", false);
				}
			}
			m_Anim.SetBool("Grounded", m_Grounded);
			m_Anim.SetFloat("VSpeed", m_Rigidbody2D.velocity.y);
			//m_Anim.SetBool("Ground", m_Grounded);

			// Set the vertical animation
			//m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);


		}


		public void Move(float move, bool crouch, bool jump) {
			// If crouching, check to see if the character can stand up
			//if (!crouch && m_Anim.GetBool("Crouch")) {
				// If the character has a ceiling preventing them from standing up, keep them crouching
				//if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) {
					//crouch = true;
				//}
			//}

			// Set whether or not the character is crouching in the animator
			//m_Anim.SetBool("Crouch", crouch);

			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl) {
				// Reduce the speed if crouching by the crouchSpeed multiplier
				move = (crouch ? move * m_CrouchSpeed : move);

				// The Speed animator parameter is set to the absolute value of the horizontal input.
				//m_Anim.SetFloat("Speed", Mathf.Abs(move));

				// Move the character
				m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight) {
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight) {
					// ... flip the player.
					Flip();
				}
			}
			// If the player should jump...
			//if (m_Grounded && jump && m_Anim.GetBool("Ground")) {
			if (m_Grounded && jump) {
				m_Anim.SetBool("Jump", true);
				// Add a vertical force to the player.
				m_Grounded = false;
				//m_Anim.SetBool("Ground", false);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
			if (move != 0) m_Anim.SetBool("isWalking", true);
			else m_Anim.SetBool("isWalking", false);
		}


		private void Flip() {
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public void Shoot(Vector3 dir)
		{
			Vector3 source = launcher.transform.position;
			GameObject spawn = (GameObject)Instantiate(projectile, source, projectile.transform.rotation);
			spawn.GetComponent<Rigidbody2D>().velocity = (dir - spawn.transform.position).normalized * 25;
		}

		public void Death()
		{
			GetComponent<SpriteRenderer>().enabled = false;
			arms.GetComponent<SpriteRenderer>().enabled = false;
			UIScript.Instance.ReloadScene();
		}

		/*

		public void NextSpell() {
			current_spell = (Spell)(((int)current_spell + 1 < Enum.GetNames(typeof(Spell)).Length) ? ((int)current_spell + 1) : (1));
		}


		public void PreviousSpell() {
			current_spell = (Spell)(((int)current_spell - 1 > 0) ? ((int)current_spell - 1) : (Enum.GetNames(typeof(Spell)).Length - 1));
		}


		public void CastEarth() {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.down, 10, (1 << LayerMask.NameToLayer("Earth")));


			if (hitInfo) {
				if (hitInfo.transform.gameObject.tag == "Interactable") {
					hitInfo.transform.GetComponent<InteractionBehaviour>().Hit(Spell.Earth, hitInfo.point);
				} else {
					Debug.Log("Did not find platform");
				}
			}
		}


		public void CastFire() {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
			Vector3 direction = Camera.main.ScreenToWorldPoint(mousePos);
			Vector3 source = launcher.position;
			GameObject spawn = (GameObject)Instantiate(fireball, source, fireball.transform.rotation);
			spawn.GetComponent<Rigidbody2D>().isKinematic = true;
			spawn.GetComponent<Rigidbody2D>().velocity = (direction - spawn.transform.position).normalized * fireballSpeed;
		}


		public void CastAir() {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
			Vector3 source = launcher.position;
			Vector3 destination = Camera.main.ScreenToWorldPoint(mousePos);
			RaycastHit2D hitInfo = Physics2D.Raycast(source, destination - source, 10, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Spikes")));
			Debug.DrawRay(source, destination - source, Color.green, 0.3f);
			if (hitInfo.collider != null && Vector2.Angle((m_FacingRight) ? transform.right : -transform.right, destination) < 175f) {
				if (hitInfo.transform.gameObject.tag == "Interactable") {
					if (hitInfo.transform.GetComponent<InteractionBehaviour>().Hit(UnityStandardAssets._2D.Spell.Air, hitInfo.point) == -1 && m_Rigidbody2D.velocity.y < m_MaxSpeed) {
						ApplyKnockback(((Vector2)transform.position - hitInfo.point).normalized * knockbackForce);
					}
				} else if (tagsReactingToAir.Contains(hitInfo.transform.gameObject.tag) &&  m_Rigidbody2D.velocity.y < m_MaxSpeed) {
					ApplyKnockback(((Vector2)transform.position - hitInfo.point).normalized * knockbackForce);
				}
			}
		}

	*/
		public void ApplyKnockback(Vector2 force) {

			m_Rigidbody2D.AddForce(force, ForceMode2D.Impulse);
		}
	}
}

using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof(PlatformerCharacter2D))]
	public class Platformer2DUserControl : MonoBehaviour
	{

		[SerializeField]
		private float cooldown = 0.5f;

		private PlatformerCharacter2D m_Character;
		private bool m_Jump;

		private Vector3 destination;
		private bool canShoot = true;

		private GameObject arms;
		private Vector2 direction;

		private void Awake()
		{
			m_Character = GetComponent<PlatformerCharacter2D>();
			foreach (Transform child in transform)
			{
				if (child.name == "Arms")
				{
					arms = child.gameObject;
					break;
				}
			}
		}

		private void Update()
		{

			if (!m_Jump)
			{
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}

			if (CrossPlatformInputManager.GetButtonDown("Fire1"))
			{
				if (canShoot)
				{
					Vector3 mousePos = Input.mousePosition;
					mousePos.z = Mathf.Abs(Camera.main.transform.position.z) + transform.position.z;
					destination = Camera.main.ScreenToWorldPoint(mousePos);

					direction = destination - arms.transform.position;
					if (Vector2.Angle(transform.right * Mathf.Sign(transform.localScale.x), direction) < 80)
					{
						UpdateArmRotation(direction);
						m_Character.Shoot(destination);
						StartCoroutine(AfterShot());
					}


				}
			}
			/*

			if (CrossPlatformInputManager.GetAxisRaw("Mouse ScrollWheel") > 0) {
				UnityStandardAssets._2D.PlatformerCharacter2D.Instance.NextSpell();
			} else if ((CrossPlatformInputManager.GetAxisRaw("Mouse ScrollWheel") < 0)) {
				UnityStandardAssets._2D.PlatformerCharacter2D.Instance.PreviousSpell();
			}
			*/
			/*
			if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
				switch (UnityStandardAssets._2D.PlatformerCharacter2D.Instance.current_spell) {
					case Spell.None:
						break;
					case Spell.Earth:
						if (canCastEarth) {
							m_Character.CastEarth();
							StartCoroutine(AfterCast(UnityStandardAssets._2D.PlatformerCharacter2D.Instance.current_spell));
						}
						break;
					case Spell.Fire:
						if (canCastFire) {
							m_Character.CastFire();
							StartCoroutine(AfterCast(UnityStandardAssets._2D.PlatformerCharacter2D.Instance.current_spell));
						}
						break;
					case Spell.Air:
						if (canCastAir) {
							m_Character.CastAir();
							StartCoroutine(AfterCast(UnityStandardAssets._2D.PlatformerCharacter2D.Instance.current_spell));
						}
						break;
				}
			}
			*/
		}

		public int rotationOffset = 0;
		private float rotZ;

		void UpdateArmRotation(Vector2 direction)
		{
			if (!arms) return;
			if (transform.localScale.x > 0) rotationOffset = 0;
			else rotationOffset = 180;
			rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Find the angle in degrees
			if (direction.x > 0)
			{
				arms.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
			}
			else
			{
				rotZ = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
				arms.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
			}

		}

		IEnumerator AfterShot()
		{
			canShoot = false;
			yield return new WaitForSeconds(cooldown);
			UpdateArmRotation(transform.right * Mathf.Sign(transform.localScale.x));
			canShoot = true;
			yield return null;
		}


		/*
		IEnumerator AfterCast(Spell spell) {
			switch (UnityStandardAssets._2D.PlatformerCharacter2D.Instance.current_spell) {
				case Spell.None:
					break;
				case Spell.Earth:
					canCastEarth = false;
					yield return new WaitForSeconds(earthSpellCooldown);
					canCastEarth = true;
					break;
				case Spell.Fire:
					canCastFire = false;
					yield return new WaitForSeconds(fireSpellCooldown);
					canCastFire = true; break;
				case Spell.Air:
					canCastAir = false;
					yield return new WaitForSeconds(airSpellCooldown);
					canCastAir = true;
					break;
			}
			yield return null;
		}
		*/

		private void FixedUpdate()
		{
			// Read the inputs.
			bool crouch = Input.GetKey(KeyCode.LeftControl);
			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			// Pass all parameters to the character control script.
			m_Character.Move(h, crouch, m_Jump);
			m_Jump = false;
		}
	}
}

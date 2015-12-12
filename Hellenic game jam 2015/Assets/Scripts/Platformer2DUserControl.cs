using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

namespace UnityStandardAssets._2D {
	[RequireComponent(typeof(PlatformerCharacter2D))]
	public class Platformer2DUserControl : MonoBehaviour {

		[SerializeField]
		private float cooldown = 0.5f;

		private PlatformerCharacter2D m_Character;
		private bool m_Jump;

		private Vector3 direction;
		private bool canShoot = true;

		private void Awake() {
			m_Character = GetComponent<PlatformerCharacter2D>();
		}

		private void Update() {
			
			if (!m_Jump) {
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}

			if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
				if (canShoot) {
					Vector3 mousePos = Input.mousePosition;
					mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
					direction = Camera.main.ScreenToWorldPoint(mousePos);
					m_Character.Shoot(direction);
					StartCoroutine(AfterShot());
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

		IEnumerator AfterShot() {
			canShoot = false;
			yield return new WaitForSeconds(cooldown);
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

		private void FixedUpdate() {
			// Read the inputs.
			bool crouch = Input.GetKey(KeyCode.LeftControl);
			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			// Pass all parameters to the character control script.
			m_Character.Move(h, crouch, m_Jump);
			m_Jump = false;
		}
	}
}

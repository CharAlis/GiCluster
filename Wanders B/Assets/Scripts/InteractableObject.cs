using UnityEngine;
using System.Collections;

using UnityStandardAssets._2D;

public enum ReactionType {
	KnockbackObject,
	PullObject,
	Break,
	Damage
}

public class InteractableObject : MonoBehaviour {



	[SerializeField]
	private ReactionType slingProjectileReaction;

	public void ApplyAction(InteractionType intType) {
		ApplyAction(intType, Vector2.zero);
	}

	public void ApplyAction(InteractionType intType, Vector2 v) {

		switch (intType) {
			case InteractionType.slingshotProjectile:
				switch(slingProjectileReaction) {
					case ReactionType.KnockbackObject:
						GetComponent<Rigidbody2D>().AddForce(v * 80);
							break;
					case ReactionType.PullObject:
						GetComponent<Rigidbody2D>().angularVelocity = Mathf.Sign(v.x) * 80f;
						break;
					case ReactionType.Break:
						break;
					case ReactionType.Damage:
						GetComponent<Rigidbody2D>().velocity = new Vector2(5 * Mathf.Sign(v.x), GetComponent<Rigidbody2D>().velocity.y + 2);
                        //GetComponent<Rigidbody2D>().AddForce(v * 0.2f, ForceMode2D.Impulse);
                        GetComponent<PatrollerEnemyScript>().Damage();
						break;
				}
				break;
			default:
				break;
		}
		
	}
}

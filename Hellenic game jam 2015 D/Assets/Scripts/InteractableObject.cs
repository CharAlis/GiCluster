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
        print("I'm inside-.-");
		switch (intType) {
			case InteractionType.slingshotProjectile:
				switch(slingProjectileReaction) {
					case ReactionType.KnockbackObject:
						print("I'm being knocked back");
						GetComponent<Rigidbody2D>().AddForce(v * 20);
							break;
					case ReactionType.PullObject:
						print("It's sucking me in");
						GetComponent<Rigidbody2D>().angularVelocity = Mathf.Sign(v.x) * 80f;
						break;
					case ReactionType.Break:
						print("I'm gonna break");
						break;
					case ReactionType.Damage:
						print("It hurts");
                        GetComponent<Rigidbody2D>().AddForce(v * 20);
                        GetComponent<PatrollerEnemyScript>().Damage();
						break;
				}
				break;
			default:
				break;
		}
		
	}
}

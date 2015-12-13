using UnityEngine;
using System.Collections;

public class SlingshotProjectile : MonoBehaviour {

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Interactable" || col.tag == "Physics Object" || col.tag == "Enemy") {
			col.GetComponent<InteractableObject>().ApplyAction(UnityStandardAssets._2D.InteractionType.slingshotProjectile, GetComponent<Rigidbody2D>().velocity);
		}
		if (col.tag != "CameraBounds" && col.tag != "Player" && col.gameObject.layer != LayerMask.NameToLayer("UI") && col.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")) {
			Destroy(gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class PhysicsKillingScript : MonoBehaviour {

	private Rigidbody2D rb;
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.tag == "Enemy" && rb.velocity.y < 0)
		{
			col.collider.GetComponent<PatrollerEnemyScript>().Death(2);
		}
	}
}

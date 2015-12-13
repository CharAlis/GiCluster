using UnityEngine;
using System.Collections;

public class ThornScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			UnityStandardAssets._2D.PlatformerCharacter2D.Instance.Death();
		}
		if (col.tag == "Enemy")
		{
			col.GetComponent<PatrollerEnemyScript>().Death(0.5f);
		}
	}
}

using UnityEngine;
using System.Collections;

public class ArmRotationScript : MonoBehaviour {
	public int rotationOffset = 0;
	private Vector3 difference;
	private float rotz;

	void Update () {
		difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();

		rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		if (difference.x > 0) transform.rotation = Quaternion.Euler(0f, 0f, rotz + rotationOffset);
		else
		{
			rotz = Mathf.Atan2(-difference.y, difference.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rotz + rotationOffset - 10);
		}
	}
}

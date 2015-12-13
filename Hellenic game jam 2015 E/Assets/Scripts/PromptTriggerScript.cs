using UnityEngine;
using System.Collections;

public class PromptTriggerScript : MonoBehaviour {

	public string str;
	public float time;
	public bool prompt = true;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			if (prompt) UIScript.Instance.WorldPrompt(str, time, new Vector2(transform.position.x, transform.position.y + 1));
			else UIScript.Instance.ChangeScene();
			GetComponent<Collider2D>().enabled = false;
		}
	}
}

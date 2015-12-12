using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIScript : MonoBehaviour {

	public static UIScript Instance;
	private Text prompts;
	private Animator animator;
	public bool paused = false;
	void Awake()
	{
		Instance = this;
		prompts = GameObject.Find("Prompts").GetComponent<Text>();
		animator = GameObject.Find("Fading").GetComponent<Animator>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ChangeScene()
	{
		animator.SetTrigger("FadeToBlack");
		StartCoroutine(WaitToChangeScene());
	}

	IEnumerator WaitToChangeScene()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void SetPrompt(string msg, float time)
	{
		StopAllCoroutines();
		prompts.text = msg;
		StartCoroutine(FadeOut(time));
	}

	IEnumerator FadeOut(float time)
	{
		for (float i = 1f; i > 0f; i -= (1 / time * Time.deltaTime))
		{
			prompts.color = new Color(prompts.color.r, prompts.color.g, prompts.color.b, i);
			yield return null;
		}
		prompts.color = new Color(prompts.color.r, prompts.color.g, prompts.color.b, 0f);
		prompts.text = "";
		yield return null;
	}

	void Update()
	{
		if (paused)
		{
			if (Input.GetButtonDown("Cancel"))
			{
				UnpauseGame();
			}
		}
		else
		{
			if (Input.GetButtonDown("Cancel"))
			{
				PauseGame();
			}
		}
	}

	public void PauseGame()
	{
		paused = true;
		prompts.text = "PAUSED";
		Time.timeScale = 0;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void UnpauseGame()
	{
		paused = false;
		prompts.text = "";
		Time.timeScale = 1;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}

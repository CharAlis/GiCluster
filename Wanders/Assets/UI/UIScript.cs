using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIScript : MonoBehaviour {

	public static UIScript Instance;
	private Text prompts;
	private Animator animator;
	private Animator logoAnimator;
	public bool paused = false;
	private bool waitingForInput = true;
	private Canvas worldCanvas;
	private Text worldPrompt;
	void Awake()
	{
		Instance = this;
		prompts = GameObject.Find("Prompts").GetComponent<Text>();
		animator = GameObject.Find("Fading").GetComponent<Animator>();
		logoAnimator = GameObject.Find("Logo").GetComponent<Animator>();
		worldCanvas = GameObject.Find("WorldCanvas").GetComponent<Canvas>();
		worldPrompt = GameObject.Find("WorldPrompt").GetComponent<Text>();
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			SetPrompt("Press A or D to move.", 999);
		}
	}

	public void ChangeScene()
	{
		animator.SetTrigger("FadeToBlack");
		StartCoroutine(WaitToChangeScene(SceneManager.GetActiveScene().buildIndex + 1));
	}

	IEnumerator WaitToChangeScene(int scene)
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(scene);
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
		if (Input.GetAxisRaw("Horizontal") != 0 && waitingForInput && SceneManager.GetActiveScene().buildIndex == 0)
		{
			waitingForInput = false;
			logoAnimator.SetTrigger("LogoToClear");
			SetPrompt("Press A or D to move.", 0.1f);
		}
	}

	public void PauseGame()
	{
		paused = true;
		prompts.text = "PAUSED";
		Time.timeScale = 0;
	}

	public void UnpauseGame()
	{
		paused = false;
		prompts.text = "";
		Time.timeScale = 1;
	}

	public void ReloadScene()
	{
		animator.SetTrigger("FadeToBlack");
		StartCoroutine(WaitToChangeScene(SceneManager.GetActiveScene().buildIndex));
	}
	
	public void WorldPrompt(string msg, float time, Vector2 pos)
	{
		worldCanvas.transform.position = pos;
		StopAllCoroutines();
		worldPrompt.text = msg;
		StartCoroutine(WorldFadeOut(time));
	}

	IEnumerator WorldFadeOut(float time)
	{
		for (float i = 1f; i > 0f; i -= (1 / time * Time.deltaTime))
		{
			worldPrompt.color = new Color(worldPrompt.color.r, worldPrompt.color.g, worldPrompt.color.b, i);
			yield return null;
		}
		worldPrompt.color = new Color(worldPrompt.color.r, worldPrompt.color.g, worldPrompt.color.b, 0f);
		worldPrompt.text = "";
		yield return null;
	}

}

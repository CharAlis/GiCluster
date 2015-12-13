using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}

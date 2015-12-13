using UnityEngine;
using System.Collections;

public class FollowCharacterX : MonoBehaviour {

    Transform playerTransform;

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
	}
}

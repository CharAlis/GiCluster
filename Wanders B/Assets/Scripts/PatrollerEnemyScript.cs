using UnityEngine;
using System.Collections;

public class PatrollerEnemyScript : MonoBehaviour {
    public int healthPoints = 5;
    public float movementSpeed = 5f;
    [Range(-1,1)]
    public int direction = -1;

    public float timeToWaitAfterLoosingPlayer = 1f;
    public float timeToKeepCharging = 0.5f;

    float initialSpeed;
    bool charging = false;
    bool spottedPlayer = false;

    Vector3 startingPosition;
    Vector3 targetPosition;
    Vector3 leftmostPosition;
    Vector3 rightmostPosition;

	private Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

    void Start() {
        startingPosition = transform.position;
        initialSpeed = movementSpeed;
        foreach (Transform child in transform) {
            if (child.name == "LeftChild") {
                leftmostPosition = child.gameObject.transform.position;
            }
            if (child.name == "RightChild") {
                rightmostPosition = child.gameObject.transform.position;
            }
        }
    }
	
	void Update () {

        if (spottedPlayer) {
			animator.SetBool("isCharging", true);
            if ((targetPosition.x < transform.position.x && direction > 0) || (targetPosition.x > transform.position.x && direction < 0)) Flip();
            //transform.Translate(Vector3.right * direction * movementSpeed * Time.deltaTime);
        } /*else {
            if (Mathf.Abs(transform.position.x - leftmostPosition.transform.position.x) < Mathf.Abs(transform.position.x - rightmostPosition.transform.position.x))
                if (transform.position.x < leftmostPosition.transform.position.x && direction < 0) {
                    Flip();
                }
            else
                if (transform.position.x > rightmostPosition.transform.position.x && direction > 0) {
                    Flip();
                }
            //transform.Translate(Vector3.right * direction * movementSpeed * Time.deltaTime);
        }*/
        transform.Translate(Vector3.right * direction * movementSpeed * Time.deltaTime);
    }

    void LateUpdate() {
        if (!charging) {
			animator.SetBool("isCharging", false);
			if ((transform.position.x >= rightmostPosition.x && direction > 0) || (transform.position.x <= leftmostPosition.x && direction < 0)) {
                //direction = -direction;
                Flip();
                //GetComponentInChildren<BoxCollider2D>().offset = new Vector2(-GetComponentInChildren<BoxCollider2D>().offset.x, GetComponentInChildren<BoxCollider2D>().offset.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            targetPosition = other.transform.position;
            StopCoroutine(TargetLost());
            spottedPlayer = true;
            charging = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            if (!charging) charging = !charging;
            if (charging) {
                targetPosition = other.transform.position;
                if (movementSpeed <= initialSpeed)
                    movementSpeed = 2 * movementSpeed;
            } else {
                movementSpeed = initialSpeed;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            spottedPlayer = false;
            StartCoroutine(TargetLost());
        }
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.tag == "Player") UnityStandardAssets._2D.PlatformerCharacter2D.Instance.Death();
	}
    public void Damage() {
        healthPoints--;
        if (healthPoints == 0) {
            Death(0f);
        }
    }

   public void Death(float deathTime) {
		Transform particles;
		particles = GetComponentInChildren<ParticleSystem>().transform;
		particles.SetParent(null);
		particles.GetComponent<ParticleSystem>().Emit(200);
		Destroy(particles.gameObject, 5);
		GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject,deathTime);
    }

    void Flip() {

        direction = -direction;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
 
    IEnumerator TargetLost() {
        yield return new WaitForSeconds(timeToWaitAfterLoosingPlayer);
        if (!spottedPlayer) {
            charging = false;
            movementSpeed = initialSpeed;
            //targetPosition = (Mathf.Abs(transform.position.x - leftmostPosition.transform.position.x) < Mathf.Abs(transform.position.x - rightmostPosition.transform.position.x)) ? (leftmostPosition.transform.position) : (rightmostPosition.transform.position);
        }
        yield return null;
    }

    IEnumerator ChargeTimer() {
        while (charging) {
            yield return new WaitForSeconds(timeToKeepCharging);
        }
        charging = false;
        yield return null;
    }
}
 

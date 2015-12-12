using UnityEngine;
using System.Collections;

public class PatrollerEnemyScript : MonoBehaviour {
    public int healthPoints = 5;
    public float movementSpeed = 5f;
    [Range(-1,1)]
    public int direction = -1;
    public Vector3 leftmostPosition;
    public Vector3 rightmostPosition;

    public float timeToWaitAfterLoosingPlayer = 1f;
    public float timeToKeepCharging = 0.5f;

    float initialSpeed;
    bool charging = false;
    bool spottedPlayer = false;

    Vector3 startingPosition;
    Vector3 targetPosition;

    void Start() {
        initialSpeed = movementSpeed;
    }
	
	void Update () {
        if (!charging) {
            transform.Translate(Vector3.right * direction * movementSpeed * Time.deltaTime);
        } else {
            if (targetPosition.x < transform.position.x) direction = -1;
            else direction = 1;
            transform.Translate(Vector3.right * direction * movementSpeed * Time.deltaTime);
        }
    }

    void LateUpdate() {
        if (!charging) {
            if ((transform.position.x >= rightmostPosition.x && direction > 0) || (transform.position.x <= leftmostPosition.x && direction < 0)) {
                direction = -direction;
                GetComponentInChildren<BoxCollider2D>().offset = new Vector2(-GetComponentInChildren<BoxCollider2D>().offset.x, GetComponentInChildren<BoxCollider2D>().offset.y);
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

    void Damage() {
        healthPoints--;
        if (healthPoints == 0) {
            Death(0f);
        }
    }

    void Death(float deathTime) {
        Destroy(gameObject, deathTime);
    }

    IEnumerator TargetLost() {
        yield return new WaitForSeconds(timeToWaitAfterLoosingPlayer);
        if (!spottedPlayer) {
            charging = false;
            movementSpeed = initialSpeed;
        }
        yield return null;
    }

    IEnumerator ChargeTimer() {
        while (charging) {
            yield return new WaitForSeconds(timeToKeepCharging);
        }
        yield return null;
    }
}

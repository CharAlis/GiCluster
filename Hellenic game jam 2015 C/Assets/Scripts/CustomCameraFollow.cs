using UnityEngine;
using System.Collections;

public class CustomCameraFollow : MonoBehaviour {

    public Transform target;
    public GameObject border;

    float damping = 0f;
    float lookAheadFactor = 0f;
    float lookAheadReturnSpeed = 0f;
    float lookAheadMoveThreshold = 0f;
    float yPosRestriction = -1f;

    // script defined borders, defined by public reference from collider
    Collider2D borderCollider;
    float minX = -20f;
    float maxX = 20f;
    float minY = -20f;
    float maxY = 20f;

    float offsetZ;
    Vector3 lastTargetPosition;
    Vector3 currentVelocity;
    Vector3 lookAheadPos;

    float orthographicWidth;
    float orthographicHeight; 

    float nextTimeToSearch = 0;

    // Use this for initialization
    void Start() {
        borderCollider = border.GetComponent<Collider2D>();
        minX = border.transform.position.x - (borderCollider.bounds.extents.x);
        maxX = border.transform.position.x + (borderCollider.bounds.extents.x);
        minY = border.transform.position.y - (borderCollider.bounds.extents.y);
        maxY = border.transform.position.y + (borderCollider.bounds.extents.y);

        lastTargetPosition = target.position;
        offsetZ = (transform.position - target.position).z;
        orthographicWidth = GetComponent<Camera>().orthographicSize * ((float) Screen.width / Screen.height);
        orthographicHeight = GetComponent<Camera>().orthographicSize;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update() {

        if (target == null) {
            FindPlayer();
            return;
        }

        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget) {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        } else {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

        newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);

        transform.position = newPos;

        if (transform.position.x < (minX + orthographicWidth)) {
            transform.position = new Vector3(minX + orthographicWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.x > (maxX - orthographicWidth)) {
            transform.position = new Vector3(maxX - orthographicWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.y < (minY + orthographicHeight)) {
            transform.position = new Vector3(transform.position.x, minY + orthographicHeight, transform.position.z);
        }
        if (transform.position.y > (maxY - orthographicHeight)) {
            transform.position = new Vector3(transform.position.x, maxY - orthographicHeight, transform.position.z);
        }

        lastTargetPosition = target.position;
    }
 
    void FindPlayer() {
        if (nextTimeToSearch <= Time.time) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
                target = searchResult.transform;
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeLayers : MonoBehaviour {
	[SerializeField]
	private bool playerSwitcher = true;

	public LayerMask[] layerList;
    LayerMask[] originalLayerList;

    List<GameObject>[] cachedObjects;

    int currentLayer;
    bool hasExited = true;
	int[] layernum;
    float startingVelocity;
    bool xLarger;

    void Start() {
        //originalLayerList = new LayerMask[layerList.Length];
        cachedObjects = new List<GameObject>[2];
        cachedObjects[0] = new List<GameObject>();
        cachedObjects[1] = new List<GameObject>();
        originalLayerList = new LayerMask[2];

        for (int i = 0; i < originalLayerList.Length; i++) {
            originalLayerList[i] = layerList[i];
        }


        layernum = new int[originalLayerList.Length];
        for (int i = 0; i < originalLayerList.Length; ++i) {
            int temp = originalLayerList[i].value;
            while (temp % 2 != 1) {
                temp /= 2;
                ++layernum[i];
            }
        }
          
        cachedObjects[0] = FindGameObjectsWithLayer(layernum[0]);
        cachedObjects[1] = FindGameObjectsWithLayer(layernum[1]);
        StartCoroutine(HideColliders(currentLayer + 1));

        layerList[1] |= Camera.main.cullingMask;
        layerList[1] ^= layerList[0];
        layerList[0] = Camera.main.cullingMask;

        currentLayer = 0;

        if (gameObject.GetComponent<BoxCollider2D>().bounds.extents.x > gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) xLarger = true;
        else xLarger = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
		if (playerSwitcher) {
			if (col.tag == "Player" && hasExited) {
                print("i have entered (enter)" + hasExited);
                if (xLarger) {
                    print("Checking y");
                    startingVelocity = col.gameObject.GetComponent<Rigidbody2D>().velocity.y;
                } else {
                    print("Checking x");
                    startingVelocity = col.gameObject.GetComponent<Rigidbody2D>().velocity.x;
                }
                print("startingVelocity =  " + startingVelocity);
                //StartCoroutine(HideColliders(currentLayer));
                hasExited = false;
				//currentLayer = ((++currentLayer) % layerList.Length);
				//Camera.main.cullingMask = layerList[currentLayer];
				//StartCoroutine(EnableColliders(currentLayer));
			}
		} else {
			if (col.gameObject.layer == layernum[currentLayer]) {
				col.gameObject.layer = layernum[(~currentLayer) & 1];
                print("currentLayer " + currentLayer + " ~currentlayer&1: " + ((~currentLayer) & 1));
			} else if (col.gameObject.layer == layernum[(~currentLayer) & 1]) {
				col.gameObject.layer = layernum[currentLayer];
                print("currentLayer " + currentLayer + " ~currentlayer&1: " + ((~currentLayer) & 1));
            }
		}
	}

    void OnTriggerExit2D(Collider2D col) {
        if (playerSwitcher) {
            if (col.tag == "Player" && hasExited == false) {
                print("i have entered (exit)" + hasExited);
                hasExited = true;
                if (xLarger) {
                    print("Checking y");
                    if (Mathf.Sign(col.gameObject.GetComponent<Rigidbody2D>().velocity.y) != Mathf.Sign(startingVelocity)) {
                        print("Velocity : " + col.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                        return;
                    }
                } else {
                    print("Checking x");
                    if (Mathf.Sign(col.gameObject.GetComponent<Rigidbody2D>().velocity.x) != Mathf.Sign(startingVelocity)) {
                        print("Velocity : " + col.gameObject.GetComponent<Rigidbody2D>().velocity.x);
                        return;
                    }
                }
                StartCoroutine(HideColliders(currentLayer));
                currentLayer = ((++currentLayer) % layerList.Length);
                Camera.main.cullingMask = layerList[currentLayer];
                StartCoroutine(EnableColliders(currentLayer));
            }
        }
    }	

    IEnumerator HideColliders(int index) {
		cachedObjects[0] = FindGameObjectsWithLayer(layernum[0]);
		cachedObjects[1] = FindGameObjectsWithLayer(layernum[1]);
		if (cachedObjects[index] == null) yield break;
        foreach (GameObject obj in cachedObjects[index]) {
			Collider2D col = obj.GetComponent<Collider2D>();
			if (col != null)
				col.enabled = false; 
			Rigidbody2D rig = obj.GetComponent<Rigidbody2D>();
			if (rig != null)
				rig.isKinematic = true;
			yield return null;
        }
        yield return null;
    }

    IEnumerator EnableColliders(int index) {
		cachedObjects[0] = FindGameObjectsWithLayer(layernum[0]);
		cachedObjects[1] = FindGameObjectsWithLayer(layernum[1]);
		if (cachedObjects[index] == null) yield break;
        foreach (GameObject obj in cachedObjects[index]) {
			Collider2D col = obj.GetComponent<Collider2D>();
			if (col != null)
				col.enabled = true;
			Rigidbody2D rig = obj.GetComponent<Rigidbody2D>();
			if (rig != null)
				rig.isKinematic = false;
            yield return null;
        }
        yield return null;
    }

    List<GameObject> FindGameObjectsWithLayer(int layer) {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        foreach (GameObject g in goArray) {
            if (g.layer == layer) {
                goList.Add(g);
            }
        }

        /* List<GameObject> goList = new List<GameObject>();
         for (int i = 0; i < goArray.Length; i++) {
             if (goArray[i].layer == layer) {
                 goList.Add(goArray[i]);
             }
         }*/
        return goList;
    }
}

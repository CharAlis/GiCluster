using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeLayers : MonoBehaviour {
    public LayerMask[] layerList;
    LayerMask[] originalLayerList;

    List<GameObject>[] cachedObjects;

    int nextLayer;
    bool hasExited = true;

    void Start() {
        //originalLayerList = new LayerMask[layerList.Length];
        cachedObjects = new List<GameObject>[2];
        cachedObjects[0] = new List<GameObject>();
        cachedObjects[1] = new List<GameObject>();
        originalLayerList = new LayerMask[2];

        for (int i = 0; i < originalLayerList.Length; i++) {
            originalLayerList[i] = layerList[i];
        }
        print(originalLayerList[0].value);

        int[] layernum = new int[originalLayerList.Length];
        for (int i = 0; i < originalLayerList.Length; ++i) {
            int temp = originalLayerList[i].value;
            while (temp % 2 != 1) {
                temp /= 2;
                ++layernum[i];
            }
        }
          
        cachedObjects[0] = FindGameObjectsWithLayer(layernum[0]);
        cachedObjects[1] = FindGameObjectsWithLayer(layernum[1]);
        StartCoroutine(HideColliders(nextLayer + 1));

        layerList[1] |= Camera.main.cullingMask;
        layerList[1] ^= layerList[0];
        layerList[0] = Camera.main.cullingMask;

        nextLayer = 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && hasExited) {
            StartCoroutine(HideColliders(nextLayer));
            hasExited = false;
            nextLayer = ((++nextLayer) % layerList.Length);
            Camera.main.cullingMask = layerList[nextLayer];
            StartCoroutine(EnableColliders(nextLayer));
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            hasExited = true;
        }
    }

    IEnumerator HideColliders(int index) {
        if (cachedObjects[index] == null) yield break;
        foreach (GameObject obj in cachedObjects[index]) {
            obj.GetComponent<Collider2D>().enabled = false;
            yield return null;
        }
        yield return null;
    }

    IEnumerator EnableColliders(int index) {
        if (cachedObjects[index] == null) yield break;
        foreach (GameObject obj in cachedObjects[index]) {
            obj.GetComponent<Collider2D>().enabled = true;
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
        print(goList.Count);
        return goList;
    }
}

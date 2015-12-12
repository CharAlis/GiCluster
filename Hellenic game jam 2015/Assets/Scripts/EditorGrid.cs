using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorGrid : MonoBehaviour {

	public bool cluster = false;
	public float cell_size = 1f;
	private float x, y, z;
	public float cell_size_y = 1f;

	void Start()
	{
		x = 0f;
		y = 0f;
		z = 0f;
	}

	void Update()
	{
		x = Mathf.Round(transform.position.x / cell_size) * cell_size;
		y = (cluster) ? Mathf.Round(transform.position.y / cell_size) * cell_size_y : Mathf.Round(transform.position.y / cell_size) * cell_size;
		z = transform.position.z;
		transform.position = new Vector3(x, y, z);
	}
}

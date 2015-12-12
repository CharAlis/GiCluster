using UnityEngine;
using System.Collections;

public class SpriteSpawner : MonoBehaviour {

	public int x;
	public int y;
	public GameObject[] obj;
	public int in_random_min;
	public int in_random_max;
	public int top_random_min;
	public int top_random_max;
	private GameObject empty;
	private GameObject child;
	private int offsetX;
	private int offsetY;
	private BoxCollider2D col;
	private EditorGrid eg;
	public static int platformCounter = 0;
	private int counter = 0;

	public void BuildObject(Vector3 pos, out GameObject platform)
	{
		platformCounter++;
		empty = new GameObject("Platform" + platformCounter.ToString());
		empty.tag = "Platform";
		empty.transform.SetParent(transform);
		empty.transform.position = pos;
		col = empty.AddComponent<BoxCollider2D>();
		eg = empty.AddComponent<EditorGrid>();
		eg.cell_size = x * 2;
		eg.cluster = true;
		col.offset = new Vector2(0, 0);
		col.size = new Vector2(2 * x, 2 * y);
		offsetY = -(int)Mathf.Floor(y / 2) * 2;
		if (y % 2 == 0) offsetY++;
		for (int j = 0; j < y; j++, offsetY += 2)
		{
			offsetX = -(int)Mathf.Floor(x/2) * 2;
			if (x % 2 == 0) offsetX++;
			for (int i = 0; i < x; i++, offsetX += 2)
			{
				child = (j != y - 1) ? Instantiate(obj[Random.Range(in_random_min, in_random_max + 1)], new Vector3(pos.x + offsetX,pos.y + offsetY, 0), Quaternion.identity) as GameObject
					: Instantiate(obj[Random.Range(top_random_min, top_random_max + 1)], new Vector3(pos.x + offsetX, pos.y + offsetY, 0), Quaternion.identity) as GameObject;
				child.transform.SetParent(empty.transform);
			}
		}
		offsetX = offsetY = 0;
		platform = empty;
	}

	public void MakeObject(Vector3 pos)
	{
		GameObject background = GameObject.Find("Trees");
		if (background == null) background = new GameObject("Trees");
		GameObject tree = Instantiate(obj[counter % obj.Length], new Vector3(pos.x, pos.y, 10), Quaternion.identity) as GameObject;
		tree.transform.SetParent(background.transform);
		counter++;
	}

}

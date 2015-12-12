using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SpriteSpawner))]
public class SpriteSpawnerEditor : Editor {
	private GameObject platform;
	private SpriteSpawner spawner;

	void OnEnable()
	{
		spawner = (SpriteSpawner)target;
	}
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Spawn"))
		{
			spawner.BuildObject(Vector3.zero, out platform);
			Undo.RegisterCreatedObjectUndo(platform, "Create platform");
		}
	}

	void OnSceneGUI()
	{
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		Event e = Event.current;
		Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
		Vector3 mousePos = ray.origin;
		RaycastHit2D hit;

		if (e.isMouse && e.type == EventType.MouseDown && e.button == 0)
		{
			GUIUtility.hotControl = controlID;
			e.Use();
			//if (hit = Physics2D.Raycast(mousePos, ray.direction))
			//{
			//	if (hit.collider == null) spawner.BuildObject(mousePos, out platform);
			//	else
			//	{
			//		if (hit.collider.tag != "Platform") spawner.BuildObject(mousePos, out platform);
			//	}
			//}
			//else spawner.BuildObject(mousePos, out platform);
			spawner.MakeObject(mousePos);
			Undo.RegisterCreatedObjectUndo(platform, "Create platform");
		}
		if (e.isMouse && e.type == EventType.MouseDown && e.button == 1)
		{
			if (hit = Physics2D.Raycast(mousePos, ray.direction))
			{
				if (hit.collider != null)
				{
					if (hit.collider.tag == "Platform") DestroyImmediate(hit.collider.gameObject);
				}
			}
		}
		if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
		{
			spawner.x++;
		}
		if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.S)
		{
			spawner.x--;
		}
		if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.D)
		{
			spawner.y++;
		}
		if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.F)
		{
			spawner.y--;
		}
	}
}

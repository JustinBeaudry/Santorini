using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMouseController : MonoBehaviour
{
	public Camera camera;

	public delegate void ClickAction (Tile tile);

	public static event ClickAction OnClicked;

	Grid grid;
	CameraController cameraController;

	void Awake ()
	{
		cameraController = camera.GetComponent<CameraController> ();
		grid = GetComponent<Grid> ();
	}

	void FixedUpdate ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Node node = grid.NodeFromWorldPoint (hit.point);
				grid.SelectNode (node);
				Tile tile = hit.collider.gameObject.GetComponent<Tile> ();
				if (OnClicked != null) {
					OnClicked (tile);
				}
			}
		}
		if (Input.GetKey (KeyCode.W)) {
			cameraController.MoveUp ();
		}
		if (Input.GetKey (KeyCode.S)) {
			cameraController.MoveDown ();
		}
		if (Input.GetKey (KeyCode.A)) {
			cameraController.MoveLeft ();
		}
		if (Input.GetKey (KeyCode.D)) {
			cameraController.MoveRight ();
		}
	}
}
using UnityEngine;

public class KeyboardMouseController : MonoBehaviour
{
	public delegate void ClickAction (Tile tile);

	public static event ClickAction OnClicked;

	public Camera camera;

	private Grid grid;
	private CameraController cameraController;

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
				Tile tile;
				if (hit.collider.gameObject.name != "Tile(Clone)") {
					tile = hit.collider.transform.GetComponentInParent<Tile> ();
				} else {
					tile = hit.collider.gameObject.GetComponent<Tile> ();
				}
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
		if (Input.GetKeyUp (KeyCode.Escape) && !MatchController.SettingsOpen) {
			MatchController.SettingsOpen = true;
			GameManager.LoadSceneAdditive ("Game Options");	
		}
		if (Input.GetKeyUp (KeyCode.Escape) && MatchController.SettingsOpen) {
			MatchController.SettingsOpen = false;
			GameManager.UnloadSceneAdditive ("Game Options");
		}
	}
}
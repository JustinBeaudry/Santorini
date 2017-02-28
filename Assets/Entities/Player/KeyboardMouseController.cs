using UnityEngine;

public class KeyboardMouseController : ControlDevice
{
	void Update ()
	{
		if ( Input.GetMouseButtonDown ( 0 ) )
		{
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			RaycastHit hit;
			if ( Physics.Raycast ( ray, out hit ) )
			{
				Tile tile;
				if ( hit.collider.gameObject.name != "Tile(Clone)" )
				{
					tile = hit.collider.transform.GetComponentInParent<Tile> ();
				} else
				{
					tile = hit.collider.gameObject.GetComponent<Tile> ();
				}
				TileInteraction ( tile );
			}
		}
		if ( Input.GetKey ( KeyCode.W ) )
		{
			MoveCameraUp ();
		}
		if ( Input.GetKey ( KeyCode.S ) )
		{
			MoveCameraDown ();
		}
		if ( Input.GetKey ( KeyCode.A ) )
		{
			MoveCameraLeft ();
		}
		if ( Input.GetKey ( KeyCode.D ) )
		{
			MoveCameraRight ();
		}
		if ( Input.GetKeyUp ( KeyCode.Escape ) )
		{
			if ( MatchController.SettingsOpen )
			{
				ShowGameOptions ();
			} else
			{
				HideGameOptions ();
			}
		}
	}
}
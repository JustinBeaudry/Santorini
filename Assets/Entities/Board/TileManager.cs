using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager tileManager;

	public GameObject Tile;
	public List<Tile> tiles;

	void Awake ()
	{
		tileManager = this;
	}

	public static Tile CreateTileFromWorldPosition (Vector3 worldPosition)
	{
		GameObject tile = Instantiate (tileManager.Tile, tileManager.transform);	
		tile.transform.position = worldPosition;
		tileManager.tiles.Add (tile.GetComponent<Tile> ());
		return tile.GetComponent<Tile> ();
	}

	public static Tile TileFromWorldPosition (Vector3 worldPosition)
	{
		Tile tile = null;
		foreach (Tile t in tileManager.tiles) {
			if (Vector3.Distance (t.transform.position, worldPosition) < 0.5f) {
				tile = t;
			}
		}
		return tile;
	}
}

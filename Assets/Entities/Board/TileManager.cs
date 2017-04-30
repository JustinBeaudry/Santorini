using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager tileManager;

	public Grid grid;
	public GameObject Tile;
	public List<Tile> tiles;

	void Awake ()
	{
		tileManager = this;
		grid = GameObject.Find ( "GridManager" ).GetComponent<Grid> ();
	}

	public static Tile CreateTileFromWorldPosition ( Vector3 worldPosition )
	{
		GameObject tile = Instantiate ( tileManager.Tile, tileManager.transform );	
		tile.transform.position = worldPosition;
		tileManager.tiles.Add ( tile.GetComponent<Tile> () );
		return tile.GetComponent<Tile> ();
	}

	public static Tile TileFromWorldPosition ( Vector3 worldPosition )
	{
		Tile tile = null;
		foreach ( Tile t in tileManager.tiles )
		{
			if ( Vector3.Distance ( t.transform.position, worldPosition ) < 0.5f )
			{
				tile = t;
			}
		}
		return tile;
	}

	public static List<Tile> GetNeighbors ( Tile tile )
	{
		List<Tile> neighbors = new List<Tile> ();
		if ( tile == null )
		{
			return neighbors;
		}
		Player player = GameActionController.CurrentPlayer;
		if ( player == null )
		{
			return neighbors;
		}
		Node node = tileManager.grid.NodeFromWorldPoint ( player.CurrentWorker.CurrentTile.transform.position );
		if ( node == null )
		{
			return neighbors;
		}
		// get all the neighbors of that node
		List<Node> neighborNodes = tileManager.grid.GetNeighbors ( node );
		if ( neighborNodes.Count == 0 )
		{
			return neighbors;
		}
		foreach ( Node n in neighborNodes )
		{
			neighbors.Add ( TileFromWorldPosition ( n.worldPosition ) );
		}
		return neighbors;
	}

	public static bool IsTileNeighbor ( Tile tile )
	{
		return GetNeighbors ( tile ).Contains ( tile );
	}
}

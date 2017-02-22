using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

	public bool DisplayGridGizmos;
	public LayerMask ImpassableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public TerrainType[] passableRegions;
	LayerMask passableMask;
	Dictionary<int, int> passableRegionsDict = new Dictionary<int, int> ();

	public Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake ()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);

		foreach (TerrainType region in passableRegions) {
			passableMask.value = passableMask |= region.terrainMask.value;
			passableRegionsDict.Add ((int)Mathf.Log (region.terrainMask.value, 2), region.terrainPenalty);
		}
		CreateGrid ();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	public List<Node> GetNeighbors (Node node)
	{
		List<Node> neighbors = new List<Node> ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbors.Add (grid [checkX, checkY]);
				} 
			}
		}
		return neighbors;
	}

	void CreateGrid ()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool passable = !(Physics.CheckSphere (worldPoint, nodeRadius, ImpassableMask));

				int movementPenalty = 0;

				if (passable) {
					Ray ray = new Ray (worldPoint + Vector3.up * 50, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 100, passableMask)) {
						passableRegionsDict.TryGetValue (hit.collider.gameObject.layer, out movementPenalty);	
					}
				}

				grid [x, y] = new Node (passable, worldPoint, x, y, movementPenalty);
			}
		}
	}

	public Node NodeFromWorldPoint (Vector3 worldPosition)
	{
//		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
//		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
//		percentX = Mathf.Clamp01 (percentX);
//		percentY = Mathf.Clamp01 (percentY);
//
//		int x = Mathf.RoundToInt ((gridSizeX - 1) * percentX);
//		int y = Mathf.RoundToInt ((gridSizeY - 1) * percentY);
		int x = (int)worldPosition.x;
		int y = (int)worldPosition.z;
		return grid [x, y];
	}

	public void SelectNode (Node node)
	{
		foreach (Node n in grid) {
			if (n != node) {
				n.selected = false;
			} else {
				n.selected = true;
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null && DisplayGridGizmos) {
			foreach (Node n in grid) {
				if (n.selected) {
					Gizmos.color = Color.black;	
				} else if (n.passable) {
					Gizmos.color = Color.white;
				} else {
					Gizmos.color = Color.red;
				}
				Gizmos.DrawWireCube (n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}

	[System.Serializable]
	public class TerrainType
	{
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}

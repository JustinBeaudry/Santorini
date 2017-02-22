using System;
using UnityEngine;

[Serializable]
public class Node : IHeapItem<Node>
{
	public bool selected = false;
	public bool passable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int movementPenalty;

	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;

	public Node (bool _passable, Vector3 _worldPosition, int _gridX, int _gridY, int _movementPenalty)
	{
		passable = _passable;
		worldPosition = _worldPosition;
		gridX = _gridX;
		gridY = _gridY;
		movementPenalty = _movementPenalty;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo (Node node)
	{
		int compare = fCost.CompareTo (node.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (node.hCost);
		}
		return -compare;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}
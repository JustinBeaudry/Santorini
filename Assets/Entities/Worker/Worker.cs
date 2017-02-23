using System;
using UnityEngine;

[Serializable]
public class Worker : MonoBehaviour
{
	public bool Alive = true;
	public Color Color = Color.white;
	public float Speed = 2f;
	public Tile CurrentTile;

	public Worker (Color color)
	{
		Alive = true;
		Color = color;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}
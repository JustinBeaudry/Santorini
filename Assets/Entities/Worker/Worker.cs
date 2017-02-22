using System;
using UnityEngine;

[Serializable]
public class Worker : MonoBehaviour
{
	public bool alive = true;
	public Color Color = Color.white;

	public Worker (Color color)
	{
		Color = color;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}
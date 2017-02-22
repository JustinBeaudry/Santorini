using System;
using UnityEngine;

[Serializable]
public class Tile : MonoBehaviour
{
	public enum Levels
	{
		Zero,
		One,
		Two,
		Three,
		Dome
	}

	public Levels Level = Levels.Zero;
	public Worker Worker = null;

	void Update ()
	{
		Transform worker = transform.FindChild ("Worker(Clone)");
		if (worker) {
			Worker = worker.GetComponent<Worker> ();	
		}
	}

	public bool HasWorker ()
	{
		return Worker != null;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}
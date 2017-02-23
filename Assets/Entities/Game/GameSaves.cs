using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaves
{
	public Dictionary<String, GameActionController> Index = new Dictionary<String, GameActionController> ();

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}

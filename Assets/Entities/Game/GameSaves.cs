using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaves
{
	public Dictionary<String, GameActionDispatch> Index = new Dictionary<String, GameActionDispatch> ();

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}

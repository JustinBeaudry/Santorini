using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileCollection
{
	public List<Profile> Index = new List<Profile> ();

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}
}

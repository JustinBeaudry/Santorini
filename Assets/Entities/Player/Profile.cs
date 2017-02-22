using System;
using UnityEngine;

[Serializable]
public class Profile : IEquatable<Profile>
{
	public String ID;
	public string PlayerName;
	public Color WorkerColor = Color.gray;

	public Profile (string playerName)
	{
		ID = Guid.NewGuid ().ToString ();
		PlayerName = playerName;
//		WorkerColor = workerColor;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}

	public bool Equals (Profile profile)
	{
		return ID.Equals (profile.ID);
	}
}
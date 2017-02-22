using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static List<Player> Players = new List<Player> ();

	void Awake ()
	{
		Object.DontDestroyOnLoad (gameObject);
	}

	public static void Init (List<Profile> profiles)
	{
		foreach (Profile profile in profiles) {
			Players.Add (new Player (profile.ID, profile.PlayerName, profile.WorkerColor));
		}
	}
}

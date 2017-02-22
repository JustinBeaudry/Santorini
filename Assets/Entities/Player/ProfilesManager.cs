using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilesManager : MonoBehaviour
{
	public static List<Profile> Profiles = new List<Profile> ();

	void Awake ()
	{
		Object.DontDestroyOnLoad (gameObject);
		Profiles.AddRange (PlayerPrefsManager.GetProfiles ());
	}
}
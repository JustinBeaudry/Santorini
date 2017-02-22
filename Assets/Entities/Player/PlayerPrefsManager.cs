using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{

	const string MUSIC_VOLUME_KEY = "music_volume";
	const string GAME_VOLUME_KEY = "game_volume";
	const string PROFILES_NAME_KEY = "profiles-name";
	const string PROFILES_COLOR_KEY = "profiles-color";

	public static void SetMusicVolume (float volume)
	{
		if (!(volume > 0f && volume < 1f)) {
			Debug.LogError ("Music volume out of range");
			return;
		}
		PlayerPrefs.SetFloat (MUSIC_VOLUME_KEY, volume);
	}

	public static float GetMusicVolume ()
	{
		return PlayerPrefs.GetFloat (MUSIC_VOLUME_KEY);
	}

	public static void SetGameVolume (float volume)
	{
		if (!(volume > 0f && volume < 1f)) {
			Debug.LogError ("Game volume out of range");
			return;
		}
		PlayerPrefs.SetFloat (GAME_VOLUME_KEY, volume);
	}

	public static float GetGameVolume ()
	{
		return PlayerPrefs.GetFloat (GAME_VOLUME_KEY);
	}

	/**
	 * PlayerPrefs only allows settings strings, ints, and floats
	 * This serializes a profile class into a joined string of other instances of profile class
	 *
	 */
	public static void SetProfile (Profile profile)
	{
		ProfileCollection profiles = JsonUtility.FromJson<ProfileCollection> (PlayerPrefs.GetString (PROFILES_NAME_KEY));
		if (profiles == null) {
			profiles = new ProfileCollection ();
		}
		profiles.Index.Add (profile);
		String JsonProfiles = JsonUtility.ToJson (profiles);
		PlayerPrefs.SetString (PROFILES_NAME_KEY, JsonProfiles);
		PlayerPrefs.Save ();
	}

	static ProfileCollection GetProfileCollection ()
	{
		return JsonUtility.FromJson<ProfileCollection> (PlayerPrefs.GetString (PROFILES_NAME_KEY));
	}

	public static List<Profile> GetProfiles ()
	{
		return GetProfileCollection ().Index;
	}

	public static void RemoveProfile (Profile profile)
	{
		ProfileCollection profiles = GetProfileCollection ();
		profiles.Index.Remove (profile);
		PlayerPrefs.SetString (PROFILES_NAME_KEY, JsonUtility.ToJson (profiles));
		PlayerPrefs.Save ();
	}
}

using UnityEngine;

[RequireComponent (typeof(PlayerPrefsManager))]
public class SettingsManager : MonoBehaviour
{

	public const float DEFUALT_MUSIC_VOLUME = 0.4f;
	public const float DEFAULT_GAME_VOLUME = 0.7f;

	public static float MusicVolume = DEFUALT_MUSIC_VOLUME;
	public static float GameVolume = DEFAULT_GAME_VOLUME;

	protected void Start ()
	{
		UpdateSettingsFromPrefs ();
	}

	public static void SetToDefaults ()
	{
		MusicVolume = DEFUALT_MUSIC_VOLUME;
		GameVolume = DEFAULT_GAME_VOLUME;
		SaveAll ();
	}

	public static void SaveAll ()
	{
		SaveMusicVolume ();
		SaveGameVolume ();
	}

	public static void SaveMusicVolume ()
	{
		PlayerPrefsManager.SetMusicVolume (MusicVolume);
	}

	public static void SaveGameVolume ()
	{
		PlayerPrefsManager.SetGameVolume (GameVolume);
	}


	private static void UpdateSettingsFromPrefs ()
	{
		MusicVolume = PlayerPrefsManager.GetMusicVolume ();
		GameVolume = PlayerPrefsManager.GetGameVolume ();
	
	}

}

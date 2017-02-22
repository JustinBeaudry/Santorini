using UnityEngine;

[System.Serializable]
[RequireComponent (typeof(SettingsManager))]
public class MusicPlayer : MonoBehaviour
{

	static MusicPlayer musicPlayer;
	private AudioSource audioSrc;
	private string currentSongName;

	public float volume = 0.0f;
	public AudioMap[] audioMap;

	protected void Awake ()
	{
		musicPlayer = this;
	}

	protected void Start ()
	{
		// Prevent MusicPlayer from being destroyed on Scene Changes
		DontDestroyOnLoad (this);

		musicPlayer.audioSrc = GetComponent<AudioSource> ();

		// Hack to set the 3d distance everywhere, so 2d sound can be heard everywhere
		// What are the performance implications of this?
		// @TODO:  There will be times when 3d sound is necessary, fix this as a variable setting
		musicPlayer.audioSrc.rolloffMode = AudioRolloffMode.Linear;
		musicPlayer.audioSrc.maxDistance = float.MaxValue;
		musicPlayer.audioSrc.volume = SettingsManager.MusicVolume;
	}

	protected void Update ()
	{
		if (musicPlayer.audioSrc != null && !Mathf.Approximately (musicPlayer.audioSrc.volume, SettingsManager.MusicVolume)) {
			musicPlayer.audioSrc.volume = SettingsManager.MusicVolume;
		}
	}

	public void ChangeAudio (string key)
	{
		int index = SceneHasAudio (key);
		AudioMap map;
		if (index > -1) {
			map = musicPlayer.audioMap [index];
			if (musicPlayer.currentSongName != map.Clip.name) {
				musicPlayer.currentSongName = map.Clip.name;
				musicPlayer.audioSrc.clip = map.Clip;
				musicPlayer.audioSrc.loop = map.Loop;
				musicPlayer.audioSrc.Play ();
			}

		} else {
			Debug.LogError ("[INFO] MusicPlayer - ChangeAudio() resource " + key + " not found");
		}
	}

	public void StopAudio ()
	{
		audioSrc.Stop ();
	}


	protected void OnDestroy ()
	{
		musicPlayer = null;
	}

	protected void FadeOut ()
	{
		if (musicPlayer.volume > 0.1f) {
			musicPlayer.volume -= 0.1f * Time.deltaTime;
			musicPlayer.audioSrc.volume = musicPlayer.volume;
		}	
	}

	protected void FadeIn ()
	{
		if (musicPlayer.volume < 1f) {
			musicPlayer.volume += 0.1f * Time.deltaTime;
			musicPlayer.audioSrc.volume = musicPlayer.volume;
		}	
	}

	[System.Serializable]
	public class AudioMap
	{
		public string Key;
		public AudioClip Clip;
		public bool Loop;
	}

	private int SceneHasAudio (string key)
	{
		int _audio = -1;
		for (int i = 0; i < musicPlayer.audioMap.Length; i++) {
			if (musicPlayer.audioMap [i].Key == key) {
				_audio = i;
			}
		}
		return _audio;
	}
}
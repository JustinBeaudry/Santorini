using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(SettingsManager))]
public class SettingsController : MonoBehaviour
{
	public Slider MusicVolumeSlider, GameVolumeSlider;
	public Button DefaultsButton, BackButton;

	void Start ()
	{
		// @NOTE:  This scene is additively loaded to other scenes
//		UpdateUI ();
		BindActionHandlers ();
	}

	private void BindActionHandlers ()
	{
//		MusicVolumeSlider.onValueChanged.AddListener (delegate {
//			OnMusicVolumeChange (MusicVolumeSlider.value);
//		});
//
//		GameVolumeSlider.onValueChanged.AddListener (delegate {
//			OnGameVolumeChange (GameVolumeSlider.value);
//		});
//
//		DefaultsButton.onClick.AddListener (OnDefault);
		BackButton.onClick.AddListener (OnBack);
	}

	private void UpdateUI ()
	{
		MusicVolumeSlider.value = SettingsManager.MusicVolume;
		GameVolumeSlider.value = SettingsManager.GameVolume;
	}

	private void OnDefault ()
	{
		SettingsManager.SetToDefaults ();
		UpdateUI ();

	}

	private void OnBack ()
	{
		SettingsManager.SaveAll ();
		GameManager.UnloadSceneAdditive ("Settings");
	}

	private void OnMusicVolumeChange (float value)
	{
		SettingsManager.MusicVolume = value;
	}

	private void OnGameVolumeChange (float value)
	{
		SettingsManager.GameVolume = value;
	}
}

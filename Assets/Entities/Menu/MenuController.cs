using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

	public Button PlayButton, SettingsButton, QuitButton;

	// Use this for initialization
	void Start ()
	{
		GameManager.InitGame ();
		BindActionHandlers ();
	}

	void BindActionHandlers ()
	{
		PlayButton.onClick.AddListener (OnStart);
		SettingsButton.onClick.AddListener (OnSettings);
//		QuitButton.onClick.AddListener (OnQuit);
	}

	void OnStart ()
	{
		GameManager.SwitchScene ("Match Settings");
	}

	void OnSettings ()
	{
		GameManager.LoadSceneAdditive ("Settings");
	}

	void OnQuit ()
	{
		Application.Quit ();
	}
}

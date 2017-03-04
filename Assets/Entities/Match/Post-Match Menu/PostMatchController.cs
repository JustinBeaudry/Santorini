using UnityEngine;
using UnityEngine.UI;

public class PostMatchController : MonoBehaviour
{

	public Button PlayButton, MainMenuButton, QuitButton;
	public Text WinningPlayerText;

	// Use this for initialization
	void Start ()
	{
		GameManager.InitGame ();
		BindActionHandlers ();
	}

	void BindActionHandlers ()
	{
		PlayButton.onClick.AddListener ( OnStart );
		MainMenuButton.onClick.AddListener ( OnMainMenu );
		QuitButton.onClick.AddListener ( OnQuit );
		WinningPlayerText.text = PlayerManager.WinningPlayer.Name + " Wins!";
	}

	void OnMainMenu ()
	{
		GameManager.SwitchScene ( "Menu" );
	}

	void OnStart ()
	{
		PlayerManager.Players.Clear ();
		PlayerManager.Init ( ProfilesManager.Profiles );
		GameManager.SwitchScene ( "Match" );
	}

	void OnQuit ()
	{
		Application.Quit ();
	}
}

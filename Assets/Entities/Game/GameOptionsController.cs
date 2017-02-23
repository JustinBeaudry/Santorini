using UnityEngine;
using UnityEngine.UI;

public class GameOptionsController : MonoBehaviour
{
	public Button SaveBtn, LoadBtn, BackBtn;

	void Awake ()
	{
		SaveBtn.onClick.AddListener (OnSave);
		LoadBtn.onClick.AddListener (OnLoad);
		BackBtn.onClick.AddListener (OnBack);
	}

	void OnSave ()
	{
		
	}

	void OnLoad ()
	{
		
	}

	void OnBack ()
	{
		GameManager.UnloadSceneAdditive ("Game Options");
	}
}
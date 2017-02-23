using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsController : MonoBehaviour
{
	public Button SaveBtn, LoadBtn, BackBtn, BackToOptionsBtn;
	public GameObject LoadContainer, SavesContainer, OptionsContainer, SaveTemplate;

	void Start ()
	{
		BindEventHandlers ();
	}

	void BindEventHandlers ()
	{
		SaveBtn.onClick.AddListener (OnSave);
		LoadBtn.onClick.AddListener (OnLoad);
		BackBtn.onClick.AddListener (OnBack);
	}

	void OnSave ()
	{
		PlayerPrefsManager.SaveGame (System.DateTime.Now.ToShortDateString () + System.DateTime.Now.ToShortDateString (), GameActionController.gameActionController);
	}

	void OnLoad ()
	{
		OptionsContainer.SetActive (false);
		LoadContainer.SetActive (true);
		BackToOptionsBtn.onClick.AddListener (delegate {
			LoadContainer.SetActive (false);
			OptionsContainer.SetActive (true);
		});
		RenderSaves ();
	}

	void RenderSaves ()
	{
		float offset = 133f;
		Dictionary<string, GameActionController> saves = PlayerPrefsManager.GetSaves ();
		foreach (string key in saves.Keys) {
			GameObject obj = Instantiate (SaveTemplate, SavesContainer.transform);
			obj.transform.localPosition = new Vector3 (0, offset, 0);
			offset -= obj.GetComponent<RectTransform> ().rect.height + 0.5f;
			obj.GetComponentInChildren<Text> ().text = key;
			Button btn = obj.GetComponent<Button> ();
			btn.onClick.AddListener (delegate {
				Debug.Log (saves [key]);
			});
		}
	}

	void OnBack ()
	{
		MatchController.SettingsOpen = false;
		GameManager.UnloadSceneAdditive ("Game Options");
	}
}
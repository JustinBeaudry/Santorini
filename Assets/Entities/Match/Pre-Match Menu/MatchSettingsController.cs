using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSettingsController : MonoBehaviour
{

	public Button StartMatch, AddProfile, BackButton;
	public InputField PlayerName;
	public Text EmptyPlayersText;
	public Text EmptyProfilesText;
	public GameObject ProfilesContainer;
	public GameObject PlayersContainer;
	public GameObject ProfileTemplate;
	public Scrollbar ProfileScroll;

	public List<Profile> profiles = new List<Profile> ();
	public List<Profile> selected = new List<Profile> ();

	void Awake ()
	{
		GameManager.InitGame ();
		BindActionHandlers ();	
		LoadExistingProfiles ();
		RenderProfiles ();
	}

	void Update ()
	{
		// @TODO  update to support 3-4 players
		StartMatch.interactable = selected.Count == 2;
		if (PlayerName.text.Length > 0 && Input.GetKey (KeyCode.Return)) {
			OnAddProfile ();
		}
	}

	void BindActionHandlers ()
	{
		StartMatch.onClick.AddListener (OnStartMatch);
		AddProfile.onClick.AddListener (OnAddProfile);
		BackButton.onClick.AddListener (OnBack);
	}

	void LoadExistingProfiles ()
	{
		profiles.AddRange (ProfilesManager.Profiles);
	}

	void ClearProfiles ()
	{
		for (int i = ProfilesContainer.transform.childCount - 1; i >= 0; i--) {
			Destroy (ProfilesContainer.transform.GetChild (i).gameObject);
		}
		for (int i = PlayersContainer.transform.childCount - 1; i >= 0; i--) {
			Destroy (PlayersContainer.transform.GetChild (i).gameObject);
		}
	}

	void RenderProfiles ()
	{
		ClearProfiles ();
		float offset = 125f;
		float selectedOffset = 110f;
		if (profiles.Count == 0) {
			Text text = Instantiate (EmptyProfilesText, ProfilesContainer.transform);
			text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, 80f);
			ProfileScroll.interactable = false;
		} else {
			ProfileScroll.interactable = true;
			foreach (Profile profile in profiles) {
				GameObject obj = Instantiate (ProfileTemplate, ProfilesContainer.transform);
				RectTransform rect = obj.GetComponent<RectTransform> ();
				Text text = obj.GetComponentInChildren<Text> ();
				Button selectBtn = obj.transform.Find ("Select Profile Button").GetComponent<Button> ();
				Button removeBtn = obj.transform.Find ("Remove Profile Button").GetComponent<Button> ();
				text.text = profile.PlayerName;
				selectBtn.onClick.AddListener (delegate {
					if (selected.Contains (profile)) {
						selected.Remove (profile);
						RenderProfiles ();
						return;
					}
					if (selected.Count == 2) {
						return;
					}
					selected.Add (profile);
					RenderProfiles ();
				});
				removeBtn.onClick.AddListener (delegate {
					selected.Remove (profile);
					PlayerPrefsManager.RemoveProfile (profile);
					RenderProfiles ();
				});
				rect.anchoredPosition = new Vector2 (0f, offset);
				offset -= (rect.rect.height + 5f);
			}
		}
		if (selected.Count == 0) {
			Text text = Instantiate (EmptyPlayersText, PlayersContainer.transform);
			text.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		} else {
			foreach (Profile profile in selected) {
				GameObject obj = Instantiate (ProfileTemplate, PlayersContainer.transform);
				RectTransform rect = obj.GetComponent<RectTransform> ();
				Text text = obj.GetComponentInChildren<Text> ();
				Button selectBtn = obj.transform.Find ("Select Profile Button").GetComponent<Button> ();
				Button removeBtn = obj.transform.Find ("Remove Profile Button").GetComponent<Button> ();
				text.text = profile.PlayerName;
				selectBtn.interactable = false;
				removeBtn.onClick.AddListener (delegate {
					selected.Remove (profile);
					RenderProfiles ();
				});
				rect.anchoredPosition = new Vector2 (0f, selectedOffset);
				selectedOffset -= (rect.rect.height + 5f);	
			}
		}
	}

	void OnAddProfile ()
	{
		if (PlayerName.text == "") {
			return;
		}
		Profile profile = new Profile (PlayerName.text);
		profiles.Add (profile);
		PlayerPrefsManager.SetProfile (profile);
		PlayerName.text = "";
		RenderProfiles ();
	}

	void OnStartMatch ()
	{
		// now that we have some profiles ready, let's create player instances for those profiles
		PlayerManager.Init (selected);
		GameManager.SwitchScene ("Match", true);
	}

	void OnBack ()
	{
		GameManager.SwitchScene ("Menu", false);
	}
}

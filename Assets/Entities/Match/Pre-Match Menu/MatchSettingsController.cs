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
	public GameObject PlayersObject;
	public GameObject ProfileUITemplate;
	public GameObject ColorPicker;
	public Scrollbar ProfileScroll;

	public List<Profile> profiles = new List<Profile> ();
	public List<Profile> selected = new List<Profile> ();

	private Profile currentProfile;
	private ProfileUI currentObj;

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
		AddProfile.onClick.AddListener (OnAddProfile);
		StartMatch.onClick.AddListener (OnStartMatch);
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

		const float padding = 5f;
		Offset profileOffset = new Offset (120f);
		Offset selectedOffset = new Offset (105f);

		if (profiles.Count == 0) {
			Text text = Instantiate (EmptyProfilesText, ProfilesContainer.transform);
			text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, 80f);
			ProfileScroll.interactable = false;
		} else {
			ProfileScroll.interactable = true;
			foreach (Profile profile in profiles) {
				ProfileUI obj = new ProfileUI (Instantiate (ProfileUITemplate, ProfilesContainer.transform), profileOffset, padding);
				SetCurrent (profile, obj);
				RenderProfileData ();

				obj.selectBtn.onClick.AddListener (delegate {
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
				obj.removeBtn.onClick.AddListener (delegate {
					profiles.Remove (profile);
					selected.Remove (profile);
					PlayerPrefsManager.RemoveProfile (profile);
					RenderProfiles ();
				});
				obj.setColorBtn.onClick.AddListener (delegate {
					SetCurrent (profile, obj);

					// Hack color picker to have the existing color selected
//					GameObject presetColorPicker = ColorPicker.transform.FindChild ("PresetColorPicker").gameObject;
//					InputField R = presetColorPicker.transform.FindChild ("InputField_R").GetComponent<InputField> ();
//					InputField G = presetColorPicker.transform.FindChild ("InputField_G").GetComponent<InputField> ();
//					InputField B = presetColorPicker.transform.FindChild ("InputField_B").GetComponent<InputField> ();
//					R.text = profile.WorkerColor.r.ToString ();
//					G.text = profile.WorkerColor.g.ToString ();
//					B.text = profile.WorkerColor.b.ToString ();

					PlayersObject.SetActive (false);
					ColorPicker.SetActive (true);

					Button saveBtn = ColorPicker.transform.FindChild ("Save Button").GetComponent<Button> ();
					saveBtn.onClick.AddListener (delegate {
						PlayerPrefsManager.UpdateProfile (currentProfile);
						ColorPicker.SetActive (false);	
						PlayersObject.SetActive (true);
						RenderProfiles ();
					});
					Button cancelBtn = ColorPicker.transform.FindChild ("Cancel Button").GetComponent<Button> ();
					cancelBtn.onClick.AddListener (delegate {
						ColorPicker.SetActive (false);
						PlayersObject.SetActive (true);
					});
				});
			}
		}
		if (selected.Count == 0) {
			Text text = Instantiate (EmptyPlayersText, PlayersContainer.transform);
			text.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		} else {
			foreach (Profile profile in selected) {
				ProfileUI obj = new ProfileUI (Instantiate (ProfileUITemplate, PlayersContainer.transform), selectedOffset, padding);
				SetCurrent (profile, obj);
				RenderProfileData ();

				obj.selectBtn.interactable = false;
				obj.setColorBtn.interactable = false;
				obj.removeBtn.onClick.AddListener (delegate {
					SetCurrent (profile, obj);
					selected.Remove (profile);
					RenderProfiles ();
				});
			}
		}
	}

	void RenderProfileData ()
	{
		if (currentObj != null && currentProfile != null) {
			currentObj.SetText (currentProfile.PlayerName);
			currentObj.SetColor (currentProfile.WorkerColor);	
		}
	}

	void SetCurrent (Profile profile, ProfileUI obj)
	{
		currentProfile = profile;
		currentObj = obj;
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

	public void OnSetColor (Color color)
	{
		currentProfile.WorkerColor = color;
		RenderProfileData ();
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

	private class Offset
	{
		public float value;

		public Offset (float offset)
		{
			value = offset;
		}
	}

	private class ProfileUI
	{
		public RectTransform rect;
		public Text text;
		public Button selectBtn;
		public Button removeBtn;
		public Button setColorBtn;

		public override string ToString ()
		{
			return JsonUtility.ToJson (this);
		}

		public ProfileUI (GameObject obj, Offset offset, float padding)
		{
			rect = obj.GetComponent<RectTransform> ();
			text = obj.GetComponentInChildren<Text> ();
			selectBtn = obj.transform.FindChild ("Select Profile Button").GetComponent<Button> ();
			removeBtn = obj.transform.FindChild ("Remove Profile Button").GetComponent<Button> ();
			setColorBtn = obj.transform.FindChild ("Color Picker Button").GetComponent<Button> ();

			rect.anchoredPosition = new Vector2 (0f, offset.value);
			offset.value -= (rect.rect.height + padding);
		}

		public void SetText (string txt)
		{
			text.text = txt;
		}

		public void SetColor (Color color)
		{
			ColorBlock colorBlock = ColorBlock.defaultColorBlock;
			colorBlock.normalColor = color;
			colorBlock.disabledColor = color;
			colorBlock.highlightedColor = new Color (color.r, color.g, color.b, 0.5f);
			colorBlock.pressedColor = color;
			setColorBtn.colors = colorBlock;
		}
	}
}

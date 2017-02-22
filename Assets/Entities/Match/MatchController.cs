using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

public class MatchController : MonoBehaviour
{

	public int round = 0;
	public GameObject Worker;
	public Text PlayerText;
	public Text RoundText;
	public Text TimeText;
	public Text ActionText;

	public enum ActionStates
	{
		idle,
		running,
		done
	}

	public ActionStates ActionState = ActionStates.idle;

	private static MatchController matchController;
	private Grid grid;
	private Queue<GameAction> GameActions = new Queue<GameAction> ();
	private bool DisplayTime = true;
	private bool SettingPlayerWorkers = true;
	private Player currentPlayer;
	private int currentRound;
	private float roundTime;
	private string playerText;
	private string actionText;

	public static void SetActionState (ActionStates actionState)
	{
		matchController.ActionState = actionState;
	}

	void Awake ()
	{
		GameManager.InitGame ();
		matchController = this;
		grid = GameObject.Find ("GridManager").GetComponent<Grid> ();
	}

	void Start ()
	{
		foreach (Node n in grid.grid) {
			TileManager.CreateTileFromWorldPosition (new Vector3 ((float)n.gridX + grid.nodeRadius, 0f, (float)n.gridY + grid.nodeRadius));	
		}
		currentRound = 1;
		DisplayTime = false;
		KeyboardMouseController.OnClicked += SetWorker;
		foreach (Player player in PlayerManager.Players) {
			GameActions.Enqueue (new GameAction (player, new PlayerAction ("Set Workers", "Set " + player.MaxWorkers + " Workers.", delegate {
			})));
		}
	}

	void Update ()
	{
		roundTime = Time.timeSinceLevelLoad;
		float minutes = Mathf.Floor (roundTime / 60);
		float seconds = Mathf.Floor (roundTime > minutes * 60 ? roundTime - (minutes * 60) : roundTime);
		if (DisplayTime) {
			TimeText.text = minutes + ":" + seconds.ToString ("0#");
		}
		PlayerText.text = playerText;
		RoundText.text = currentRound.ToString ();
		ActionText.text = actionText;

//		CheckWinConditions();
		if (GameActions.Count == 0 && ActionState != ActionStates.running) {
			if (SettingPlayerWorkers) {
				KeyboardMouseController.OnClicked -= SetWorker;
				DisplayTime = true;
				SettingPlayerWorkers = false;
			} else {
				currentRound++;
			}
			SetPlayerActions ();
		}
		if (GameActions.Count > 0 && ActionState != ActionStates.running) {
			ActionState = ActionStates.running;
			GameAction gameAction = GameActions.Dequeue ();	
			currentPlayer = gameAction.player;
			playerText = gameAction.player.Name;
			actionText = gameAction.playerAction.ActionText;
			gameAction.playerAction.Action ();
		}
	}

	void SetPlayerActions ()
	{
		foreach (Player player in PlayerManager.Players) {
			Debug.Log ("Setting PlayerActions: " + player.PlayerActions.Count);
			foreach (PlayerAction playerAction in player.PlayerActions) {
				GameActions.Enqueue (new GameAction (player, playerAction));	
			}
			Debug.Log ("Set PlayerActions: " + player.PlayerActions.Count);
		}
	}

	void SetWorker (Tile tile)
	{
		if (tile != null && !tile.HasWorker ()) {
			GameObject worker = Instantiate (Worker, tile.transform);
			worker.transform.localPosition = new Vector3 (0, 0.25f, 0);
			currentPlayer.Workers.Add (worker.GetComponent<Worker> ());
			Debug.Log (currentPlayer.Workers);
		}
		if (currentPlayer.Workers.Count == 2) {
			ActionState = ActionStates.done;
		}
	}

	void CheckWinConditions ()
	{
		foreach (Player player in PlayerManager.Players) {
			foreach (WinCondition condition in player.WinConditions) {
				if (condition.DoCheck ()) {
					Debug.Log ("Win!");
				}
			}
		}	
	}
}

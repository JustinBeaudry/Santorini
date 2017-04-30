using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameActionController : MonoBehaviour
{
	public String Version = "1.0.0";
	public static GameActionController gameActionController;

	public List<GameAction> Index = new List<GameAction> ();

	public enum ActionStates
	{
		idle,
		running
	}

	public ActionStates ActionState = ActionStates.idle;

	public int ActionCursor = -1;

	public static GameAction CurrentGameAction {
		get {
			if (gameActionController.CheckActionCursor()) {
				return null;
			}
			return gameActionController.Index[gameActionController.ActionCursor];
		}
	}

	public static GameAction PrevGameAction {
		get {
			if (gameActionController.CheckActionCursor(1)) {
				return null;
			}
			return gameActionController.Index[gameActionController.ActionCursor - 1];
		}
	}

	public static Player CurrentPlayer {
		get {
			GameAction gameAction = CurrentGameAction;
			if ( gameAction == null )
			{
				return null;			
			}
			return gameAction.player;
		}
	}

	void Awake ()
	{
		gameActionController = this;
	}

	public static void AddAction ( GameAction gameAction )
	{
		gameActionController.Index.Add( gameAction );
	}

	public static void NextAction ()
	{
		SetActionRun ();
		gameActionController.ActionCursor++;
	}

	public static void UndoAction() {
		// A player can undo up until "Select Worker"
		if (CurrentGameAction.playerAction.ActionText != "Select Worker") {
			gameActionController.ActionCursor--;
			CurrentPlayer.CurrentWorker = CurrentGameAction.player.CurrentWorker;
			
			SetActionRun();
		}
	}

	public static bool HasGameActions ()
	{
		return gameActionController.ActionCursor < (gameActionController.Index.Count - 1);
	}

	public static bool IsRunning ()
	{
		return gameActionController.ActionState == ActionStates.running;
	}

	public static bool IsIdle ()
	{
		return gameActionController.ActionState == ActionStates.idle;
	}

	public static void SetActionIdle ()
	{
		gameActionController.ActionState = ActionStates.idle;
	}

	public static void SetActionRun ()
	{
		gameActionController.ActionState = ActionStates.running;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson ( this );
	}

	private bool CheckActionCursor ( int index = 0 )
	{
		return gameActionController.ActionCursor == -1 
			|| Index.Count == 0
			|| ( gameActionController.ActionCursor - index ) < 0
			|| ( gameActionController.ActionCursor - index ) > (Index.Count - 1);
	}
}

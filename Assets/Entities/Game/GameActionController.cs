using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameActionController : MonoBehaviour
{
	public String Version = "1.0.0";
	public static GameActionController gameActionController;

	public enum ActionStates
	{
		idle,
		running
	}

	public ActionStates ActionState = ActionStates.idle;

	public List<GameAction> History = new List<GameAction> ();
	public int ActionCursor = -1;

	public static GameAction CurrentGameAction {
		get {
			return gameActionController.CheckActionCursor () ? null : gameActionController.History [gameActionController.ActionCursor];
		}
	}

	public static GameAction PrevGameAction {
		get {
			return gameActionController.CheckActionCursor (-1) ? null : gameActionController.History [gameActionController.ActionCursor - 1];
		}
	}

	public static Player CurrentPlayer {
		get {
			GameAction gameAction = CurrentGameAction;
			if (gameAction == null) {
				return null;			
			}
			return gameAction.player;
		}
	}

	public Queue<GameAction> Index = new Queue<GameAction> ();

	void Awake ()
	{
		gameActionController = this;
	}

	public static void AddAction (GameAction gameAction)
	{
		gameActionController.Index.Enqueue (gameAction);
	}

	public static GameAction NextAction ()
	{
		SetActionRun ();
		GameAction gameAction = gameActionController.Index.Dequeue ();	
		gameActionController.History.Add (gameAction);
		gameActionController.ActionCursor++;
		return gameAction;
	}

	public static bool HasGameActions ()
	{
		return gameActionController.Index.Count > 0;
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
		return JsonUtility.ToJson (this);
	}

	private bool CheckActionCursor (int index = 0)
	{
		return ActionCursor == -1 || History.Count == 0 || (ActionCursor - index) < 0 || (ActionCursor - index) > History.Count;
	}
}

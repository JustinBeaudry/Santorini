using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameActionController : MonoBehaviour
{
  public String Version = "1.0.0";
  public static GameActionController gameActionController;

  public static List<GameAction> Index = new List<GameAction>();

  public enum ActionStates
  {
    idle,
    running
  }

  public ActionStates ActionState = ActionStates.idle;

  public static int ActionCursor = -1;

  public static Boolean Undoing = false;

  public static GameAction CurrentGameAction
  {
    get
    {
      if (CheckActionCursor())
      {
        return null;
      }
      return Index[ActionCursor];
    }
  }

  public static GameAction PrevGameAction
  {
    get
    {
      if (CheckActionCursor(1))
      {
        return null;
      }
      return Index[ActionCursor - 1];
    }
  }

  public static GameAction NextGameAction
  {
    get
    {
      if (CheckActionCursor(-1))
      {
        return null;
      }
      return Index[ActionCursor + 1];
    }
  }

  public static Player CurrentPlayer
  {
    get
    {
      if (CurrentGameAction == null)
      {
        return null;
      }
      return CurrentGameAction.player;
    }
  }

  void Awake()
  {
    gameActionController = this;
  }

  public static void AddAction(GameAction gameAction)
  {
    Index.Add(gameAction);
  }

  public static void NextAction()
  {
    SetActionRun();
    ActionCursor++;
  }

  public static void UndoAction()
  {
    Debug.Log("UndoAction()");
    Debug.Log("Player: " + CurrentPlayer.Name);
    Debug.Log("Previous Player: " + PrevGameAction.player.Name);
    // A player can only undo until the beginning of his/her turn
    if (PrevGameAction.player == CurrentPlayer)
    {
      Debug.Log("ActionCursor: " + ActionCursor);
      Undoing = true;
      SetActionRun();
      ActionCursor--;
      Tile prevTile = CurrentGameAction.prevTile.Clone() as Tile;
      CurrentGameAction.currTile = null;
      CurrentGameAction.prevTile = null;
      // Reverse tiles from acton to undo
      CurrentGameAction.playerAction.Action(prevTile);
    }
  }

  public static bool HasGameActions()
  {
    return ActionCursor < (Index.Count - 1);
  }

  public static void GameActionComplete()
  {
    SetActionIdle();
    ControlDevice.RemoveInteraction(CurrentGameAction.playerAction.Action);

  }

  public static bool IsRunning()
  {
    return gameActionController.ActionState == ActionStates.running;
  }

  public static bool IsIdle()
  {
    return gameActionController.ActionState == ActionStates.idle;
  }

  public static void SetActionIdle()
  {
    gameActionController.ActionState = ActionStates.idle;
  }

  public static void SetActionRun()
  {
    gameActionController.ActionState = ActionStates.running;
  }

  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }

  private static bool CheckActionCursor(int index = 0)
  {
    return ActionCursor == -1 || Index.Count == 0 || (ActionCursor - index) < 0
        || (ActionCursor - index) > (Index.Count - 1);
  }
}

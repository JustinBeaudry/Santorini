using System;
using System.Collections.Generic;

public struct GameActionDispatch : ICloneable
{
  public String Version;
  public List<GameAction> Index;
  public int ActionCursor;
  public enum ActionStates
  {
    Idle,
    Running,
    Undoing
  }
  public ActionStates ActionState;

  public void Initialize()
  {
    Version = "1.0.0";
    Index = new List<GameAction>();
    ActionState = ActionStates.Idle;
    ActionCursor = -1;
  }

  public GameAction CurrentGameAction
  {
    get
    {
      return Index[ActionCursor];
    }
  }

  public GameAction PrevGameAction
  {
    get
    {
      return Index[ActionCursor - 1];
    }
  }

  public void AddAction(GameAction gameAction)
  {
    Index.Add(gameAction);
  }

  public void NextAction()
  {
    SetActionRun();
    ActionCursor++;
  }

  public void UndoAction()
  {
    if (PrevGameAction.Player == CurrentGameAction.Player)
    {
      SetActionUndo();
      ActionCursor--;
      Tile startTile = CurrentGameAction.StartTile.Clone() as Tile;
      CurrentGameAction.Clear();
    }
  }

  public static void InteractDispatch(Tile tile, GameAction gameAction)
  {
    // Get the worker to do work on
    Worker worker = gameAction.Player.CurrentWorker;
    if (worker != null)
    {
      gameAction.EndTile = tile;
      gameAction.StartTile = worker.CurrentTile;
    }
    gameAction.PlayerAction.Action(tile, gameAction);
  }

  public void Complete()
  {
    SetActionIdle();
  }

  public object Clone()
  {
    return (GameActionDispatch)this.MemberwiseClone();
  }

  private bool IsIdle()
  {
    return ActionState == ActionStates.Idle;
  }
  private bool IsRunning()
  {
    return ActionState == ActionStates.Running;
  }
  private bool IsUndo()
  {
    return ActionState == ActionStates.Undoing;
  }
  private void SetActionIdle()
  {
    ActionState = ActionStates.Idle;
  }
  private void SetActionRun()
  {
    ActionState = ActionStates.Running;
  }
  private void SetActionUndo()
  {
    ActionState = ActionStates.Undoing;
  }
}

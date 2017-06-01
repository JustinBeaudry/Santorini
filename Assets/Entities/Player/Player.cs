using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class Player : IEquatable<Player>
{
  public string ID;
  public string Name;
  public Color WorkerColor;
  public int MaxWorkers = 2;
  public List<Worker> Workers = new List<Worker>();
  public List<WinCondition> WinConditions = new List<WinCondition>();
  public List<PlayerAction> PlayerActions = new List<PlayerAction>();
  public Worker CurrentWorker;
  public enum PlayerStates
  {
    active,
    inactive
  }
  public PlayerStates PlayerState = PlayerStates.active;

  List<PlayerAction> DefaultGameActions = new List<PlayerAction>();
  List<WinCondition> DefaultWinConditions = new List<WinCondition>();
  Grid grid;

  public void Awake()
  {
    LoadGameActions();
  }

  private void LoadGameActions()
  {
    object DefaultGameActions = IO.ReadFile("DefaultActions.json");
  }

  public Player(string id, string name, Color workerColor, List<PlayerAction> playerActions = null, List<WinCondition> winConditions = null)
  {
    // establish default game actions
    // these will be used until the Pantheon class is completed and players can be assigned God Cards and Powers
    PlayerAction select = new PlayerAction("Select Worker", "Select a worker to perform actions with", SelectWorker);
    PlayerAction move = new PlayerAction("Move", "Move worker by one space", Move);
    PlayerAction build = new PlayerAction("Build", "Build tower by one level by one space", Build);
    DefaultGameActions.Add(select);
    DefaultGameActions.Add(move);
    DefaultGameActions.Add(build);

    // establish default win conditions
    // there is only one default
    WinCondition win = new WinCondition();
    DefaultWinConditions.Add(win);

    ID = id;
    Name = name;
    WorkerColor = workerColor;
    if (playerActions == null)
    {
      playerActions = DefaultGameActions;
    }
    PlayerActions.AddRange(playerActions);
    if (winConditions == null)
    {
      winConditions = DefaultWinConditions;
    }
    WinConditions.AddRange(winConditions);
  }

  public void SetInactive()
  {
    PlayerState = PlayerStates.inactive;
  }

  public bool IsInactive()
  {
    return PlayerState == PlayerStates.inactive;
  }

  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }

  public bool Equals(Player player)
  {
    return ID.Equals(player.ID);
  }

  public bool IsWorkerOwner(Worker worker)
  {
    return Workers.Contains(worker);
  }
}
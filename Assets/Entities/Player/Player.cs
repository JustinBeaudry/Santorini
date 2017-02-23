using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : IEquatable<Player>
{
	public string ID;
	public string Name;
	public Color WorkerColor;
	public int MaxWorkers = 2;
	public List<Worker> Workers = new List<Worker> ();
	public List<WinCondition> WinConditions = new List<WinCondition> ();
	public List<PlayerAction> PlayerActions = new List<PlayerAction> ();
	public Worker CurrentWorker;

	List<PlayerAction> DefaultGameActions = new List<PlayerAction> ();
	List<WinCondition> DefaultWinConditions = new List<WinCondition> ();

	public Player (string id, string name, Color workerColor, List<PlayerAction> playerActions = null, List<WinCondition> winConditions = null)
	{
		// establish default game actions
		// these will be used until the Pantheon class is completed and players can be assigned God Cards and Powers
		PlayerAction select = new PlayerAction ("Select Worker", "Select a worker to perform actions with", delegate {
			KeyboardMouseController.OnClicked += SelectWorker;
		});
		PlayerAction move = new PlayerAction ("Move", "Move worker by one space", delegate {
			KeyboardMouseController.OnClicked += Move;
		});
		PlayerAction build = new PlayerAction ("Build", "Build tower by one level by one space", delegate {
			KeyboardMouseController.OnClicked += Build;
		});
		DefaultGameActions.Add (select);
		DefaultGameActions.Add (move);
		DefaultGameActions.Add (build);

		// establish default win conditions
		// there is only one default
		WinCondition win = new WinCondition ();
		DefaultWinConditions.Add (win);

		ID = id;
		Name = name;
		WorkerColor = workerColor;
		if (playerActions == null) {
			playerActions = DefaultGameActions;
		}
		PlayerActions.AddRange (playerActions);
		if (winConditions == null) {
			winConditions = DefaultWinConditions;
		}
		WinConditions.AddRange (winConditions);
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson (this);
	}

	public bool Equals (Player player)
	{
		return ID.Equals (player.ID);
	}

	public bool IsWorkerOwner (Worker worker)
	{
		return Workers.Contains (worker);
	}

	public void SelectWorker (Tile tile)
	{
		if (tile != null && tile.Worker != null && tile.HasWorker () && this.IsWorkerOwner (tile.Worker)) {
			CurrentWorker = tile.Worker;
			KeyboardMouseController.OnClicked -= SelectWorker;
			GameActionController.SetActionIdle ();
		}
	}

	public void Move (Tile tile)
	{
		if (tile != null && !tile.HasWorker ()) {
			float offset = 0.25f;
			int diff = CurrentWorker.CurrentTile.DiffLevelsByInt (tile);
			if (tile.HasTower ()) {
				if (diff > 1) {
					return;
				}
				offset = tile.LevelAsOffset ();
			}
			Vector3 target = tile.transform.position;
			CurrentWorker.transform.position = new Vector3 (target.x, offset, target.z);
			CurrentWorker.transform.SetParent (tile.transform);
			tile.Worker = null;
			GameActionController.CurrentGameAction.currTile = tile;
			GameActionController.CurrentGameAction.prevTile = CurrentWorker.CurrentTile;
			CurrentWorker.CurrentTile = tile;
			KeyboardMouseController.OnClicked -= Move;
			GameActionController.SetActionIdle ();
		}
	}

	public void Build (Tile tile)
	{
		if (tile != null && tile.CanBuild ()) {
			tile.Build ();
			GameActionController.CurrentGameAction.currTile = tile;
			GameActionController.CurrentGameAction.prevTile = CurrentWorker.CurrentTile;
			KeyboardMouseController.OnClicked -= Build;
			GameActionController.SetActionIdle ();
		}
	}
}
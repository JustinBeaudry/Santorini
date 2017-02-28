using System;
using UnityEngine;

[Serializable]
public class GameAction
{
	public Player player;
	public PlayerAction playerAction;
	public Tile prevTile;
	public Tile currTile;
	public int time;

	public GameAction ( Player _player, PlayerAction _playerAction )
	{
		player = _player;
		playerAction = _playerAction;
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson ( this );
	}
}
using System;
using UnityEngine;

public struct GameAction : ICloneable
{
  public Player Player;
  public PlayerAction PlayerAction;
  public Tile StartTile;
  public Tile EndTile;
  public int Time;

  public void Initialize(Player player, PlayerAction playerAction)
  {
    Player = player;
    PlayerAction = playerAction;
  }

  public void Clear()
  {
    StartTile = null;
    EndTile = null;
    Time = 0;
  }

  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }

  public object Clone()
  {
    return (GameAction)this.MemberwiseClone();
  }
}
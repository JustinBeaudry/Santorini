using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

  public static List<GameActionController> Matches = new List<GameActionController>();

  public static void MatchAction(Tile tile, GameAction gameAction)
  {
    // Get the worker to do work on
    Worker worker = gameAction.player.CurrentWorker;
    GameActionController.CurrentGameAction.currTile = tile;
    GameActionController.CurrentGameAction.prevTile = worker.CurrentTile;
    GameActionController.CurrentGameAction.playerAction.Action(tile);
  }
}

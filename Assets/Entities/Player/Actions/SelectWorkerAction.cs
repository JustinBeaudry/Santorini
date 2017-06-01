using UnityEngine;

public class SelectWorkerAction : MonoBehaviour
{
  public void Do(Tile tile, GameAction gameAction)
  {
    if (tile != null && tile.Worker != null && tile.HasWorker() && gameAction.Player.IsWorkerOwner(tile.Worker))
    {
      if (GameActionController.IsUndo())
      {
        gameAction.Player.CurrentWorker = null;
      }
      else
      {
        gameAction.Player.CurrentWorker = tile.Worker;
      }
      GameActionController.GameActionComplete();
    }
  }
}
using UnityEngine;

public class MoveAction : MonoBehaviour
{
  public void Do(Tile tile, GameAction gameAction)
  {
    Worker worker = gameAction.Player.CurrentWorker;
    if (tile != null && tile.CanMove(worker))
    {
      if (GameActionController.IsUndo())
      {
        tile = gameAction.StartTile;
      }
      Vector3 endTile = tile.transform.position;
      Vector3 endPosition = new Vector3(endTile.x, tile.LevelAsOffset(), endTile.z);
      // set the workers position in world space
      worker.transform.position = endPosition;
      // set the workers parent
      worker.transform.SetParent(tile.transform);
      // remove the worker from the tile 
      tile.Worker = null;
      worker.CurrentTile = tile;
      GameActionController.GameActionComplete();
    }
  }
}
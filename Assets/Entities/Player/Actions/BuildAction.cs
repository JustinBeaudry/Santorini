using UnityEngine;

public class BuildAction : MonoBehaviour
{
  public void Do(Tile tile, GameAction gameAction)
  {
    if (tile != null && tile.CanBuild() && TileManager.IsTileNeighbor(tile))
    {
      if (GameActionController.IsUndo())
      {
        tile.UnBuild();
      }
      else
      {
        tile.Build();
      }
      GameActionController.GameActionComplete();
    }
  }
}
public class WinCondition
{
  // When this method is called the game should already have workers set
  // if this behavior changes, this method will break
  public void DoCheck()
  {
    // Main Win Condition
    // if the current gameAction is Move
    // and a player moves from Level 2 to Level 3, they Win
    GameAction currAction = GameActionController.CurrentGameAction;
    if (currAction != null)
    {
      Player currPlayer = currAction.player;
      if (currAction.playerAction.ActionText == "Move")
      {
        if (currAction.startTile == null && currAction.endTile == null)
        {
          return;
        }
        if (currAction.startTile.Level == Tile.Levels.Two && currAction.endTile.Level == Tile.Levels.Three)
        {
          MatchManager.CompleteMatch(GameActionController.GetInstance(), currPlayer);
          GameManager.SwitchScene("Post Match");
        }
      }
      // Alternate Win Condition(s)
      // (1) If a player has 0 workers, they lose
      if (currPlayer.Workers.Count == 0)
      {
        currPlayer.SetInactive();
      }
      // (2) If a players worker is unable to move, they become inactive
      // (3) If a players worker is unable to build, they become inactive
      // (4) if there is only one active player, the active player wins
    }
  }
}
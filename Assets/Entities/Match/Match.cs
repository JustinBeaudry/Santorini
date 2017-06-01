public struct Match
{
  public Player Winner;
  public GameActionDispatch GameActionDispatch;
  public void Initialize(GameActionDispatch gameActionDispatch, Player winner)
  {
    Winner = winner;
    GameActionDispatch = gameActionDispatch;
  }
}
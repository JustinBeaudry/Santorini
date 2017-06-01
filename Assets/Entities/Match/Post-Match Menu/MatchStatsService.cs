using System.Collections.Generic;
public class MatchStatsService
{
  public MatchStats GetStatsFromGameState(MatchController matchController)
  {
    int rounds = matchController.currentRound;
    // @NOTE:  The PlayerManager should not hold a reference to the winning player
    // @TODO:  Move the winning player to the MatchController  
    Player winner = MatchManager.GetLastMatch().Winner;
    return new MatchStats(rounds, winner);
  }
}

public class MatchStats
{
  public int Rounds;
  public Player Winner;

  public MatchStats(int rounds, Player winner)
  {
    Rounds = rounds;
    Winner = winner;
  }
}

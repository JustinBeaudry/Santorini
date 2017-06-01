using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
  public static List<Match> Matches = new List<Match>();
  private static int MatchCursor = -1;
  private static Match CurrentMatch
  {
    get
    {
      return Matches[MatchCursor];
    }
  }

  private void Awake()
  {

  }

  public static void NewMatch()
  {
    MatchCursor++;
    Match match = new Match();
    Matches.Add(match);
  }

  public static void ReplayActions(GameActionDispatch gameActionDispatch)
  { }

  public static void CompleteMatch(GameActionDispatch gameActionDispatch, Player winner)
  {
    Match match = CurrentMatch;
    GameActionDispatch completedMatch = (GameActionDispatch)gameActionDispatch.Clone();
    match.Initialize(completedMatch, winner);
    Matches.Add(match);
  }

  public static Match GetLastMatch()
  {
    return Matches[MatchCursor - 1];
  }
}
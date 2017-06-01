using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MatchController : MonoBehaviour
{
  public Text PlayerText;
  public Text RoundText;
  public Text TimeText;
  public Text ActionText;
  public Image PlayerWorkerColor;
  public static bool SettingsOpen = false;

  Grid grid;
  public int currentRound;
  public float totalTime;
  private float roundTime;

  private string playerText
  {
    get
    {
      Player player = GameActionController.CurrentPlayer;
      return player == null ? "" : player.Name;
    }
  }

  private string actionText
  {
    get
    {
      return GameActionController.CurrentGameAction.PlayerAction.ActionText;
    }
  }

  private Color playerWorkerColor
  {
    get
    {
      return GameActionController.CurrentPlayer.WorkerColor;
    }
  }

  void Awake()
  {
    GameManager.InitGame();
    grid = GameObject.Find("GridManager").GetComponent<Grid>();
  }

  void Start()
  {
    CreateTilesFromGrid();
    ResetRound();
    SetupGame();
  }

  void Update()
  {
    RenderGameUI();
    CheckWinConditions();
    CheckGameActions();
  }

  private void CreateTilesFromGrid()
  {
    foreach (Node n in grid.grid)
    {
      Vector3 tilePosition = new Vector3((float)n.gridX + grid.nodeRadius, 0f, (float)n.gridY + grid.nodeRadius);
      TileManager.CreateTileFromWorldPosition(tilePosition);
    }
  }

  private void ResetRound()
  {
    currentRound = 1;
  }

  private void SetupGame()
  {
    foreach (Player player in PlayerManager.Players)
    {
      PlayerAction playerAction = new PlayerAction();
      playerAction.Initialize("Set Workers", "Set " + player.MaxWorkers + " Workers.", SetWorker);

      GameAction gameAction = new GameAction();
      gameAction.Initialize(player, playerAction);

      GameActionController.AddAction(gameAction);
    }
    GameActionController.NextAction();
    ControlDevice.AddInteraction(MatchManager.InteractDispatch);
  }

  void SetPlayerActions()
  {
    // Go through each player in the game
    foreach (Player player in PlayerManager.Players)
    {
      // If the player has been set to inactive, don't add any actions for that player
      if (player.IsInactive())
      {
        // nothing to do for this player, but continue looping
        continue;
      }
      // Add each PlayerAction as a GameAction to the GameActionController
      foreach (PlayerAction playerAction in player.PlayerActions)
      {
        // queue up a GameAction for the player
        GameAction gameAction = new GameAction();
        gameAction.Initialize(player, playerAction);
        GameActionController.AddAction(gameAction);
      }
    }
  }

  void RenderGameUI()
  {
    totalTime = Time.timeSinceLevelLoad;
    roundTime = Time.timeSinceLevelLoad;
    float minutes = Mathf.Floor(roundTime / 60);
    float seconds = Mathf.Floor(roundTime > minutes * 60 ? roundTime - (minutes * 60) : roundTime);
    if (TimeText != null)
    {
      TimeText.text = minutes + ":" + seconds.ToString("0#");
    }

    if (GameActionController.CurrentPlayer != null)
    {
      if (PlayerText != null)
      {
        PlayerText.text = playerText;
      }
      if (RoundText != null)
      {
        RoundText.text = currentRound.ToString();
      }
      if (ActionText != null)
      {
        ActionText.text = actionText;
      }
      if (PlayerWorkerColor != null)
      {
        PlayerWorkerColor.color = playerWorkerColor;
      }
    }
  }

  void SetWorker(Tile tile, GameAction gameAction)
  {

  }

  void CheckWinConditions()
  {
    if (GameActionController.HasGameActions())
    {
      foreach (Player player in PlayerManager.Players)
      {
        foreach (WinCondition winCondition in player.WinConditions)
        {
          winCondition.DoCheck();
        }
      }
    }
  }

  void CheckGameActions()
  {
    Debug.Log("CheckGameActions()");
    if (GameActionController.IsIdle())
    {
      Debug.Log("GameActionController is Idle");
      if (GameActionController.HasGameActions())
      {
        Debug.Log("Has GameActions");
        GameActionController.NextAction();
      }
      else
      {
        if (MatchStarted)
        {
          currentRound++;
        }
        SetPlayerActions();
      }
    }
  }

}

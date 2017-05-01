using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MatchController : MonoBehaviour
{
  public GameObject Worker;
  public Material BaseWorkerMaterial;
  public Text PlayerText;
  public Text RoundText;
  public Text TimeText;
  public Text ActionText;
  public Image PlayerWorkerColor;
  public static bool SettingsOpen = false;
  public static bool MatchStarted = false;

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
      GameAction gameAction = GameActionController.CurrentGameAction;
      return gameAction == null ? "" : gameAction.playerAction.ActionText;
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
    foreach (Node n in grid.grid)
    {
      Vector3 tilePosition = new Vector3((float)n.gridX + grid.nodeRadius, 0f, (float)n.gridY + grid.nodeRadius);
      TileManager.CreateTileFromWorldPosition(tilePosition);
    }
    currentRound = 1;
    foreach (Player player in PlayerManager.Players)
    {
      PlayerAction playerAction = new PlayerAction("Set Workers", "Set " + player.MaxWorkers + " Workers.",
      (Tile tile) => SetWorker(tile));
      GameAction gameAction = new GameAction(player, playerAction);
      GameActionController.AddAction(gameAction);
    }
    GameActionController.NextAction();
  }

  void Update()
  {
    RenderGameUI();
    CheckWinConditions();
    CheckGameActions();
  }

  void SetPlayerActions()
  {
    foreach (Player player in PlayerManager.Players)
    {
      foreach (PlayerAction playerAction in player.PlayerActions)
      {
        GameActionController.AddAction(new GameAction(player, playerAction));
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

  void SetWorker(Tile tile)
  {
    if (tile != null && !tile.HasWorker())
    {
      GameObject worker = Instantiate(Worker, tile.transform);

      // Set worker color
      Material workerMaterial = new Material(BaseWorkerMaterial);
      workerMaterial.color = GameActionController.CurrentPlayer.WorkerColor;
      worker.GetComponent<MeshRenderer>().material = workerMaterial;

      // Set worker position
      worker.transform.localPosition = new Vector3(0, 0.25f, 0);

      // Get Worker Component
      Worker _worker = worker.GetComponent<Worker>();

      // Set reference to workers tile
      _worker.CurrentTile = tile;

      // Add worker to player
      GameActionController.CurrentPlayer.Workers.Add(_worker);
    }
    if (GameActionController.CurrentPlayer.Workers.Count == GameActionController.CurrentPlayer.MaxWorkers)
    {
      GameActionController.GameActionComplete();
      MatchStarted = true;
    }
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
    if (GameActionController.IsIdle())
    {
      if (GameActionController.HasGameActions())
      {
        GameActionController.NextAction();
        if (GameActionController.CurrentGameAction != null)
        {
          ControlDevice.AddInteraction(GameActionController.CurrentGameAction.playerAction.Action);
        }
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

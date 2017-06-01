using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlDevice : MonoBehaviour
{
  public delegate void TileInteract(Tile tile, GameAction gameAction);

  public static event TileInteract OnTileInteract;

  public new Camera camera;

  CameraController cameraController;

  void Start()
  {
    cameraController = camera.GetComponent<CameraController>();
  }

  public static void TileInteraction(Tile tile)
  {
    if (OnTileInteract != null)
    {
      OnTileInteract(tile, GameActionController.CurrentGameAction);
    }
  }

  public static void AddInteraction(TileInteract interact)
  {
    if (OnTileInteract == null)
    {
      OnTileInteract += interact;
    }
  }

  public static void RemoveInteraction(TileInteract interact)
  {
    if (OnTileInteract != null)
    {
      OnTileInteract -= interact;
    }
  }

  public void MoveCameraUp()
  {
    cameraController.MoveUp();
  }

  public void MoveCameraDown()
  {
    cameraController.MoveDown();
  }

  public void MoveCameraLeft()
  {
    cameraController.MoveLeft();
  }

  public void MoveCameraRight()
  {
    cameraController.MoveRight();
  }

  public void Undo()
  {
    GameActionController.UndoAction();
  }

  public void ShowGameOptions()
  {
    if (!MatchController.SettingsOpen)
    {
      MatchController.SettingsOpen = true;
      GameManager.LoadSceneAdditive("Game Options");
    }
  }

  public void HideGameOptions()
  {
    if (MatchController.SettingsOpen)
    {
      MatchController.SettingsOpen = false;
      GameManager.UnloadSceneAdditive("Game Options");
    }
  }

  void OnDestroy()
  {
    OnTileInteract = null;
  }
}

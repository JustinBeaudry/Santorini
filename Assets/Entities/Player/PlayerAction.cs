using System;
using System.Collections;

public struct PlayerAction
{
  // Text to display in the Actions UI
  public string ActionText;
  // Text to display when hovering over Actions UI
  public string ActionDescription;
  // Action to call to perform the GameAction
  public ControlDevice.TileInteract Action;

  public void Initialize(string actionText, string actionDescription, ControlDevice.TileInteract interact)
  {
    ActionText = actionText;
    ActionDescription = actionDescription;
    Action = interact;
  }
}
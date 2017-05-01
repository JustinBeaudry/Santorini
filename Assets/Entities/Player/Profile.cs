using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Profile : IEquatable<Profile>
{
  public String ID;
  public string PlayerName;
  public Color WorkerColor = Color.gray;

  public List<ControlDevice> ControlDevice = new List<ControlDevice>();

  public Profile(string playerName)
  {
    ID = Guid.NewGuid().ToString();
    PlayerName = playerName;
    // @TODO:  Add ControlDevice(s) to Profile
  }

  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }

  public bool Equals(Profile profile)
  {
    return ID.Equals(profile.ID);
  }
}
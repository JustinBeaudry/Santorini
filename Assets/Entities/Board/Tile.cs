using System;
using UnityEngine;

[Serializable]
public class Tile : MonoBehaviour, ICloneable
{

  public enum Levels
  {
    Ground,
    One,
    Two,
    Three,
    Dome
  }

  public Levels Level = Levels.Ground;
  public GameObject Tower1;
  public GameObject Tower2;
  public GameObject Tower3;
  public GameObject Dome;

  public Worker Worker = null;

  private const float tOneOffset = 0.5f;
  private float tTwoOffset;
  private float tThreeOffset;
  private float domeOffset;
  private float playerDomeOffset;

  void Awake()
  {
    print("tOneOffset: " + tOneOffset);
    tTwoOffset = tOneOffset * Tower1.transform.localScale.y;
    print("tTwoOffset: " + tTwoOffset);
    print("Tower1.transform.localScale.y: " + Tower1.transform.localScale.y);
    tThreeOffset = tTwoOffset + Tower2.transform.localScale.y;
    print("tThreeOffset: " + tThreeOffset);
    print("Tower2.transform.localScale.y: " + Tower2.transform.localScale.y);
    domeOffset = tThreeOffset + Tower3.transform.localScale.y - (Dome.transform.localScale.y / 2);
    print("domeOffset: " + domeOffset);
    print("Tower3.transform.localScale.y: " + Tower3.transform.localScale.y);
    print("Dome.transform.localScale.y: " + Dome.transform.localScale.y);
    playerDomeOffset = domeOffset + Tower3.transform.localScale.y;
    print("playerDomeOffset: " + playerDomeOffset);
  }

  void Update()
  {
    Transform worker = transform.Find("Worker(Clone)");
    Worker = worker == null ? null : worker.GetComponent<Worker>();
  }

  public bool CanBuild()
  {
    if (GameActionController.IsUndo())
    {
      return true;
    }
    if (HasWorker())
    {
      return false;
    }
    switch (Level)
    {
      case Levels.Ground:
        return true;
      case Levels.One:
        return true;
      case Levels.Two:
        return true;
      case Levels.Three:
        return true;
      case Levels.Dome:
        return false;
    }
    return false;
  }

  public bool CanMove(Worker worker)
  {
    Tile startingTile = worker.CurrentTile;
    bool hasNoWorker = !(this.HasWorker());
    bool isUndo = GameActionController.IsUndo();
    bool isNeighbor = TileManager.IsTileNeighbor(this);
    bool hasWalkableTower = true;

    if (this.HasTower() && startingTile.DiffLevelsByInt(this) > 1)
    {
      hasWalkableTower = false;
    }

    Debug.Log("isUndo " + isUndo);
    Debug.Log("hasNoWorker " + hasNoWorker);
    Debug.Log("isNeighbor " + isNeighbor);
    Debug.Log("hasWalkableTower " + hasWalkableTower);

    return isUndo || (hasNoWorker && isNeighbor && hasWalkableTower);
  }

  public void Build()
  {
    switch (Level)
    {
      case Levels.Ground:
        Level = Levels.One;
        GameObject t1 = Instantiate(Tower1, transform);
        t1.transform.localPosition = new Vector3(0, tOneOffset, 0);
        break;
      case Levels.One:
        Level = Levels.Two;
        GameObject t2 = Instantiate(Tower2, transform);
        t2.transform.localPosition = new Vector3(0, tTwoOffset, 0);
        break;
      case Levels.Two:
        Level = Levels.Three;
        GameObject t3 = Instantiate(Tower3, transform);
        t3.transform.localPosition = new Vector3(0, tThreeOffset, 0);
        break;
      case Levels.Three:
        Level = Levels.Dome;
        GameObject dome = Instantiate(Dome, transform);
        dome.transform.localPosition = new Vector3(0, domeOffset, 0);
        break;
    }
  }

  public void UnBuild()
  {
    switch (Level)
    {
      case Levels.Ground:
        break;
      case Levels.One:
        Level = Levels.Ground;
        GameObject t1 = GameObject.Find("Tile Level 1(Clone)");
        if (t1 != null)
        {
          Destroy(t1);
        }
        break;
      case Levels.Two:
        Level = Levels.One;
        GameObject t2 = GameObject.Find("Tile Level 2(Clone)");
        if (t2 != null)
        {
          Destroy(t2);
        }
        break;
      case Levels.Three:
        Level = Levels.Two;
        GameObject t3 = GameObject.Find("Tile Level 3(Clone)");
        if (t3 != null)
        {
          Destroy(t3);
        }
        break;
      case Levels.Dome:
        Level = Levels.Three;
        GameObject dome = GameObject.Find("Dome(Clone)");
        if (dome != null)
        {
          Destroy(dome);
        }
        break;
    }
  }

  public bool HasWorker()
  {
    return Worker != null;
  }

  public bool HasTower()
  {
    return Level != Levels.Ground;
  }

  public int DiffLevelsByInt(Tile tile)
  {
    return tile.LevelAsInt() - LevelAsInt();
  }

  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }

  public int LevelAsInt()
  {
    switch (Level)
    {
      case Levels.Ground:
        return 0;
      case Levels.One:
        return 1;
      case Levels.Two:
        return 2;
      case Levels.Three:
        return 3;
      case Levels.Dome:
        return 4;
    }
    return 0;
  }

  public float LevelAsOffset()
  {
    switch (Level)
    {
      case Levels.Ground:
        return tOneOffset;
      case Levels.One:
        return tTwoOffset;
      case Levels.Two:
        return tThreeOffset;
      case Levels.Three:
        return playerDomeOffset;
    }
    return tOneOffset;
  }

  public object Clone()
  {
    return (Tile)this.MemberwiseClone();
  }
}
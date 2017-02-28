using System;
using UnityEngine;

[Serializable]
public class Tile : MonoBehaviour
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

	void Awake ()
	{
		tTwoOffset = tOneOffset + Tower1.transform.localScale.y;
		tThreeOffset = tTwoOffset + Tower2.transform.localScale.y;
		domeOffset = tThreeOffset + Tower3.transform.localScale.y - ( Dome.transform.localScale.y / 2 );
		playerDomeOffset = domeOffset + Tower3.transform.localScale.y;
	}

	void Update ()
	{
		Transform worker = transform.FindChild ( "Worker(Clone)" );
		Worker = worker == null ? null : worker.GetComponent<Worker> ();
	}

	public bool CanBuild ()
	{
		if ( HasWorker () )
		{
			return false;
		}
		switch ( Level )
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

	public void Build ()
	{
		switch ( Level )
		{
			case Levels.Ground:
				Level = Levels.One;
				GameObject t1 = Instantiate ( Tower1, transform );
				t1.transform.localPosition = new Vector3 ( 0, tOneOffset, 0 );
				break;
			case Levels.One:
				Level = Levels.Two;
				GameObject t2 = Instantiate ( Tower2, transform );
				t2.transform.localPosition = new Vector3 ( 0, tTwoOffset, 0 );
				break;
			case Levels.Two:
				Level = Levels.Three;
				GameObject t3 = Instantiate ( Tower3, transform );
				t3.transform.localPosition = new Vector3 ( 0, tThreeOffset, 0 );
				break;
			case Levels.Three:
				Level = Levels.Dome;
				GameObject dome = Instantiate ( Dome, transform );
				dome.transform.localPosition = new Vector3 ( 0, domeOffset, 0 );
				break;
		}
	}

	public bool HasWorker ()
	{
		return Worker != null;
	}

	public bool HasTower ()
	{
		return Level != Levels.Ground;
	}

	public int DiffLevelsByInt ( Tile tile )
	{
		return tile.LevelAsInt () - LevelAsInt ();
	}

	public override string ToString ()
	{
		return JsonUtility.ToJson ( this );
	}

	public int LevelAsInt ()
	{
		switch ( Level )
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

	public float LevelAsOffset ()
	{
		switch ( Level )
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
}
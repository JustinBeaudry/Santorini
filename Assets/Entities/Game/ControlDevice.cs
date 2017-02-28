using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlDevice : MonoBehaviour
{
	public delegate void TileInteract ( Tile tile );

	public static event TileInteract OnTileInteract;

	public Camera camera;

	CameraController cameraController;

	void Start ()
	{
		cameraController = camera.GetComponent<CameraController> ();
	}

	public static void TileInteraction ( Tile tile )
	{
		if ( OnTileInteract != null )
		{
			OnTileInteract ( tile );
		}
	}

	public static void AddInteraction ( TileInteract interact )
	{
		if ( OnTileInteract == null )
		{
			OnTileInteract += interact;
		} 
	}

	public static void RemoveInteraction ( TileInteract interact )
	{
		OnTileInteract -= interact;
	}

	public void MoveCameraUp ()
	{
		cameraController.MoveUp ();
	}

	public void MoveCameraDown ()
	{
		cameraController.MoveDown ();
	}

	public void MoveCameraLeft ()
	{
		cameraController.MoveLeft ();
	}

	public void MoveCameraRight ()
	{
		cameraController.MoveRight ();
	}

	public void ShowGameOptions ()
	{
		if ( !MatchController.SettingsOpen )
		{
			MatchController.SettingsOpen = true;
			GameManager.LoadSceneAdditive ( "Game Options" );
		}
	}

	public void HideGameOptions ()
	{
		if ( MatchController.SettingsOpen )
		{
			MatchController.SettingsOpen = false;
			GameManager.UnloadSceneAdditive ( "Game Options" );
		}
	}

	void OnDestroy ()
	{
		OnTileInteract = null;
	}
}

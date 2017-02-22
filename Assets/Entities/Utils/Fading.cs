using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private const int drawDepth = -1000;
	private float alpha = 0.0f;
	private int fadeDir = -1;

	void OnGUI ()
	{
		// fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert operation into seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) the number between 0 and 1 between GUI.color uses aplha value between 0 and 1
		alpha = Mathf.Clamp (alpha, 0.0f, 1.0f);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);			 // set the alpha
		GUI.depth = drawDepth; 															 // ensure the black texture is drawn on top
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);  // drawDepth the texture to fit entire screen		
	}

	//
	//
	//
	public float BeginFade (int direction, float speed = 0.8f)
	{
		fadeDir = direction;
		fadeSpeed = speed;
		return (fadeSpeed);
	}

	void OnLevelWasLoaded ()
	{
		// Fade out after level has been loaded
		BeginFade (-1);		
	}
}

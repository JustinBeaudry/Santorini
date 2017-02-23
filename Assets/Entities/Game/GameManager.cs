using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{

	private static GameManager gameManager;

	private string currentSceneName;
	private string nextSceneName;
	private AsyncOperation resourceUnloadTask;
	private AsyncOperation sceneLoadTask;
	private static SceneState sceneState;

	private Fading fading;

	public static float SplashDelay = 0f;
	public static bool StartedFromMain = false;

	private delegate void UpdateDelegate ();

	private static UpdateDelegate[] updateDelegates;

	private enum SceneState
	{
		Reset,
		Preload,
		Load,
		Unload,
		Postload,
		Ready,
		Run,
		Count}

	;

	// Called from each scenes Singleton
	public static void InitGame (string scene = "Main")
	{
		if (!StartedFromMain) {
			if (SceneManager.GetActiveScene ().name == "Main") {
				StartedFromMain = true;
			} else {
				SceneManager.LoadScene (scene);
			}
		}
	}

	public static void SwitchScene (string scene, bool fade = true)
	{
		if (gameManager != null) {
			if (fade) {
				gameManager.StartCoroutine (TransitionScene (scene));
			} else {
				_SwitchScene (scene);
			}
		}
	}

	public static void LoadSceneAdditive (string scene)
	{
		if (gameManager != null) {
			SceneManager.LoadScene (scene, LoadSceneMode.Additive);
		}
	}

	public static void UnloadSceneAdditive (string scene)
	{
		if (gameManager != null) {
			SceneManager.UnloadSceneAsync (scene);
		}

	}

	public float GetSceneLoadProgress ()
	{
		return sceneLoadTask.progress;
	}

	protected static IEnumerator TransitionScene (string scene)
	{
		float fadeTime = gameManager.fading.BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		_SwitchScene (scene);
	}

	private static void _SwitchScene (string scene)
	{
		if (gameManager != null && gameManager.currentSceneName != scene) {
			gameManager.nextSceneName = scene;
			sceneState = SceneState.Reset;	
		}	
	}

	private static void showSplash ()
	{
		gameManager.StartCoroutine (TransitionSplash ());
	}

	protected static IEnumerator TransitionSplash ()
	{

		yield return new WaitForSeconds (SplashDelay);	

		float fadeSpeed = gameManager.fading.BeginFade (1);

		yield return new WaitForSeconds (fadeSpeed);

		_SwitchScene ("Menu");
	}

	protected void Awake ()
	{

		InitGame ();

		Object.DontDestroyOnLoad (gameObject);

		// Singleton
		gameManager = this;

		// Get the instance of the fading component
		fading = gameManager.GetComponent<Fading> ();

		// Update Array of updateDelegates
		updateDelegates = new UpdateDelegate[(int)SceneState.Count];

		updateDelegates [(int)SceneState.Reset] = UpdateSceneReset;
		updateDelegates [(int)SceneState.Preload] = UpdateScenePreload;
		updateDelegates [(int)SceneState.Load] = UpdateSceneLoad;
		updateDelegates [(int)SceneState.Unload] = UpdateSceneUnload;
		updateDelegates [(int)SceneState.Postload] = UpdateScenePostload;
		updateDelegates [(int)SceneState.Ready] = UpdateSceneReady;
		updateDelegates [(int)SceneState.Run] = UpdateSceneRun;

		showSplash ();
	}

	protected void OnDestroy ()
	{
		if (updateDelegates != null) {
			for (int i = 0; i < (int)SceneState.Count; i++) {
				updateDelegates [i] = null;
			}
			updateDelegates = null;
		}	

		gameManager = null;
	}

	protected void Update ()
	{
		if (updateDelegates != null && sceneState != null && updateDelegates [(int)sceneState] != null) {
			updateDelegates [(int)sceneState] ();
		}
	}

	private void UpdateSceneReset ()
	{
		print ("[INFO] Reset()");
		System.GC.Collect ();
		sceneState = SceneState.Preload;
	}

	private void UpdateScenePreload ()
	{
		print ("[INFO] Preload()");
		if (nextSceneName != null) {
			sceneLoadTask = SceneManager.LoadSceneAsync (nextSceneName);
		}
		sceneState = SceneState.Load;
	}

	private void UpdateSceneLoad ()
	{
		print ("[INFO] Load()");
		if (sceneLoadTask != null && sceneLoadTask.isDone) {
			sceneState = SceneState.Unload;
		} else {
			// update scene loading process
		}
	}

	private void UpdateSceneUnload ()
	{
		print ("[INFO] Unload()");
		if (resourceUnloadTask == null) {
			resourceUnloadTask = Resources.UnloadUnusedAssets ();
		} else {
			if (resourceUnloadTask.isDone) {
				resourceUnloadTask = null;
				sceneState = SceneState.Postload;
			}
		}
	}

	private void UpdateScenePostload ()
	{
		print ("[INFO] Postload()");
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
	}

	private void UpdateSceneReady ()
	{
		print ("[INFO] Read()");
		// if there are unused gameobjects in the scene, don't do this
		System.GC.Collect ();
		sceneState = SceneState.Run;
	}

	private void UpdateSceneRun ()
	{
		if (currentSceneName != nextSceneName) {
			sceneState = SceneState.Reset;
		}
	}
}

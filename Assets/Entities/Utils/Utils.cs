using UnityEngine;

public class Utils : MonoBehaviour
{
	private static Utils utils;

	void Awake ()
	{
		utils = this;
	}

	public static void Noop ()
	{
	}
}

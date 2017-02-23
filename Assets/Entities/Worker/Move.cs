using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
	public float speed = 20;
	Vector3[] path;
	int targetIndex;

	public void ToTarget (Transform target)
	{
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
	}

	void OnPathFound (Vector3[] newPath, bool success)
	{
		if (success) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}	
	}

	IEnumerator FollowPath ()
	{
		Debug.Log (path.Length);
		Vector3 currentWaypoint = path [0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path [targetIndex];
			}
			currentWaypoint.y = 0.25f;
			transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}

	void OnDrawGizmos ()
	{
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube (path [i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine (transform.position, path [i]);				
				} else {
					Gizmos.DrawLine (path [i - 1], path [i]);
				}
			}
		}
	}
}


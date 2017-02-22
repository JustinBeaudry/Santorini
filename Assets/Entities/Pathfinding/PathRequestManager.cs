using UnityEngine;
using System;
using System.Collections.Generic;

public class PathRequestManager : MonoBehaviour
{
	Queue<PathRequest> requestQueue = new Queue <PathRequest> ();
	PathRequest currentRequest;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake ()
	{
		instance = this;
		pathfinding = GetComponent<Pathfinding> ();
	}

	public static void RequestPath (Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		PathRequest newRequest = new PathRequest (pathStart, pathEnd, callback);
		instance.requestQueue.Enqueue (newRequest);
		instance.TryProcessNext ();
	}

	void TryProcessNext ()
	{
		if (!isProcessingPath && requestQueue.Count > 0) {
			currentRequest = requestQueue.Dequeue ();
			isProcessingPath = true;
			pathfinding.StartFindPath (currentRequest.pathStart, currentRequest.pathEnd);
		}
	}

	public void FinishProcessingPath (Vector3[] path, bool success)
	{
		currentRequest.callback (path, success);
		isProcessingPath = true;
		TryProcessNext ();
	}

	struct PathRequest
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest (Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
		{
			pathStart = _pathStart;
			pathEnd = _pathEnd;
			callback = _callback;
		}
	}

}


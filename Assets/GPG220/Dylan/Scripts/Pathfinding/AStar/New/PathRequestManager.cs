using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.New
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        private PathRequest currentPathRequest;

        private static PathRequestManager instance;
        private AStarPathfinder pathfinding;

        private bool isProcessingPath;

        private void Awake()
        {
            instance = this;
            pathfinding = GetComponent<AStarPathfinder>();
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callBack(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callBack;

            public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> _callBack)
            {
                pathStart = start;
                pathEnd = end;
                callBack = _callBack;
            }
        }
    }
}
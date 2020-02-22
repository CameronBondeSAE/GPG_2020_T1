using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderController : MonoBehaviour
    {
        public bool goToNearestPossiblePos = true;
        public int proximityFlowFieldRadius = 4;
        public List<PathFinderSector> sectors;
        
        [FoldoutGroup("Sector Generation")]
        public bool autoGenerateSectors = false;
        [FoldoutGroup("Sector Generation")]
        public int maxSectorSize = 50;
        [FoldoutGroup("Sector Generation")]
        public float sectorTileSize = 1;
        [FoldoutGroup("Sector Generation")]
        public LayerMask walkableMask = -1;
        [FoldoutGroup("Sector Generation")]
        public LayerMask ignoreMask = 0;
        [FoldoutGroup("Sector Generation")]
        public int sectorLayer = 0;
        [FoldoutGroup("Sector Generation")]
        public GameObject tileObjectPrefab;

        private void Start()
        {
            if(autoGenerateSectors)
                CreateSectors();
        }

        #region Debug

        // HACKY & POTENTIALLY BUGGY DEBUG SYSTEM
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private bool _debugPathGeneration = false;
        [FoldoutGroup("Debug"), ShowInInspector]
        private List<PathFinderSectorTileData> _debugPathList = new List<PathFinderSectorTileData>();
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private float _debugPathGenerationDelay = 0.0001f; // seconds
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private float _debugPathGenerationDataDeletionDelay = 3f;
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private Color32 _debugStartColor = new Color32(0,0,255,200);
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private Color32 _debugEndColor = new Color32(255,200,50,200);
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private Color32 _debugGreenColor = new Color32(50,255,50,200);
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private Color32 _debugRedColor = new Color32(255,0,0,200);
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private Color32 _debugPathColor = new Color32(0,0,0,255);
    
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private readonly Vector3 _debugEndPointSize = new Vector3(0.4f, 2.5f, 0.4f);
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private readonly Vector3 _debugWaypointPointSize = new Vector3(0.4f, 1.5f, 0.4f);
    
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private GameObject _testStartPos;
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private GameObject _testEndPos;
        [FoldoutGroup("Debug"), ShowInInspector, SerializeField]
        private bool _calculateFlowField;

        [FoldoutGroup("Debug"), ShowInInspector]
        private List<PathFinderSectorTileData> _waypointList;
/*    [FoldoutGroup("Debug"), Button("Find Path"), DisableInEditorMode]
    private void TestFindPath()
    {
        if (testStartPos == null || testEndPos == null)
            return;

        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating path. " + path.tilePath?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(path));
            }
            waypointList = path.tilePath;
        };
        FindPathTo(testStartPos.transform.position, testEndPos.transform.position, false, onDoneFunc);
    }*/
    
        [FoldoutGroup("Debug"), Button("Find Path (Proximity FlowField)"), DisableInEditorMode]
        private void TestFindPathProximityFlowField()
        {
            if (_testStartPos == null || _testEndPos == null)
                return;

            Action<PathFinderPath> onDoneFunc = path =>
            {
                if (_calculateFlowField)
                {
                    StartCoroutine(FindFlowFieldPathInProximity(path));
                }
                _waypointList = path.tilePath;
            };
            FindPathTo(_testStartPos.transform.position, _testEndPos.transform.position, false, onDoneFunc);
        }
        
        private IEnumerator DrawFlowFieldTEMPTEST(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
        {
            var rayStartPos = currentTileData.GetPosition();
            rayStartPos.y += 0.2f;

            if (_debugPathGeneration)
            {
                Debug.DrawRay(rayStartPos-(currentTileData.flowFieldDirection/2), currentTileData.flowFieldDirection, Color.red, 30f);
                Debug.DrawRay(rayStartPos+(currentTileData.flowFieldDirection/2*.3f),currentTileData.flowFieldDirection*.3f, Color.yellow, 30f);
            }
        
            currentTileData.vecDirDrawed = true;
            yield return new WaitForEndOfFrame();
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if(neighbourTileData == null || neighbourTileData.vecDirDrawed) return;
            
                var generateVectorField = DrawFlowFieldTEMPTEST(neighbourTileData, sector, path);
                while(generateVectorField.MoveNext())
                {
                }
            });

            yield return 0;
        }
    
        private IEnumerator DrawFlowFieldTEMPTESTNEW(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
        {
            var rayStartPos = currentTileData.GetPosition();
            rayStartPos.y += 0.2f;
            Debug.DrawRay(rayStartPos-(currentTileData.flowFieldDirection/2), currentTileData.flowFieldDirection, Color.red, 30f);
            Debug.DrawRay(rayStartPos+(currentTileData.flowFieldDirection/2*.3f),currentTileData.flowFieldDirection*.3f, Color.yellow, 30f);
            currentTileData.vecDirDrawed = true;
            yield return new WaitForEndOfFrame();

            if (currentTileData?.tile?.neighbourTiles != null)
            {
                foreach (var neighbourTile in currentTileData.tile.neighbourTiles)
                {
                    if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) continue;
                    path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                    if(neighbourTileData == null || neighbourTileData.vecDirDrawed) continue;

                    yield return StartCoroutine(DrawFlowFieldTEMPTESTNEW(neighbourTileData, sector, path));
                }
            }

            yield return 0;
        }
        
        private void OnDrawGizmos()
        {
            if(!_debugPathGeneration || _debugPathList == null || _debugPathList.Count == 0)
                return;

        
            foreach (var tileData in _debugPathList)
            {
                Gizmos.color = tileData.isStart ? _debugStartColor : tileData.isEnd ? _debugEndColor : tileData.isPath ? _debugPathColor : tileData.isRed ? _debugRedColor : tileData.isGreen ? _debugGreenColor : new Color32(0,0,0,0);
            
                Gizmos.DrawCube(tileData.GetPosition(), tileData.isStart || tileData.isEnd ? _debugEndPointSize : _debugWaypointPointSize);
            }
        }
        
        #endregion

        #region Sector Functionality

        /// <summary>
        /// Looks up all sectors in the current scene and initialized them.
        /// </summary>
        /// <param name="reloadExisting">Reloads sectors that already have been loaded.</param>
        [Button("Load All Sectors In Scene")]
        public void LoadAllSectorsInScene(bool reloadExisting = true)
        {
            var sectorsInScene = FindObjectsOfType<PathFinderSector>();

            // Load all sectors
            //sectorsInScene.ForEach(s => StartCoroutine(LoadSector(s, reloadExisting, false)));

            foreach (var sec in sectorsInScene)
            {
                var x = LoadSector(sec, reloadExisting, false);
                while (x.MoveNext())
                {
                
                }
            }
        
            // Connect all sectors
            sectorsInScene.ForEach(s => StartCoroutine(ConnectSector(s)));
        }
    
        private IEnumerator LoadSector(PathFinderSector sector, bool reload = false, bool connectSector = false)
        {
            if (sectors.Contains(sector) && !reload) // TODO Maybe Reload sector? Recalculate?
                yield break;
            sectors.Remove(sector);
        
            sector.CreateSectorTileGrid();
            if (connectSector)
            {
                var cr = ConnectSector(sector);
                while (cr.MoveNext())
                {
                    
                }
            }
        
            sectors.Add(sector);
            yield return 0;
        }

        private static IEnumerator ConnectSector(PathFinderSector sector)
        {
            foreach (var borderTileObject in sector.borderTileObjects)
            {
                borderTileObject.Tile?.neighbourTiles.AddRange(borderTileObject.GetNeighbourTileObjects(true).Select(obj => obj.Tile).ToList());
            }
            yield return 0;
        }

        private List<PathFinderSector> GetConnectingSectors(PathFinderSector sector)
        {
            var hits = Physics.OverlapBox(sector.bounds.center, sector.bounds.extents);

            var connectingSectors = hits.Where(col =>
            {
                var pfs = col.GetComponent<PathFinderSector>();
                return pfs != null && pfs != sector;
            }).Select(col => col.GetComponent<PathFinderSector>()).ToList();
        
            return connectingSectors;
        }
        
        /// <summary>
        /// Returns a sector at given point.
        /// </summary>
        public PathFinderSector GetSectorByPoint(Vector3 point) // Point must be within a sector
        {
            return sectors.FirstOrDefault(s => s.ContainsPoint(point));
        }

        /// <summary>
        /// Returns the nearest sector of a given point.
        /// </summary>
        public PathFinderSector GetNearestSector(Vector3 point) // Point may be outside of any sector
        {
            PathFinderSector nearestSector = null;
            float nearestSectorDist = -1;

            // TODO !!! UNreliable method, especially if the sectors have different sizes! Improve
            sectors.ForEach(sec =>
            {
                var dist = Vector3.Distance(sec.transform.position, point);

                if (nearestSector != null && !(dist < nearestSectorDist)) return;
                nearestSector = sec;
                nearestSectorDist = dist;
            });
        
            return nearestSector;
        }

        #endregion

        #region Path Finding

        /// <summary>
        /// Tries to find a path from a given start position to a target position.
        /// </summary>
        /// <param name="startPos">The starting point from which to search from.</param>
        /// <param name="targetPos">Target target point.</param>
        /// <param name="useFlowField">If set to true, a flowfield is calculated for each sectors along the path. Otherwise only a path in form of a list of waypoints is calculated.</param>
        /// <param name="onDoneCallback">Function that gets called when the path is fully calculated and returns an instance of <c>PathFinderPath</c>.</param>
        public void FindPathTo(Vector3 startPos, Vector3 targetPos, bool useFlowField = true, Action<PathFinderPath> onDoneCallback = null)
        {
            Action<PathFinderPath> onDoneFuncInternal = path =>
            {
                if (useFlowField)
                    StartCoroutine(FindFlowFieldPathInProximity(path, onDoneCallback));
                else
                    onDoneCallback?.Invoke(path);
            };
            StartCoroutine(FindPath(startPos, targetPos, onDoneFuncInternal));
        }
        
        private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Action<PathFinderPath> onDoneCallback = null)
        {
            var path = new PathFinderPath(this);
        
            var tmpDebugPathList = new List<PathFinderSectorTileData>();

            var startNode = GetNearestNode(startPos);
            if(startNode == null) yield break;
            var startTileData = new PathFinderSectorTileData(startNode);
            var targetTile = GetNearestNode(targetPos);

            if (startTileData.tile == null || targetTile == null) yield break;
        
            startTileData.GCost = 0;
            startTileData.HCost = PathFinderSectorTileData.CalculateHCost(startTileData.tile, targetTile);
        
            if (_debugPathGeneration)
            {
                startTileData.isStart = true;
                startTileData.isGreen = true;
                _debugPathList.Add(startTileData);
                tmpDebugPathList.Add(startTileData);
            }
        
            var greenTilesData = new List<PathFinderSectorTileData>(){startTileData};
            var redTilesData = new List<PathFinderSectorTileData>();
        
            PathFinderSectorTileData targetTileData = null;
        
            while (true) // TODO Add security break!
            {
                var currentTileData = GetLowestFCostTile(greenTilesData);
            
                if(currentTileData == null)
                    break;

                greenTilesData.Remove(currentTileData);
                if (_debugPathGeneration)
                    currentTileData.isGreen = false;
                redTilesData.Add(currentTileData);
                if (_debugPathGeneration)
                    currentTileData.isRed = true;

                if (currentTileData.tile.Equals(targetTile) || currentTileData.tile.neighbourTiles == null || currentTileData.tile.neighbourTiles.Count == 0)
                {
                    if (goToNearestPossiblePos || currentTileData.tile.Equals(targetTile))
                    {
                        targetTileData = currentTileData;
                        if (_debugPathGeneration)
                            currentTileData.isEnd = true;
                    }
                    break;
                }
            
                foreach (var neighbourTile in currentTileData.tile.neighbourTiles)
                {
                    if(neighbourTile == null) continue;
                
                    path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                    if(neighbourTileData == null)
                        neighbourTileData = new PathFinderSectorTileData(neighbourTile);
                    currentTileData.neighbourTiles.Add(neighbourTileData);
                
                    if(redTilesData.Contains(neighbourTileData)) continue;

                    if (_debugPathGeneration && !_debugPathList.Contains(neighbourTileData))
                    {
                        _debugPathList.Add(neighbourTileData);
                        tmpDebugPathList.Add(neighbourTileData);
                    }

                    var neighbourGCost = Vector3.Distance(neighbourTile.position, currentTileData.tile.position) +
                                         currentTileData.GCost;
                    var neighbourHCost = PathFinderSectorTileData.CalculateHCost(neighbourTile, targetTile);
                    var neighbourFCost = neighbourGCost + neighbourHCost;

                    if ((neighbourTileData.fCost >= 0 && neighbourFCost < neighbourTileData.fCost) ||
                        !greenTilesData.Contains(neighbourTileData))
                    {
                        neighbourTileData.GCost = neighbourGCost;
                        neighbourTileData.HCost = neighbourHCost;
                        neighbourTileData.lastTile = currentTileData.tile;
                        neighbourTileData.lastTileData = currentTileData;
                    
                        if (!greenTilesData.Contains(neighbourTileData))
                        {
                            greenTilesData.Add(neighbourTileData);
                            if (_debugPathGeneration)
                                neighbourTileData.isGreen = true;
                        }
                    }
                }
                if (_debugPathGeneration && _debugPathGenerationDelay > 0)
                    yield return new WaitForSeconds(_debugPathGenerationDelay);
            }

            var waypointDataList = CreateWaypointsListFromTileData(targetTileData);
            path.tilePath = waypointDataList;
            onDoneCallback?.Invoke(path);

            if (_debugPathGeneration)
            {
                waypointDataList?.ForEach(wp => { wp.isPath = true; });
            
                yield return new WaitForSeconds(_debugPathGenerationDataDeletionDelay);
                _debugPathList = _debugPathList.Except(tmpDebugPathList).ToList(); // TODO SUPER HACKY & POTENTIALLY BUGGY
            }

            yield return 0;
        }
        
        private IEnumerator FindFlowFieldPathInProximity(PathFinderPath path, Action<PathFinderPath> onDoneAction = null)
        {
            path.flowFieldAvailable = true;
            // PATH MUST BE End to Start sorted! 
        
            //var flowFields = new Dictionary<PathFinderSector, PathFinderFlowField>();


            var endPositionTilesTMPTEST = new List<PathFinderSectorTileData>();
        
            //PathFinderFlowField secFlowField = null;
            var inverseTilePath = new List<PathFinderSectorTileData>(path.tilePath);
        
            inverseTilePath.Reverse();
            for (int i = 0; i < inverseTilePath.Count; i++)
            {
                var currentTileData = inverseTilePath[i];
                if(currentTileData == null || currentTileData.tile.sector == null)
                    continue;

                // Calculate FlowField
                if (i == 0/*inverseTilePath.Count-1*/ || inverseTilePath[i-1].tile.sector != currentTileData.tile.sector)
                {
                    if(_debugPathGeneration)
                        Debug.DrawRay(currentTileData.GetPosition(), Vector3.up*5, Color.cyan,30f);
                
                    Action GenVecFieldAction = () =>
                    {
                        /*foreach (var t in inverseTilePath)
                    {
                        if (t.tile.sector == currentTileData.tile.sector)
                        {
                            Debug.Log("Gen Proximity Vec Field...");
                            IEnumerator x = GenerateSurroundingVectorField(t, path, proximityFlowFieldRadius);
                            while (x.MoveNext())
                            {
                                
                            }
                        }
                    }*/
                        //StartCoroutine(DrawFlowFieldTEMPTESTNEW(currentTileData, currentTileData.tile.sector, path));
                    };
                    endPositionTilesTMPTEST.Add(currentTileData);
                    yield return StartCoroutine(GenerateHeatmap(currentTileData, currentTileData.tile.sector, path, currentTileData.tile.position, GenVecFieldAction));
                
                }
            }
        
            foreach (var t in inverseTilePath)
            {
                IEnumerator x = GenerateSurroundingVectorField(t, t.tile.sector, path, proximityFlowFieldRadius);
                while (x.MoveNext())
                {
                                
                }
            }

            endPositionTilesTMPTEST.ForEach(tileData =>
            {
                /*if (tileData.lastTile != null)
            {
                tileData.flowFieldDirection = tileData.GetPosition() - tileData.lastTile.position;
            }*/
                if(_debugPathGeneration)
                    StartCoroutine(DrawFlowFieldTEMPTESTNEW(tileData, tileData.tile.sector, path));
            });
        
            onDoneAction?.Invoke(path);
            yield return 0;
        }

        /// <summary>
        /// Calculates the vector field around given <paramref name="currentTileData"/>. This is a recursive function
        /// and is intended to be called as a Coroutine.
        /// </summary>
        /// <param name="currentTileData">The tile data of which to calculate the vector field.</param>
        /// <param name="sector">The sector of the initial tile.</param>
        /// <param name="path">Reference to the path. A valid path is required. <see cref="FindPathTo"/></param>
        /// <param name="onDoneAction">Function that gets called when the path is fully calculated.</param>
        /// <param name="depth">Maximum tiles away from the initial tile to calculate the vector field. -1 stands for unlimited.</param>
        /// <returns></returns>
        public IEnumerator GenerateSurroundingVectorField(PathFinderSectorTileData currentTileData, PathFinderSector sector, 
            PathFinderPath path, Action onDoneAction, int depth = -1)
        {
            yield return StartCoroutine(GenerateSurroundingVectorField(currentTileData, sector, path, depth));
            onDoneAction?.Invoke();
            yield return 0;
        }
    
        /// <summary>
        /// Calculates the vector field around given <paramref name="currentTileData"/>. This is a recursive function
        /// and is intended to be called as a Coroutine.
        /// </summary>
        /// <param name="currentTileData">The tile data of which to calculate the vector field.</param>
        /// <param name="sector">The sector of the initial tile.</param>
        /// <param name="path">Reference to the path. A valid path is required. <see cref="FindPathTo"/></param>
        /// <param name="depth">Maximum tiles away from the initial tile to calculate the vector field. -1 stands for unlimited.</param>
        /// <returns></returns>
        private IEnumerator GenerateSurroundingVectorField(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path, int depth = -1)
        {
            if (currentTileData?.tile == null || depth == 0)
                yield break;
        
            if (currentTileData.flowFieldDirection.Equals(Vector3.negativeInfinity))
            {
                // Calculate Flow Field Direction
                PathFinderSectorTileData leftTileData = null, rightTileData = null, topTileData = null, bottomTileData = null;
                PathFinderSectorTile leftTile = currentTileData.tile.GetLeftTile(),
                    rightTile = currentTileData.tile.GetRightTile(),
                    topTile = currentTileData.tile.GetTopTile(),
                    bottomTile = currentTileData.tile.GetBottomTile();
            
                // ITs IMPORTANT THAT this method is being called for each node END to Start... 
                if(leftTile != null) path.tileDataList.TryGetValue(leftTile, out leftTileData);
                var leftTileDist = leftTile == null || leftTileData == null ? currentTileData.flowFieldDistanceToTarget : leftTile.sector != sector ? currentTileData.flowFieldDistanceToTarget/*currentTileData.flowFieldDistanceToTarget+1*/ : leftTileData.flowFieldDistanceToTarget;
            
                if(rightTile != null) path.tileDataList.TryGetValue(rightTile, out rightTileData);
                var rightTileDist = rightTile == null || rightTileData == null ? currentTileData.flowFieldDistanceToTarget : rightTile.sector != sector ? currentTileData.flowFieldDistanceToTarget/*currentTileData.flowFieldDistanceToTarget+1*/ : rightTileData.flowFieldDistanceToTarget;
            
                if(topTile != null) path.tileDataList.TryGetValue(topTile, out topTileData);
                var topTileDist = topTile == null || topTileData == null ? currentTileData.flowFieldDistanceToTarget : topTile.sector != sector ? currentTileData.flowFieldDistanceToTarget/*currentTileData.flowFieldDistanceToTarget+1*/ : topTileData.flowFieldDistanceToTarget;
            
                if(bottomTile != null) path.tileDataList.TryGetValue(bottomTile, out bottomTileData);
                var bottomTileDist = bottomTile == null || bottomTileData == null ? currentTileData.flowFieldDistanceToTarget : bottomTile.sector != sector ? currentTileData.flowFieldDistanceToTarget/*currentTileData.flowFieldDistanceToTarget+1*/ : bottomTileData.flowFieldDistanceToTarget;

                if (leftTileData != null && currentTileData.tile.sector != sector && leftTile?.sector == sector)
                    rightTileDist = leftTileData.flowFieldDistanceToTarget + 2;
                if (rightTileData != null && currentTileData.tile.sector != sector && rightTile?.sector == sector)
                    leftTileDist = rightTileData.flowFieldDistanceToTarget + 2;
                if (topTileData != null && currentTileData.tile.sector != sector && topTile?.sector == sector)
                    bottomTileDist = topTileData.flowFieldDistanceToTarget + 2;
                if (bottomTileData != null && currentTileData.tile.sector != sector && bottomTile?.sector == sector)
                    topTileDist = bottomTileData.flowFieldDistanceToTarget + 2;
            
                // ITs IMPORTANT THAT this method is being called for each node END to Start... 
                var dirVector = new Vector3(leftTileDist-rightTileDist,0,bottomTileDist - topTileDist);
/*

            if (leftTile?.sector != currentTileData.tile.sector &&
                leftTile?.sector == currentTileData.lastTile.sector && leftTileData != null)
                leftTileData.flowFieldDirection = (currentTileData.GetPosition()-leftTile.position).normalized;
            if (rightTile?.sector != currentTileData.tile.sector &&
                rightTile?.sector == currentTileData.lastTile.sector && rightTileData != null)
                rightTileData.flowFieldDirection = (currentTileData.GetPosition()-rightTile.position).normalized;
            if (topTile?.sector != currentTileData.tile.sector &&
                topTile?.sector == currentTileData.lastTile.sector && topTileData != null)
                topTileData.flowFieldDirection = (currentTileData.GetPosition()-topTile.position).normalized;
            if (bottomTile?.sector != currentTileData.tile.sector &&
                bottomTile?.sector == currentTileData.lastTile.sector && bottomTileData != null)
                bottomTileData.flowFieldDirection = (currentTileData.GetPosition()-bottomTile.position).normalized;*/
            
                /*var dirVector = new Vector3((leftTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (rightTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget), 
                0, 
                (bottomTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (topTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget));*/

                /*if (dirVector.Equals(Vector3.zero) && currentTileData.lastTileData != null){
                dirVector.x = currentTileData.flowFieldDistanceToTarget-currentTileData.lastTileData.flowFieldDistanceToTarget;
                dirVector.z = currentTileData.flowFieldDistanceToTarget-currentTileData.lastTileData.flowFieldDistanceToTarget;
            }*/
            
                currentTileData.flowFieldDirection = dirVector.normalized;
            }

            // If this tile is from a neighbouring sector, we don't want to calculate their children. ITs IMPORTANT THAT this method is being called for each node END to Start...
            if (currentTileData.tile.sector != sector) yield break;

            var newDepth = depth == -1 ? depth : depth - 1;
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if (neighbourTileData == null || !neighbourTileData.flowFieldDirection.Equals(Vector3.negativeInfinity)/* || currentTileData.tile.sector != neighbourTile.sector*/) return;
            
                var generateVectorField = GenerateSurroundingVectorField(neighbourTileData, sector, path, newDepth);
                while(generateVectorField.MoveNext())
                {
                }
            });
            
            yield return null;
        }
    
        private IEnumerator GenerateHeatmap(PathFinderSectorTileData currentTileData, PathFinderSector sector,
            PathFinderPath path, Vector3 targetPosition, Action onDoneAction)
        {
            yield return StartCoroutine(GenerateHeatmap(currentTileData, sector, path, targetPosition));
            onDoneAction?.Invoke();
            yield return 0;
        }
    
        private IEnumerator GenerateHeatmap(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path, Vector3 targetPosition)
        {
            if (currentTileData.GetPosition() == targetPosition)
            {
                currentTileData.flowFieldDistanceToTarget = 0;
            }
            
            var neighboursToEvaluate = new List<PathFinderSectorTileData>();
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if (neighbourTileData == null)
                {
                    neighbourTileData = new PathFinderSectorTileData(neighbourTile);
                    path.AddTileData(neighbourTileData);
                }
                
                var distToTargetNotSetYet = neighbourTileData.flowFieldDistanceToTarget < 0;
                if (distToTargetNotSetYet || neighbourTileData.flowFieldDistanceToTarget > currentTileData.flowFieldDistanceToTarget + 1)
                {
                    neighbourTileData.flowFieldLastTile = currentTileData.tile;
                    neighbourTileData.flowFieldLastTileData = currentTileData;
                    neighbourTileData.flowFieldDistanceToTarget = currentTileData.flowFieldDistanceToTarget + 1;
                    neighboursToEvaluate.Add(neighbourTileData);
                }
            });

            foreach (var neighbourTileData in neighboursToEvaluate)
            {
                if (neighbourTileData == null || (sector != null && neighbourTileData.tile.sector != sector)) continue;
                //yield return StartCoroutine(GenerateHeatmap(neighbourTileData, sector, path, targetPosition));
                IEnumerator genItr = GenerateHeatmap(neighbourTileData, sector, path, targetPosition);
                while (genItr.MoveNext())
                {
                
                }
            }

            yield return 0;
        }
    
        private static List<PathFinderSectorTileData> CreateWaypointsListFromTileData(PathFinderSectorTileData targetTileData)
        {
            var waypoints = new List<PathFinderSectorTileData>();
            if (targetTileData == null)
                return waypoints;

            var currentTileData = targetTileData;
            while (currentTileData.lastTile != null)
            {
                waypoints.Add(currentTileData); // .position
                currentTileData = currentTileData.lastTileData;
            }

            waypoints.Reverse();

            return waypoints;
        }
        
        
        public PathFinderSectorTile GetNearestNode(Vector3 position, bool ignoreNonSectorPos = true)
        {
            var sector = GetSectorByPoint(position);
            bool sectorNull = sector == null;
            
            if ((sectorNull || sector.sectorTiles == null || sector.sectorTiles.Count <= 0) && ignoreNonSectorPos) return null;
            
            if (!ignoreNonSectorPos && sectorNull)
            {
                sector = GetNearestSector(position);
                if (sector == null || sector.sectorTiles == null || sector.sectorTiles.Count <= 0)
                    return null;
            }
        
            PathFinderSectorTile nearestNode = null;
            var nearestNodeDist = 0f;

            if (sector.sectorTiles == null)
                return null;
            
            foreach (var tile in sector.sectorTiles)
            {
                var dist = Vector3.Distance(tile.position, position);
            
                if (!(dist < nearestNodeDist) && nearestNode != null) continue;
            
                nearestNode = tile;
                nearestNodeDist = dist;
            }

            return nearestNode;
        }

        private static PathFinderSectorTileData GetLowestFCostTile(IEnumerable<PathFinderSectorTileData> tileDataList)
        {
            PathFinderSectorTileData lowestCostTileData = null;
            foreach (var tileData in tileDataList)
            {
                if (lowestCostTileData == null || tileData.fCost < lowestCostTileData.fCost)
                {
                    lowestCostTileData = tileData;
                }   
            }

            return lowestCostTileData;
        }
        
        /*private IEnumerator GenerateSurroundingHeatmap(PathFinderSectorTileData currentTileData,
            PathFinderPath path, Vector3 targetPosition, Action onDoneAction, int depth = -1)
        {
            yield return StartCoroutine(GenerateSurroundingHeatmap(currentTileData, path, targetPosition, depth));
            onDoneAction?.Invoke();
            yield return 0;
        }
    
        private IEnumerator GenerateSurroundingHeatmap(PathFinderSectorTileData currentTileData, PathFinderPath path, Vector3 targetPosition, int depth = -1, int currentDistanceToTarget = 0)
        {
            if (currentTileData.GetPosition() == targetPosition && currentTileData.flowFieldDistanceToTarget < 0)
            {
                currentTileData.flowFieldDistanceToTarget = 0;
                currentTileData.flowFieldSurroundingTargetTileData = currentTileData;
            }
            
            var neighboursToEvaluate = new List<PathFinderSectorTileData>();
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if (neighbourTileData == null)
                {
                    neighbourTileData = new PathFinderSectorTileData(neighbourTile);
                    path.AddTileData(neighbourTileData);
                }
                
                if (neighbourTileData.flowFieldDistanceToTarget < 0 ||
                    (neighbourTileData.flowFieldSurroundingTargetTileData.Equals(currentTileData) &&
                     neighbourTileData.flowFieldDistanceToTarget > currentDistanceToTarget + 1))
                {
                    neighbourTileData.flowFieldLastTile = currentTileData.tile;
                    neighbourTileData.flowFieldLastTileData = currentTileData;
                    neighbourTileData.flowFieldDistanceToTarget = currentDistanceToTarget + 1;
                    neighbourTileData.flowFieldSurroundingTargetTileData = currentTileData;
                    //neighboursToEvaluate.Add(neighbourTileData);
                }
                neighboursToEvaluate.Add(neighbourTileData);
            });

            if (depth != 0)
            {
                var newDepth = depth == -1 ? -1 : depth - 1;
                foreach (var neighbourTileData in neighboursToEvaluate)
                {
                    if (neighbourTileData == null) continue;
                    //yield return StartCoroutine(GenerateHeatmap(neighbourTileData, sector, path, targetPosition));
                    IEnumerator genItr = GenerateSurroundingHeatmap(neighbourTileData, path, targetPosition, newDepth, currentDistanceToTarget+1);
                    while (genItr.MoveNext())
                    {
                
                    }
                }
            }
        

            yield return 0;
        }*/
        
        /*public void GenerateSurroundingVectorField(PathFinderSectorTileData tileData, PathFinderPath path)
{
IEnumerator x = GenerateSurroundingVectorField(tileData, tileData.tile.sector, path,
    proximityFlowFieldRadius);
while (x.MoveNext())
{
                        
}
}*/
        
        /*public IEnumerator FindFlowFieldPath(PathFinderPath path, Action<PathFinderPath> onDoneAction = null)
    {
        var flowFields = new Dictionary<PathFinderSector, PathFinderFlowField>();


        var endPositionTilesTMPTEST = new List<PathFinderSectorTileData>();
        for (int i = 0; i < path.tilePath.Count; i++)
        {
            var currentTileData = path.tilePath[i];
            if(currentTileData == null || currentTileData.tile.sector == null)
                continue;
            
            if (i == path.tilePath.Count-1 || path.tilePath[i+1].tile.sector != currentTileData.tile.sector)
            {
                PathFinderFlowField secFlowField;
                
                if (!flowFields.ContainsKey(currentTileData.tile.sector))
                {
                    secFlowField = (PathFinderFlowField) currentTileData.tile.sector.pathFinderFlowFieldTemplate?.Clone();

                    if (secFlowField == null)
                    {
                        var rowsX = (int)(currentTileData.tile.sector.bounds.size.x / currentTileData.tile.sector.sectorTileSize);
                        var rowsZ = (int)(currentTileData.tile.sector.bounds.size.z / currentTileData.tile.sector.sectorTileSize);
                        secFlowField = new PathFinderFlowField(currentTileData.tile.sector.sectorTileSize, rowsX, rowsZ);
                    }
                    flowFields.Add(currentTileData.tile.sector, secFlowField);
                }
                else
                {
                    secFlowField = flowFields[currentTileData.tile.sector];
                }

                secFlowField.targetPosition = currentTileData.tile.position;
                if(debugPathGeneration)
                    Debug.Log("Start Gen HeatMap "+currentTileData.tile.sector);
                secFlowField.GenerateHeatmap(currentTileData, currentTileData.tile.sector, path);
                //StartCoroutine(secFlowField.GenerateHeatmap(currentTile, currentTile.sector));
                //yield return new WaitForSeconds(1); // TODO SUPER UGLY HACK;; Need to wait until the previous coroutine is done, then execute next coroutine.
                //Debug.Log("Start Gen VecField");
                if(debugPathGeneration)
                    Debug.DrawRay(currentTileData.GetPosition(), Vector3.up*5, Color.cyan,30f);
                
                StartCoroutine(GenerateVectorField(currentTileData, currentTileData.tile.sector, path));
                
                
                //secFlowField.GenerateVectorField(currentTile, currentTile.sector);
                endPositionTilesTMPTEST.Add(currentTileData);
            }
        }

        path.flowFieldAvailable = true;
        if (debugPathGeneration)
        {
            endPositionTilesTMPTEST.ForEach(entry =>
                StartCoroutine(DrawFlowFieldTEMPTEST(entry, entry.tile.sector, path)));
            //StartCoroutine(DrawFlowFieldTEMPTEST(path.tilePath[0], path.tilePath[0].tile.sector, path));
        }
        
        
        Debug.Log("Done with function...");
        onDoneAction?.Invoke(path); // TODO: Theres a chance that not all vectorfields are calculated at this point...
        yield return 0;
    }*/
        
        /*private IEnumerator GenerateVectorField(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
        {
            if (currentTileData?.tile == null || !currentTileData.flowFieldDirection.Equals(Vector3.negativeInfinity))
                yield break;

            /*if (currentTileData.lastTile != null && currentTileData.lastTile.sector != currentTileData.tile.sector)
        {
            currentTileData.lastTileData.flowFieldDirection =
                currentTileData.lastTile.position - currentTileData.GetPosition();
        }#1#
        
            // Calculate Flow Field Direction
            PathFinderSectorTileData leftTileData = null, rightTileData = null, topTileData = null, bottomTileData = null;
            PathFinderSectorTile leftTile = currentTileData.tile.GetLeftTile(),
                rightTile = currentTileData.tile.GetRightTile(),
                topTile = currentTileData.tile.GetTopTile(),
                bottomTile = currentTileData.tile.GetBottomTile();
            if(leftTile != null) path.tileDataList.TryGetValue(leftTile, out leftTileData);
            if(rightTile != null) path.tileDataList.TryGetValue(rightTile, out rightTileData);
            if(topTile != null) path.tileDataList.TryGetValue(topTile, out topTileData);
            if(bottomTile != null) path.tileDataList.TryGetValue(bottomTile, out bottomTileData);
        
            var dirVector = new Vector3((leftTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (rightTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget), 
                0, 
                (bottomTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (topTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget));
            currentTileData.flowFieldDirection = dirVector.normalized;

        
            /#1#/ Debug Stuff
        if (debugPathGeneration && Mathf.Approximately(currentTileData.flowFieldDistanceToTarget, 0))
        {
            Debug.Log("@TARGET : "+currentTileData.flowFieldDirection+" ;; "+dirVector+" ;; "+dirVector.normalized);
            Debug.Log("LEFT: "+leftTileData?.flowFieldDistanceToTarget+
                      " ;;right: "+rightTileData?.flowFieldDistanceToTarget+
                      " ;;top: "+topTileData?.flowFieldDistanceToTarget+
                      " ;;bottom: "+bottomTileData?.flowFieldDistanceToTarget);
            
            if(leftTileData != null)
                Debug.DrawRay(leftTileData.GetPosition(), Vector3.up * 5, Color.red, 30f);
            if(rightTileData != null)
                Debug.DrawRay(rightTileData.GetPosition(), Vector3.up * 5, Color.red, 30f);
            if(topTileData != null)
                Debug.DrawRay(topTileData.GetPosition(), Vector3.up * 5, Color.blue, 30f);
            if(bottomTileData != null)
                Debug.DrawRay(bottomTileData.GetPosition(), Vector3.up * 5, Color.blue, 30f);
        }#1#
            
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if (neighbourTileData == null || !neighbourTileData.flowFieldDirection.Equals(Vector3.negativeInfinity)) return;
            
                var generateVectorField = GenerateVectorField(neighbourTileData, sector, path);
                while(generateVectorField.MoveNext())
                {
                }
            });
            
            yield return null;
        }*/

        #endregion


        // Currently only with Terrains
        
        [FoldoutGroup("Sector Generation"), Button("Generate Sectors")]
        public void CreateSectors()
        {
            // TODO Not considering potentially already existing sectors.
            
            var terrains = Terrain.activeTerrains;
            
            // TODO Sectors are currently limited to cover areas within one terrain.
            foreach (var terrain in terrains)
            {
                var terrainSize = terrain.terrainData.size;
                var sectorsX = Mathf.CeilToInt(terrainSize.x / maxSectorSize);
                var sectorsZ = Mathf.CeilToInt(terrainSize.z / maxSectorSize);
                var terrainLowerLeftPos = terrain.GetPosition();

                for (int x = 0; x < sectorsX; x++)
                {
                    for (int z = 0; z < sectorsZ; z++)
                    {
                        var size = new Vector3(Mathf.Clamp(maxSectorSize, 0, terrainSize.x - (x+1)*(maxSectorSize/2)),
                            Mathf.Clamp(maxSectorSize, 0, terrainSize.y),
                            Mathf.Clamp(maxSectorSize, 0, terrainSize.z-(z+1)*(maxSectorSize/2)));

                        
                        var center = terrainLowerLeftPos + new Vector3(x*maxSectorSize,0,z*maxSectorSize) + (size / 2);
                        Debug.Log(center+" -> tLLPos: "+terrainLowerLeftPos+" -> "+new Vector3(x*maxSectorSize,0,z*maxSectorSize)+" -> Size/2 "+(size / 2));
                        
                        var sectorBounds = new Bounds(center,size);
                        PathFinderSector sector = CreateSector(sectorBounds);
                        
                    }
                }
            }
        }

        public PathFinderSector CreateSector(Bounds bounds)
        {
            GameObject sectorGO = new GameObject("Sector");
            sectorGO.transform.position = bounds.center;
            sectorGO.layer = sectorLayer;
            PathFinderSector pfs = sectorGO.AddComponent<PathFinderSector>();
            pfs.bounds = bounds;
            pfs.sectorTileSize = 1;
            pfs.walkableMask = walkableMask;
            pfs.ignoreMask = ignoreMask;
            pfs.tileObjectPrefab = tileObjectPrefab;
            
            BoxCollider bc = sectorGO.GetComponent<BoxCollider>() ?? sectorGO.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            bc.size = bounds.size;
            
            return pfs;
        }



        /*public void CreateSectors()
    {
        var walkableObjects = GetWalkableGameObjects();
    }

    private List<GameObject> GetWalkableGameObjects()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        var walkableObjects = new List<GameObject>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            if (((1 << gameObjects[i].layer) & walkableLayers) != 0)
            {
                walkableObjects.Add(gameObjects[i]);
            }
        }

        return walkableObjects;
    }*/
    }
}

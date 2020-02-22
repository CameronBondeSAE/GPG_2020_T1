using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class PathFinderController : MonoBehaviour
{
    public bool goToNearestPossiblePos = true;
    //private PathFinderSector[,] sectors;
    
    public List<PathFinderSector> sectors;
    
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
    
    // TODO SUPER HACKY & POTENTIALLY BUGGY DEBUG SYSTEM
    [FoldoutGroup("Debug")]
    public bool debugPathGeneration = false;
    [FoldoutGroup("Debug"), ShowInInspector]
    public List<PathFinderSectorTileData> debugPathList = new List<PathFinderSectorTileData>();
    [FoldoutGroup("Debug")]
    public float debugPathGenerationDelay = 0.0001f; // seconds
    [FoldoutGroup("Debug")]
    public float debugPathGenerationDataDeletionDelay = 3f;
    [FoldoutGroup("Debug")]
    public Color32 debugStartColor = new Color32(0,0,255,200);
    [FoldoutGroup("Debug")]
    public Color32 debugEndColor = new Color32(255,200,50,200);
    [FoldoutGroup("Debug")]
    public Color32 debugGreenColor = new Color32(50,255,50,200);
    [FoldoutGroup("Debug")]
    public Color32 debugRedColor = new Color32(255,0,0,200);
    [FoldoutGroup("Debug")]
    public Color32 debugPathColor = new Color32(0,0,0,255);
    
    [FoldoutGroup("Debug")]
    public readonly Vector3 debugEndPointSize = new Vector3(0.4f, 2.5f, 0.4f);
    [FoldoutGroup("Debug")]
    public readonly Vector3 debugWaypointPointSize = new Vector3(0.4f, 1.5f, 0.4f);
    
    [FoldoutGroup("Debug")]
    public GameObject testStartPos;
    [FoldoutGroup("Debug")]
    public GameObject testEndPos;
    [FoldoutGroup("Debug")]
    public bool calculateFlowField;

    [FoldoutGroup("Debug"), ShowInInspector]
    public List<PathFinderSectorTileData> waypointList;
    [FoldoutGroup("Debug"), Button("Find Path"), DisableInEditorMode]
    public void TestFindPath()
    {
        if (testStartPos == null || testEndPos == null)
            return;

        /*Action<List<PathFinderSectorTileData>> onDoneFunc = list =>
        {
            Debug.Log("Done calculating path. " + list?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(list));
            }
            waypointList = list;
        };*/
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating path. " + path.tilePath?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(path));
            }
            waypointList = path.tilePath;
        };
        //StartCoroutine(FindPath(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc));
        FindPathTo(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc);
    }
    
    [FoldoutGroup("Debug"), Button("Find Path (Proximity FlowField)"), DisableInEditorMode]
    public void TestFindPathProximityFlowField()
    {
        if (testStartPos == null || testEndPos == null)
            return;

        /*Action<List<PathFinderSectorTileData>> onDoneFunc = list =>
        {
            Debug.Log("Done calculating path. " + list?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(list));
            }
            waypointList = list;
        };*/
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating (flowfield proximity) path. " + path.tilePath?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPathInProximity(path));
            }
            waypointList = path.tilePath;
        };
        //StartCoroutine(FindPath(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc));
        FindPathTo(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc);
    }

    private IEnumerator LoadSector(PathFinderSector sector, bool reload = false, bool connectSector = false)
    {
        if (sectors.Contains(sector) && !reload) // TODO Maybe Reload sector? Recalculate?
            yield break;
        sectors.Remove(sector);
        
        sector.CreateSectorTileGrid();
        if(connectSector)
            ConnectSector(sector);
        
        
        sectors.Add(sector);
        yield return 0;
    }

    private IEnumerator ConnectSector(PathFinderSector sector)
    {
        var connectingSectors = GetConnectingSectors(sector);
        foreach (var borderTileObject in sector.borderTileObjects)
        {
            if(borderTileObject.Tile == null)
                continue;
            
            borderTileObject.Tile.neighbourTiles.AddRange(borderTileObject.GetNeighbourTileObjects(true).Select(obj => obj.Tile).ToList());
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

    public void FindPathTo(Vector3 startPos, Vector3 targetPos, Action<PathFinderPath> onDoneCallback = null)
    {
        //Debug.Log(startPos+" -- "+targetPos);
        StartCoroutine(FindPath(startPos, targetPos, onDoneCallback));
    }
    
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Action<PathFinderPath> onDoneCallback = null)
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
        
        if (debugPathGeneration)
        {
            startTileData.isStart = true;
            startTileData.isGreen = true;
            debugPathList.Add(startTileData);
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
            if (debugPathGeneration)
                currentTileData.isGreen = false;
            redTilesData.Add(currentTileData);
            if (debugPathGeneration)
                currentTileData.isRed = true;

            if (currentTileData.tile.Equals(targetTile) || currentTileData.tile.neighbourTiles == null || currentTileData.tile.neighbourTiles.Count == 0)
            {
                if (goToNearestPossiblePos || currentTileData.tile.Equals(targetTile))
                {
                    targetTileData = currentTileData;
                    if (debugPathGeneration)
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

                if (debugPathGeneration && !debugPathList.Contains(neighbourTileData))
                {
                    debugPathList.Add(neighbourTileData);
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
                        if (debugPathGeneration)
                            neighbourTileData.isGreen = true;
                    }
                }
            }
            if (debugPathGeneration && debugPathGenerationDelay > 0)
                yield return new WaitForSeconds(debugPathGenerationDelay);
        }

        var waypointDataList = CreateWaypointsListFromTileData(targetTileData);
        path.tilePath = waypointDataList;
        onDoneCallback?.Invoke(path);

        if (debugPathGeneration)
        {
            waypointDataList?.ForEach(wp => { wp.isPath = true; });
            
            yield return new WaitForSeconds(debugPathGenerationDataDeletionDelay);
            debugPathList = debugPathList.Except(tmpDebugPathList).ToList(); // TODO SUPER HACKY & POTENTIALLY BUGGY
        }

        yield return 0;
    }

    public Color32[] colors = new Color32[15];
    public IEnumerator FindFlowFieldPath(PathFinderPath path, Action<PathFinderPath> onDoneAction = null)
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
    }

    public int proximityFlowFieldRadius = 2;
    public IEnumerator FindFlowFieldPathInProximity(PathFinderPath path, Action<PathFinderPath> onDoneAction = null)
    {
        path.flowFieldAvailable = true;
        // PATH MUST BE End to Start sorted! 
        
        //var flowFields = new Dictionary<PathFinderSector, PathFinderFlowField>();


        var endPositionTilesTMPTEST = new List<PathFinderSectorTileData>();
        
        //PathFinderFlowField secFlowField = null;
        var inverseTilePath = new List<PathFinderSectorTileData>(path.tilePath);
        Debug.Log(inverseTilePath.Count+" <<<< LENGTH");
        inverseTilePath.Reverse();
        for (int i = 0; i < inverseTilePath.Count; i++)
        {
            var currentTileData = inverseTilePath[i];
            if(currentTileData == null || currentTileData.tile.sector == null)
                continue;


            
            // Calculate FlowField
            if (i == 0/*inverseTilePath.Count-1*/ || inverseTilePath[i-1].tile.sector != currentTileData.tile.sector)
            {
                if(debugPathGeneration)
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
            /*

            Action DebugDrawFlowField = () =>
            {
                Debug.Log("Draw Flow Field...");
                StartCoroutine(DrawFlowFieldTEMPTESTNEW(currentTileData, currentTileData.tile.sector, path));
                //if (debugPathGeneration) endPositionTilesTMPTEST.ForEach(entry => StartCoroutine(DrawFlowFieldTEMPTESTNEW(entry, entry.tile.sector, path)));
            };*/


            //StartCoroutine(GenerateHeatmap(currentTileData, currentTileData.tile.sector, path, currentTileData.tile.position, GenVecFieldAction));
            //yield return StartCoroutine(GenerateHeatmap(currentTileData, currentTileData.tile.sector, path, currentTileData.tile.position, GenVecFieldAction));

            /*StartCoroutine(GenerateSurroundingVectorField(currentTileData, currentTileData.tile.sector,
                path, proximityFlowFieldRadius));*/
        }
        
        foreach (var t in inverseTilePath)
        {
            Debug.Log("Gen Proximity Vec Field...");
            IEnumerator x = GenerateSurroundingVectorField(t, path, proximityFlowFieldRadius);
            while (x.MoveNext())
            {
                                
            }
        }

        endPositionTilesTMPTEST.ForEach(tileData =>
        {
            if (tileData.lastTile != null)
            {
                tileData.flowFieldDirection = tileData.GetPosition() - tileData.lastTile.position;
            }
            StartCoroutine(DrawFlowFieldTEMPTESTNEW(tileData, tileData.tile.sector, path));
        });
        
        

        /*if (debugPathGeneration)
        {
            yield return new WaitForSeconds(5);
            endPositionTilesTMPTEST.ForEach(entry =>
                StartCoroutine(DrawFlowFieldTEMPTEST(entry, entry.tile.sector, path)));
            //StartCoroutine(DrawFlowFieldTEMPTEST(path.tilePath[0], path.tilePath[0].tile.sector, path));
        }*/
        
        
        Debug.Log("Done with function...");
        onDoneAction?.Invoke(path); // TODO: Theres a chance that not all vectorfields are calculated at this point...
        yield return 0;
    }

    private IEnumerator DrawFlowFieldTEMPTEST(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
    {
        var rayStartPos = currentTileData.GetPosition();
        rayStartPos.y += 0.2f;
        //Debug.Log("GEN VEC FIELD Pos: "+currentTile.position+" Vector: "+dirVector+" neighbours: "+currentTile.neighbourTiles.Count);
        //Debug.DrawRay(currentTileData.GetPosition(), Vector3.up * currentTileData.flowFieldDistanceToTarget, colors[Mathf.Clamp((int)currentTileData.flowFieldDistanceToTarget,0,14)], 30f);
        Debug.DrawRay(rayStartPos-(currentTileData.flowFieldDirection/2), currentTileData.flowFieldDirection, Color.red, 30f);
        Debug.DrawRay(rayStartPos+(currentTileData.flowFieldDirection/2*.3f),currentTileData.flowFieldDirection*.3f, Color.yellow, 30f);
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

                yield return StartCoroutine(DrawFlowFieldTEMPTEST(neighbourTileData, sector, path));
            }
        }

        yield return 0;
    }

    private IEnumerator GenerateVectorField(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
    {
        if (currentTileData?.tile == null || !currentTileData.flowFieldDirection.Equals(Vector3.negativeInfinity))
            yield break;

        /*if (currentTileData.lastTile != null && currentTileData.lastTile.sector != currentTileData.tile.sector)
        {
            currentTileData.lastTileData.flowFieldDirection =
                currentTileData.lastTile.position - currentTileData.GetPosition();
        }*/
        
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

        
        /*// Debug Stuff
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
        }*/
            
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
    }
    
    public int c = 0;
    public int calcs = 0;
    public int noFlowFieldNum = 0;
    
    private IEnumerator GenerateSurroundingVectorField(PathFinderSectorTileData currentTileData, 
        PathFinderPath path, Action onDoneAction, int depth = -1)
    {
        yield return StartCoroutine(GenerateSurroundingVectorField(currentTileData, path, depth));
        onDoneAction?.Invoke();
        yield return 0;
    }
    
    // -1 = unlimited
    public IEnumerator GenerateSurroundingVectorField(PathFinderSectorTileData currentTileData, PathFinderPath path, int depth = -1)
    {
        if (currentTileData?.tile == null || depth == 0)
            yield break;
        
        /*if(depth == proximityFlowFieldRadius) // TODO HACK
            yield return new WaitForSeconds(2);*/
        
        c++;
        if (currentTileData.flowFieldDirection.Equals(Vector3.negativeInfinity))
        {
            calcs++;
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

            /*if (dirVector.Equals(Vector3.zero) && currentTileData.lastTileData != null){
                noFlowFieldNum++;

                dirVector.x = currentTileData.lastTileData.flowFieldDistanceToTarget -
                              currentTileData.flowFieldDistanceToTarget;
                dirVector.z = currentTileData.lastTileData.flowFieldDistanceToTarget -
                              currentTileData.flowFieldDistanceToTarget;
            }*/
            
            currentTileData.flowFieldDirection = dirVector.normalized;
        }

        var newDepth = depth == -1 ? depth : depth - 1;
        currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
        {
            if (neighbourTile == null) return;
            path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
            if (neighbourTileData == null || !neighbourTileData.flowFieldDirection.Equals(Vector3.negativeInfinity) || currentTileData.tile.sector != neighbourTile.sector) return;
            
            var generateVectorField = GenerateSurroundingVectorField(neighbourTileData, path, newDepth);
            while(generateVectorField.MoveNext())
            {
            }
        });
            
        yield return null;
    }

    /*public void GenerateSurroundingVectorField(PathFinderSectorTileData tileData, PathFinderPath path)
    {
        IEnumerator x = GenerateSurroundingVectorField(tileData, tileData.tile.sector, path,
            proximityFlowFieldRadius);
        while (x.MoveNext())
        {
                                
        }
    }*/
    
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
    
    private IEnumerator GenerateSurroundingHeatmap(PathFinderSectorTileData currentTileData,
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

    public PathFinderSector GetSectorByPoint(Vector3 point) // Point must be within a sector
    {
        return sectors.FirstOrDefault(s => s.ContainsPoint(point));
    }

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
    
    public PathFinderSectorTile GetNearestNode(Vector3 position, bool ignoreNonSectorPos = true)
    {
        var sector = GetSectorByPoint(position);
        
        if ((sector == null || sector.sectorTiles == null || sector.sectorTiles.Count <= 0) && ignoreNonSectorPos) return null;
        else if (!ignoreNonSectorPos && sector == null)
        {
            sector = GetNearestSector(position);
            if (sector == null || sector.sectorTiles == null || sector.sectorTiles.Count <= 0)
                return null;
        }
        
        PathFinderSectorTile nearestNode = null;
        var nearestNodeDist = 0f;
        
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

    private void OnDrawGizmos()
    {
        if(!debugPathGeneration || debugPathList == null || debugPathList.Count == 0)
            return;

        
        foreach (var tileData in debugPathList)
        {
            Gizmos.color = tileData.isStart ? debugStartColor : tileData.isEnd ? debugEndColor : tileData.isPath ? debugPathColor : tileData.isRed ? debugRedColor : tileData.isGreen ? debugGreenColor : new Color32(0,0,0,0);
            
            Gizmos.DrawCube(tileData.GetPosition(), tileData.isStart || tileData.isEnd ? debugEndPointSize : debugWaypointPointSize);
        }
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

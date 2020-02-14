using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
        sectorsInScene.ForEach(s => StartCoroutine(LoadSector(s, reloadExisting, false)));
        // Connect all sectors
        sectorsInScene.ForEach(s => StartCoroutine(ConnectSector(s)));
    }
    
    // TODO SUPER HACKY & POTENTIALLY BUGGY DEBUG SYSTEM
    [FoldoutGroup("Debug")]
    public bool debugPathGeneration = false;
    [FoldoutGroup("Debug"), ShowInInspector]
    public List<PathFinderSectorTile> debugPathList = new List<PathFinderSectorTile>();
    [FoldoutGroup("Debug")]
    public float debugPathGenerationDelay = 0.25f; // seconds
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
    [FoldoutGroup("Debug"), Button("Find Path"), DisableInEditorMode]
    public void TestFindPath()
    {
        if (testStartPos == null || testEndPos == null)
            return;
        
        StartCoroutine(FindPath(testStartPos.transform.position, testEndPos.transform.position, list => Debug.Log("Done calculating path. "+list?.Count)));
    }
    [FoldoutGroup("Debug"), Button("Test"), DisableInEditorMode]
    public void Test()
    {
        var tOrig = new PathFinderSectorTile();
        tOrig.position = transform.position;
        tOrig.sector = sectors[0];

        var tClone = (PathFinderSectorTile)tOrig.Clone();
        
        var list1 = new List<PathFinderSectorTile>(){tOrig};
        
        
        Debug.Log(list1.IndexOf(tOrig)+" <> "+list1.IndexOf(tClone)+" ;; "+tOrig.Equals(tClone));
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
        //Debug.Log("Found "+connectingSectors.Count+" connecting sectors");
        /*foreach (var conSec in connectingSectors)
        {
            if(conSec == null || conSec.borderTiles == null || conSec.borderTiles.Count == 0)
                return;
        }*/
        foreach (var borderTileObject in sector.borderTileObjects)
        {
            if(borderTileObject.Tile == null)
                continue;
            
            borderTileObject.Tile.neighbourTiles.AddRange(borderTileObject.GetNeighbourTileObjects(true).Select(obj => obj.Tile).ToList());
            //Debug.Log("border tile test. tileRect Pos: "+borderTile.tileRect.position+" ;; tileRect Center: "+borderTile.tileRect.center+" ;; tileRect size: "+borderTile.tileRect.size);
            //var hits = Physics.OverlapBox(borderTile.tileRect.position, borderTile.tileRect.size * .5f);

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

    
    
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Action<List<PathFinderSectorTile>> onDoneCallback = null)
    {
        var clonedTiles = new List<PathFinderSectorTile>();
        var tmpDebugPathList = new List<PathFinderSectorTile>();
        var startTile = (PathFinderSectorTile)GetNearestNode(startPos)?.Clone();
        clonedTiles.Add(startTile);
        
        var endTile = GetNearestNode(targetPos);

        if (startTile == null || endTile == null)
            yield break;
        
        startTile.GCost = 0;
        startTile.HCost = PathFinderSectorTile.CalculateHCost(startTile, endTile);
        
        if (debugPathGeneration)
        {
            startTile.isStart = true;
            startTile.isGreen = true;
            debugPathList.Add(startTile);
            tmpDebugPathList.Add(startTile);
        }
        
        var greenTiles = new List<PathFinderSectorTile>(){startTile};
        var redTiles = new List<PathFinderSectorTile>();
        
        PathFinderSectorTile finalEndNode = null;
        
        Debug.Log("PathFinder: Start while...");
        while (true) // TODO Add security break!
        {
            var currentTile = GetLowestFCostTile(greenTiles);
            
            if(currentTile == null)
                break;

            greenTiles.Remove(currentTile);
            if (debugPathGeneration)
                currentTile.isGreen = false;
            redTiles.Add(currentTile);
            if (debugPathGeneration)
                currentTile.isRed = true;
            /*

            if (debugPathGeneration)
                yield return new WaitForSeconds(debugPathGenerationDelay);*/

            if (currentTile.position.Equals(endTile.position) || currentTile.neighbourTiles == null || currentTile.neighbourTiles.Count == 0)
            {
                if (goToNearestPossiblePos || currentTile.position.Equals(endTile.position))
                {
                    finalEndNode = currentTile;
                    if (debugPathGeneration)
                        currentTile.isEnd = true;
                }
                break;
            }

            // Clone neighbour tiles
            //currentTile.neighbourTiles = currentTile.neighbourTiles.Select(nt => (PathFinderSectorTile)nt.Clone()).ToList();
            var newNeighbourTilesList = new List<PathFinderSectorTile>();
            currentTile.neighbourTiles.ForEach(nt =>
            {
                if (nt == null)
                    return;
                
                var clonedIndex = clonedTiles.IndexOf(nt);
                if (clonedIndex >= 0 )
                {
                    newNeighbourTilesList.Add(clonedTiles[clonedIndex]);
                }
                else
                {
                    var ntClone = (PathFinderSectorTile) nt.Clone();
                    clonedTiles.Add(ntClone);
                    newNeighbourTilesList.Add(ntClone);
                }
            });
            currentTile.neighbourTiles = newNeighbourTilesList;
            
            
            foreach (var neighbourTile in currentTile.neighbourTiles)
            {
                if(neighbourTile == null || redTiles.Contains(neighbourTile))
                    continue;

                if (debugPathGeneration && !debugPathList.Contains(neighbourTile))
                {
                    debugPathList.Add(neighbourTile);
                    tmpDebugPathList.Add(neighbourTile);
                }

                var neighbourGCost = Vector3.Distance(neighbourTile.position, currentTile.position) +
                                     currentTile.GCost;
                var neighbourHCost = PathFinderSectorTile.CalculateHCost(neighbourTile, endTile);
                var neighbourFCost = neighbourGCost + neighbourHCost;

                if ((neighbourTile.fCost >= 0 && neighbourFCost < neighbourTile.fCost) ||
                    !greenTiles.Contains(neighbourTile))
                {
                    neighbourTile.GCost = neighbourGCost;
                    neighbourTile.HCost = neighbourHCost;
                    neighbourTile.lastNode = currentTile;
                    
                    if (!greenTiles.Contains(neighbourTile))
                    {
                        greenTiles.Add(neighbourTile);
                        if (debugPathGeneration)
                            neighbourTile.isGreen = true;
                    }
                }
                
                /*////////////
                if ((!(neighbourTile.fCost >= 0) || !(neighbourFCost < neighbourTile.fCost)) &&
                    greenTiles.Contains(neighbourTile)) continue;
                
                neighbourTile.GCost = neighbourGCost;
                neighbourTile.HCost = neighbourHCost;
                neighbourTile.lastNode = currentTile;

                if (greenTiles.Contains(neighbourTile)) continue;
                
                greenTiles.Add(neighbourTile);
                if (debugPathGeneration)
                    neighbourTile.isGreen = true;*/
                
                /*if (debugPathGeneration)
                    yield return new WaitForSeconds(debugPathGenerationDelay);*/

                 //yield return 0;
            }
            yield return null;
        }
        
        Debug.Log("FindPath: Done w/ While Loop");

        var waypointList = CreateWaypointsListFromTile(finalEndNode);
        onDoneCallback?.Invoke(waypointList);

        if (debugPathGeneration)
        {
            waypointList?.ForEach(wp => { wp.isPath = true; });
            
            yield return new WaitForSeconds(debugPathGenerationDataDeletionDelay);
            debugPathList = debugPathList.Except(tmpDebugPathList).ToList(); // TODO SUPER HACKY & POTENTIALLY BUGGY
        }
        
        Debug.Log("FindPath: End");

        yield return 0;
    }
    
    private static List<PathFinderSectorTile> CreateWaypointsListFromTile(PathFinderSectorTile endTile)
    {
        var waypoints = new List<PathFinderSectorTile>();
        if (endTile == null)
            return waypoints;

        var currentTile = endTile;
        while (currentTile.lastNode != null)
        {
            waypoints.Add(currentTile); // .position
            currentTile = currentTile.lastNode;
        }

        return waypoints;
    }

    public PathFinderSector GetSectorByPoint(Vector3 point)
    {
        return sectors.FirstOrDefault(s => s.ContainsPoint(point));
    }
    
    public PathFinderSectorTile GetNearestNode(Vector3 position)
    {
        var sector = GetSectorByPoint(position);
        
        if (sector == null) return null;
        
        PathFinderSectorTile nearestNode = null;
        var nearestNodeDist = 0f;
        
        if (sector.sectorTiles == null || sector.sectorTiles.Count <= 0) return null;
        
        foreach (var tile in sector.sectorTiles)
        {
            var dist = Vector3.Distance(tile.position, position);
            
            if (!(dist < nearestNodeDist) && nearestNode != null) continue;
            
            nearestNode = tile;
            nearestNodeDist = dist;
        }

        return nearestNode;
    }

    private PathFinderSectorTile GetLowestFCostTile(List<PathFinderSectorTile> tiles)
    {
        PathFinderSectorTile lowestCostTile = null;
        foreach (var tile in tiles)
        {
            if (lowestCostTile == null || tile.fCost < lowestCostTile.fCost)
            {
                lowestCostTile = tile;
            }   
        }

        return lowestCostTile;
    }

    private void OnDrawGizmos()
    {
        if(!debugPathGeneration || debugPathList == null || debugPathList.Count == 0)
            return;

        
        foreach (var tile in debugPathList)
        {
            Gizmos.color = tile.isStart ? debugStartColor : tile.isEnd ? debugEndColor : tile.isPath ? debugPathColor : tile.isRed ? debugRedColor : tile.isGreen ? debugGreenColor : new Color32(0,0,0,0);
            
            Gizmos.DrawCube(tile.position, tile.isStart || tile.isEnd ? debugEndPointSize : debugWaypointPointSize);
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

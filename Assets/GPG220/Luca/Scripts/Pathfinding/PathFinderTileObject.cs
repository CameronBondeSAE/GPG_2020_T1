using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathFinderTileObject : MonoBehaviour
{
    [ShowInInspector]
    private PathFinderSectorTile _tile;

    public PathFinderSectorTile Tile
    {
        get => _tile;
        set
        {
            _tile = value;
            InitObject();
        }
    }

    public PathFinderSector sector = null;
    private List<PathFinderTileObject> neighbourTileObjects;

    public BoxCollider boxCollider;

    public bool drawBox = false;
    public Color32 boxColor = new Color32(100,100,150,100);
    // Start is called before the first frame update
    private void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
        InitObject();
        
    }
    
    public void InitObject()
    {
        if (Tile == null)
            return;
        
        boxCollider.size = new Vector3(Tile.tileRect.size.x,.5f,Tile.tileRect.size.y);
        boxCollider.center = Vector3.zero;
        //transform.position = Tile.position;
    }

    public List<PathFinderTileObject> GetNeighbourTileObjects(bool recalculate = false)
    {
        if (Tile == null || ((neighbourTileObjects != null && neighbourTileObjects.Count > 0) && !recalculate)) return neighbourTileObjects;
        
        neighbourTileObjects = new List<PathFinderTileObject>();

        var otherTileObjects = Physics.OverlapBox(Tile.position, boxCollider.size * .5f,
            Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)));
        
        neighbourTileObjects = otherTileObjects.Where(col =>
        {
            var pfto = col.GetComponent<PathFinderTileObject>();
            return pfto != null && pfto.sector != sector;
        }).Select(col => col.GetComponent<PathFinderTileObject>()).ToList();

        return neighbourTileObjects;
    }

    private void OnDrawGizmos()
    {
        if (drawBox)
        {
            Gizmos.color = boxColor;
            Gizmos.DrawCube(transform.position, boxCollider.size);
        }
    }
}

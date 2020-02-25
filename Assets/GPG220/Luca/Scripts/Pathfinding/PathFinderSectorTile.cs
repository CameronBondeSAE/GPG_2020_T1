using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathFinderSectorTile : IEquatable<PathFinderSectorTile>
{
    public PathFinderSector sector;
    public Vector3 position;
    public Rect tileRect;

    public float terrainSlope = -1f;

    //public readonly List<PathFinderSectorTile> neighbourTiles = new List<PathFinderSectorTile>();
    public readonly Dictionary<PathFinderSectorTile, float> neighbourTiles = new Dictionary<PathFinderSectorTile, float>(); // <Tile, slope>
    
    [ShowInInspector]
    private PathFinderSectorTile _leftTile;
    public PathFinderSectorTile GetLeftTile(float maxSlope = -1, float stepHeight = -1) // -1 do deactivate
    {
        if (_leftTile == null)
            FindAdjacentTiles();
        return _leftTile != null && (maxSlope < 0 && stepHeight < 0) || CanTraverseToNeighbour(_leftTile, maxSlope, stepHeight) ? _leftTile : null;
        //return _leftTile == null || (maxSlope >= 0 && (neighbourTiles[_leftTile] > maxSlope || _leftTile.terrainSlope > maxSlope)) ? null : _leftTile;
    }

    [ShowInInspector]
    private PathFinderSectorTile _rightTile;
    public PathFinderSectorTile GetRightTile(float maxSlope = -1, float stepHeight = -1)
    {
        if (_rightTile == null)
            FindAdjacentTiles();
        return _rightTile != null && (maxSlope < 0 && stepHeight < 0) || CanTraverseToNeighbour(_rightTile, maxSlope, stepHeight) ? _rightTile : null;
        //return _rightTile == null || (maxSlope >= 0 && (neighbourTiles[_rightTile] > maxSlope || _rightTile.terrainSlope > maxSlope)) ? null : _rightTile;
    }
    [ShowInInspector]
    private PathFinderSectorTile _topTile;
    public PathFinderSectorTile GetTopTile(float maxSlope = -1, float stepHeight = -1)
    {
        if (_topTile == null)
            FindAdjacentTiles();
        return _topTile != null && (maxSlope < 0 && stepHeight < 0) || CanTraverseToNeighbour(_topTile, maxSlope, stepHeight) ? _topTile : null;
        //return _topTile == null || (maxSlope >= 0 && (neighbourTiles[_topTile] > maxSlope || _topTile.terrainSlope > maxSlope)) ? null : _topTile;
    }
    [ShowInInspector]
    private PathFinderSectorTile _bottomTile;
    public PathFinderSectorTile GetBottomTile(float maxSlope = -1, float stepHeight = -1)
    {
        if (_bottomTile == null)
            FindAdjacentTiles();
        return _bottomTile != null && (maxSlope < 0 && stepHeight < 0) || CanTraverseToNeighbour(_bottomTile, maxSlope, stepHeight) ? _bottomTile : null;
        //return _bottomTile == null || (maxSlope >= 0 && (neighbourTiles[_bottomTile] > maxSlope || _bottomTile.terrainSlope > maxSlope)) ? null : _bottomTile;
    }

    private void FindAdjacentTiles()
    {
        if (neighbourTiles == null || neighbourTiles.Count == 0)
            return;
        foreach (var neighbour in neighbourTiles.Keys)
        {
            var horSame = Mathf.Approximately(neighbour.position.x, position.x);
            var vertSame = Mathf.Approximately(neighbour.position.z,position.z);

            if (horSame && !vertSame)
            {
                if (neighbour.position.z > position.z) _topTile = neighbour;
                else _bottomTile = neighbour;

                continue;
            }
            if (!horSame && vertSame)
            {
                if (neighbour.position.x > position.x) _rightTile = neighbour;
                else _leftTile = neighbour;
            }
        }
    }

    public override bool Equals(object obj)
    {
        var other = (PathFinderSectorTile) obj;
        return Equals(sector, other?.sector) && position.Equals(other?.position);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((sector != null ? sector.GetHashCode() : 0) * 397) ^ position.GetHashCode();
        }
    }

    public bool Equals(PathFinderSectorTile other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals(sector, other.sector) && position.Equals(other.position);
    }

    public float GetSlopeBetweenTiles(PathFinderSectorTile otherTile)
    {
        //var slopeAtPos = 0f;
        var forward = otherTile.position - position;
        forward.y = 0;
        // TODO Maybe not accurate
        return Vector3.Angle(otherTile.position - position, forward);
        //return (Mathf.Approximately(position.y,0) || Mathf.Approximately(otherTile.position.y,0)) ? 0 : Vector3.Angle(otherTile.position-position, Vector3.up);
        /*var terrainAtPos1 = GetTerrainAtPos(tile1.position);
        var terrainAtPos2 = GetTerrainAtPos(tile2.position);
        if (terrainAtPos1 != null && terrainAtPos2 != null)
        {
            var terrainPos1 = tile1.position - terrainAtPos1.transform.position;
            var posOnTerrain1 = new Vector2(terrainPos1.x / terrainAtPos1.terrainData.size.x, terrainPos1.z / terrainAtPos1.terrainData.size.z);
            var terrainPos2 = tile2.position - terrainAtPos2.transform.position;
            var posOnTerrain2 = new Vector2(terrainPos2.x / terrainAtPos2.terrainData.size.x, terrainPos2.z / terrainAtPos2.terrainData.size.z);
            slopeAtPos = terrainAtPos.terrainData.GetSteepness(posOnTerrain.x,posOnTerrain.y);
        }*/
    }

    public bool HasImpassableNeighbour(float maxSlope, float stepHeight)
    {
        return neighbourTiles.Count(t => !CanTraverseToNeighbourInsecure(t.Key, t.Value, maxSlope, stepHeight)) > 0;
    }

    public bool CanTraverseToNeighbour(PathFinderSectorTile neighbour, float maxSlope, float stepHeight)
    {
        return neighbour != null &&
               neighbourTiles.ContainsKey(neighbour) &&
               ((neighbour.terrainSlope <= maxSlope &&
                 neighbourTiles[neighbour] <= maxSlope) ||
                Mathf.Abs(position.y-neighbour.position.y) <= stepHeight);
    }

    // Insecure cuz we're not actually checking if given neighbour is an acutal neighbour - but more efficient.
    public bool CanTraverseToNeighbourInsecure(PathFinderSectorTile neighbour, float slopeToNeighbour, float maxSlope, float stepHeight)
    {
        return neighbour != null &&
               ((neighbour.terrainSlope <= maxSlope &&
                 slopeToNeighbour <= maxSlope) ||
                Mathf.Abs(position.y-neighbour.position.y) <= stepHeight);
    }
    
}

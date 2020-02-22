using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathFinderSectorTile : IEquatable<PathFinderSectorTile>
{
    public PathFinderSector sector;
    public Vector3 position;
    public Rect tileRect;

    public readonly List<PathFinderSectorTile> neighbourTiles = new List<PathFinderSectorTile>();
    
    [ShowInInspector]
    private PathFinderSectorTile _leftTile;
    public PathFinderSectorTile GetLeftTile()
    {
        if (_leftTile == null)
            FindAdjacentTiles();
        return _leftTile;
    }

    [ShowInInspector]
    private PathFinderSectorTile _rightTile;
    public PathFinderSectorTile GetRightTile()
    {
        if (_rightTile == null)
            FindAdjacentTiles();
        return _rightTile;
    }
    [ShowInInspector]
    private PathFinderSectorTile _topTile;
    public PathFinderSectorTile GetTopTile()
    {
        if (_topTile == null)
            FindAdjacentTiles();
        return _topTile;
    }
    [ShowInInspector]
    private PathFinderSectorTile _bottomTile;
    public PathFinderSectorTile GetBottomTile()
    {
        if (_bottomTile == null)
            FindAdjacentTiles();
        return _bottomTile;
    }

    private void FindAdjacentTiles()
    {
        if (neighbourTiles == null || neighbourTiles.Count == 0)
            return;
        foreach (var neighbour in neighbourTiles)
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
    
}

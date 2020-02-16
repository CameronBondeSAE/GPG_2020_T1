using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderSectorTile : ICloneable, IEquatable<PathFinderSectorTile>
{
    public PathFinderSector sector;
    public Vector3 position;
    public Rect tileRect;

    public List<PathFinderSectorTile> neighbourTiles = new List<PathFinderSectorTile>();

    // Only needed for Path Calculation
    private float _gCost = 0; // Dist to start

    public float GCost
    {
        get => _gCost;
        set
        {
            _gCost = value;
            fCost = GCost + HCost;
        }
    }

    private float _hCost = 0; // Dist to end

    public float HCost
    {
        get => _hCost;
        set
        {
            _hCost = value;
            fCost = HCost + GCost;
        }
    }

    public float fCost = -1; // gCost + hCost
    public PathFinderSectorTile lastNode;
    public bool isTempCopy = false;

    // TMP Debug stuff
    public bool isRed = false;
    public bool isGreen = false;
    public bool isStart = false;
    public bool isEnd = false;
    public bool isPath = false;

    // FlowField stuff
    public float flowFieldDistanceToTarget = -1f;
    public Vector3 flowFieldDirection = Vector3.zero;
    public PathFinderSectorTile flowFieldLastNode;

    private PathFinderSectorTile _leftTile;
    public PathFinderSectorTile GetLeftTile()
    {
        if (_leftTile == null)
            FindAdjacentTiles();
        return _leftTile;
    }

    private PathFinderSectorTile _rightTile;
    public PathFinderSectorTile GetRightTile()
    {
        if (_rightTile == null)
            FindAdjacentTiles();
        return _rightTile;
    }
    private PathFinderSectorTile _topTile;
    public PathFinderSectorTile GetTopTile()
    {
        if (_topTile == null)
            FindAdjacentTiles();
        return _topTile;
    }
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
            var horSame = Mathf.Approximately(neighbour.position.x, position.x);//neighbour.position.x.Equals(position.x);
            var vertSame = Mathf.Approximately(neighbour.position.z,position.z);//neighbour.position.z.Equals(position.z);

            if (horSame && !vertSame)
            {
                if (neighbour.position.z > position.z)
                    _topTile = neighbour;
                else
                    _bottomTile = neighbour;
                break;
            }
            if (!horSame && vertSame)
            {
                if (neighbour.position.x > position.x)
                    _rightTile = neighbour;
                else
                    _leftTile = neighbour;
                break;
            }
        }
    }
    
    
    public static float CalculateHCost(PathFinderSectorTile fromTile, PathFinderSectorTile toTile)
    {
        return Vector3.Distance(fromTile.position, toTile.position);
    }
    
    public object Clone()
    {
        var clone = new PathFinderSectorTile
        {
            position = position,
            sector = sector,
            tileRect = tileRect,
            neighbourTiles = neighbourTiles,
            GCost = GCost,
            HCost = HCost,
            fCost = fCost,
            lastNode = lastNode,
            isTempCopy = true,
            isEnd = isEnd,
            isGreen = isGreen,
            isPath = isPath,
            isRed = isRed,
            isStart = isStart
        };

        return clone;
    }

    public override bool Equals(object obj)
    {
        var other = (PathFinderSectorTile) obj;
        return Equals(sector, other?.sector) && position.Equals(other?.position);
    }
/*

    protected bool Equals(PathFinderSectorTile other)
    {
        return Equals(sector, other.sector) && position.Equals(other.position);
    }*/

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
        //if (ReferenceEquals(this, other)) return true;
        return Equals(sector, other.sector) && position.Equals(other.position);
    }
}

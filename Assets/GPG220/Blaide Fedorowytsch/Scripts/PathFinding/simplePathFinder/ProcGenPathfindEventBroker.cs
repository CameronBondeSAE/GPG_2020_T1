using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using UnityEngine;

public class ProcGenPathfindEventBroker : MonoBehaviour
{
    public ProceduralGrowthSystem proceduralGrowthSystem;
    public NodeGrid nodeGrid;

    private void Start()
    {
        proceduralGrowthSystem.onUpdateProceduralGrowth += nodeGrid.UpdateGrid;
    }
}

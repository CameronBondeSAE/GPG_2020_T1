﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Pathfinding;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestUnitGroup : MonoBehaviour
{
    public PathFinderPath currentPath;
    public List<TestUnit> controlledUnits;
    
    public bool calculateFlowFieldPath = true;
    public PathFinderController pfController;
    
    public GameObject testTarget;
    [Button("Recalculate Path"), DisableInEditorMode]
    public void RecalculatePathToTarget()
    {
        controlledUnits?.ForEach(unit => unit.move = false);
        
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating path. " + path.tilePath?.Count);
            if (calculateFlowFieldPath)
            {
                Action<PathFinderPath> onFinallyDoneFunc = PathCalculationDone;
                StartCoroutine(pfController.FindFlowFieldPath(path, onFinallyDoneFunc));
            }

            currentPath = path;
        };
        pfController.FindPathTo(CalculateStartPosition(), testTarget.transform.position, onDoneFunc);
        //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
    }
    
    [Button("Recalculate Path (Proximity)"), DisableInEditorMode]
    public void RecalculatePathToTargetProximity()
    {
        controlledUnits?.ForEach(unit => unit.move = false);
        
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating proximity path. " + path.tilePath?.Count);
            if (calculateFlowFieldPath)
            {
                Action<PathFinderPath> onFinallyDoneFunc = PathCalculationDone;
                StartCoroutine(pfController.FindFlowFieldPathInProximity(path, onFinallyDoneFunc));
            }

            currentPath = path;
        };
        pfController.FindPathTo(CalculateStartPosition(), testTarget.transform.position, onDoneFunc);
        //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
    }

    public Vector3 CalculateStartPosition()
    {
        var center = Vector3.zero;

        if (controlledUnits.Count <= 0) return center;
        center = controlledUnits.Aggregate(center, (current, unit) => current + unit.transform.position);
        center /= ( controlledUnits.Count+1 );

        return center;
    }
    
    private void PathCalculationDone(PathFinderPath path)
    {
        controlledUnits?.ForEach(unit =>
        {
            unit.currentPath = path;
            unit.move = true;
        });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (pfController == null)
            pfController = FindObjectOfType<PathFinderController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

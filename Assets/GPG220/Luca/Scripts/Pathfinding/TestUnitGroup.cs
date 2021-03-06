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
    public List<MedicUnit> controlledMedicUnits;
    
    public bool calculateFlowFieldPath = true;
    public PathFinderController pfController;
    
    public GameObject testTarget;
/*    [Button("Recalculate Path"), DisableInEditorMode]
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
        pfController.FindPathTo(CalculateStartPosition(), testTarget.transform.position, false, onDoneFunc);
        //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
    }*/
    
    [Button("Recalculate Path (Proximity)"), DisableInEditorMode]
    public void RecalculatePathToTargetProximity()
    {
        if (controlledUnits == null || controlledUnits.Count == 0)
            return;
        
        controlledUnits?.ForEach(unit => unit.move = false);
        
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating proximity path. " + path.tilePath?.Count);
            /*if (calculateFlowFieldPath)
            {
                Action<PathFinderPath> onFinallyDoneFunc = PathCalculationDone;
                StartCoroutine(pfController.FindFlowFieldPathInProximity(path, onFinallyDoneFunc));
            }*/

            PathCalculationDone(path);
        };
        pfController.FindPathTo(CalculateStartPosition(), testTarget.transform.position, calculateFlowFieldPath, onDoneFunc);
        //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
    }
    
    [Button("Recalculate Path (MEDIC UNIT)"), DisableInEditorMode]
    public void CalculatePathToTargetMedic()
    {
        if (controlledMedicUnits == null || controlledMedicUnits.Count == 0 ||
            controlledMedicUnits.Count(unit => unit.gameObject.activeInHierarchy) == 0)
            return;
        
        controlledMedicUnits?.ForEach(unit => unit.move = false);
        
        Action<PathFinderPath> onDoneFunc = path =>
        {
            controlledMedicUnits?.ForEach(unit =>
            {
                if(!unit.gameObject.activeInHierarchy)
                    return;
                unit.currentPath = path;
                unit.move = true;
            });
        };
        pfController.FindPathTo(CalculateStartPositionMedics(), testTarget.transform.position, calculateFlowFieldPath, onDoneFunc);
        //StartCoroutine(pfController.FindPath(transform.position, testTarget.transform.position, onDoneFunc));
    }
    public Vector3 CalculateStartPositionMedics()
    {
        var center = Vector3.zero;

        if (controlledMedicUnits.Count <= 0) return center;
        center = controlledMedicUnits.Where(unit => unit.gameObject.activeInHierarchy).Aggregate(center, (current, unit) => current + unit.transform.position);
        center /= ( controlledMedicUnits.Count+1 );

        return center;
    }

    public Vector3 CalculateStartPosition()
    {
        var center = Vector3.zero;

        if (controlledUnits.Count <= 0) return center;
        center = controlledUnits.Where(unit => unit.gameObject.activeInHierarchy).Aggregate(center, (current, unit) => current + unit.transform.position);
        center /= ( controlledUnits.Count+1 );

        return center;
    }
    
    private void PathCalculationDone(PathFinderPath path)
    {
        currentPath = path;
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

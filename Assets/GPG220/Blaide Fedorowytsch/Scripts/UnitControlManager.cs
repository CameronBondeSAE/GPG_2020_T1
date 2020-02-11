using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitControlManager : MonoBehaviour
{
    private Camera mainCam;
    
    public InputAction selectKeyPressed;
    public InputAction selectKeyReleased;
    public bool selectKeyDown;
    public InputAction actionKey;
    public InputAction cursorMove;

    public Vector2 cursorPoint;
    public Vector3 targetPoint;
    public Vector3[] selectionRect;

    
    public BoxCollider boxCollider;
    public LineRenderer lineRenderer;
    public float HeightOffset;

    public LayerMask layerMask;
    // Start is called before the first frame update
    private void Awake()
    {
        selectKeyPressed.performed += SelectKepPressed;
        selectKeyReleased.performed += SelectKeyReleased;
        actionKey.performed += DoAction;
        cursorMove.performed += CursorMove;

    }

    private void OnEnable()
    {
        selectKeyPressed.Enable();
        selectKeyReleased.Enable();
        actionKey.Enable();
        cursorMove.Enable();
    }

    private void OnDisable()
    {
        selectKeyPressed.Disable();
        selectKeyReleased.Disable();
        actionKey.Disable();
        cursorMove.Disable();
    }

    void Start()
    {
        mainCam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        selectionRect = new Vector3[4];
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectKepPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("select Pressed");
        selectionRect[0] = new Vector3(targetPoint.x,targetPoint.y + HeightOffset,targetPoint.z);
        selectKeyDown = true;
    }

    void SelectKeyReleased(InputAction.CallbackContext ctx)
    {
        Debug.Log("select Released");
        selectionRect[2] = new Vector3(targetPoint.x,targetPoint.y + HeightOffset,targetPoint.z + HeightOffset);
        selectKeyDown = false;
        DrawRectangle();
        CheckSelection();
    }

    void DrawRectangle()
    {
        selectionRect[1]= new Vector3(selectionRect[0].x,targetPoint.y + HeightOffset,targetPoint.z);
        selectionRect[2]= new Vector3(targetPoint.x,targetPoint.y +HeightOffset,targetPoint.z);
        selectionRect[3] = new Vector3(targetPoint.x,targetPoint.y + HeightOffset,selectionRect[0].z);
        lineRenderer.SetPositions(selectionRect);
    }

    void CheckSelection()
    {
        boxCollider.center = Vector3.Lerp(selectionRect[0], selectionRect[2], 0.5f);
        boxCollider.size = new Vector3(Vector3.Distance( selectionRect[1],selectionRect[2]),10f,Vector3.Distance( selectionRect[0],selectionRect[1]));
    }




    void DoAction(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Action");
    }

    void CursorMove(InputAction.CallbackContext ctx)
    {
        cursorPoint = ctx.ReadValue<Vector2>();
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(ctx.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out hit,1000,layerMask,QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
        }
        
        if (selectKeyDown)
        {
            DrawRectangle();
            CheckSelection();
        }
    }
    
}

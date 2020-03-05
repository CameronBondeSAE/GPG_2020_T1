using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    /// <summary>
    /// Unit selection is all handled here,  It only selects objects that inherit from Iselectable.
    /// 
    /// </summary>
    public class UnitSelectionManager : SerializedMonoBehaviour
    {
        private Camera mainCam;
        [SerializeField]
        private uint localnetID;
        
        public InputAction selectKeyPressed;
        public InputAction selectKeyReleased;
        [SerializeField]
        private bool selectKeyDown;
        public InputAction actionKey;
        public InputAction cursorMove;
        
        private bool hadFocusLastFrame;
        private bool windowHasFocus;
        
        /// <summary>
        /// screenSpace Cursor location.
        /// </summary>
        public Vector2 cursorPoint;
        
        /// <summary>
        /// WorldSpace location the point on the ground that the mouse is over.
        /// </summary>
        public Vector3 targetPoint; 
        
        /// <summary>
        /// The current object the mouse cursor is over.
        /// </summary>
        public GameObject targetObject;
        private Vector3[] selectionRect;
        [SerializeField]
        private float minimumSelectionSize = 0.1f;

        private bool cursorOverUI;

        public BoxCollider boxCollider;

        private LineRenderer lineRenderer;
        [SerializeField]
        private float heightOffset;
        [SerializeField]
        private LayerMask worldLayerMask;
        [SerializeField]
        private LayerMask unitLayerMask;

        private GameManager gameManager;
        private RTSNetworkManager rtsNetworkManager;

        /// <summary>
        /// Called every time a selection is made.
        /// </summary>
        [HideInInspector]
        public Action<List<ISelectable>> onSelectionEvent;
        
        /// <summary>
        /// called every time any units are deselected, including when units die and leave the selection.
        /// </summary>
        [HideInInspector] 
        public Action<List<ISelectable>> onDeselectionEvent;
        
        /// <summary>
        /// called when the mouse is over an Iselectable;
        /// </summary>
        [HideInInspector]
        public Action<ISelectable> mouseOverIselectable;
        
        [SerializeField]
        private List<ISelectable> iSelectablesInSelection;

        /// <summary>
        /// List of selected Iselectables, To get the UnitBases You Will need to cast it as UnitBase like (UnitBase)selectedIselectables[i].whatever
        /// 
        /// </summary>
        public List<ISelectable> selectedIselectables;
        
        private Outline outline;
        // Start is called before the first frame update
        private void Awake()
        {
            selectKeyPressed.performed += SelectKeyPressed;
            selectKeyReleased.performed += SelectKeyReleased;
            actionKey.performed += DoAction;
            cursorMove.performed += CursorMove;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            windowHasFocus = hasFocus;
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
            outline = GetComponent<Outline>();
            mainCam = Camera.main;
            lineRenderer = GetComponent<LineRenderer>();
            selectionRect = new Vector3[4];
            boxCollider = GetComponent<BoxCollider>();
            gameManager = FindObjectOfType<GameManager>();
            rtsNetworkManager = FindObjectOfType<RTSNetworkManager>();
        }

        // Update is called once per frame
        void Update()
        {
            cursorOverUI =  UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            hadFocusLastFrame = windowHasFocus;
        }

        void SelectKeyPressed(InputAction.CallbackContext ctx)
        {
            //Check for UI element at this cursorPosition;
            if (cursorOverUI|| !hadFocusLastFrame )
            {
                
            }
            else
            {
                selectionRect[0] = new Vector3(targetPoint.x, targetPoint.y + heightOffset, targetPoint.z);
                selectKeyDown = true;
                DeselectAll();
            }
        }

        void IselectableDeadOrDestroyed( Health h)
        {
            ISelectable i = h.GetComponent<ISelectable>();
                
            selectedIselectables.Remove(i);
            iSelectablesInSelection.Remove(i);
            List<ISelectable> iList = new List<ISelectable>();
            iList.Add(i);
            onDeselectionEvent.Invoke(iList);
            
;        }

        PlayerBase FindLocalPlayer()
        {
            PlayerBase retPlayerBase = null;
            if (gameManager != null && gameManager.listofPlayerBases.Count > 0)
            { 
                retPlayerBase = gameManager.listofPlayerBases[0];
                foreach (PlayerBase pbs in gameManager.listofPlayerBases )
                {
                    if (pbs.isLocalPlayer)
                    {
                        retPlayerBase = pbs;
                    }
                }
            }
            return retPlayerBase;
        }

        uint GetOwnerID()
        {
            localnetID = FindLocalPlayer().netId;
            return localnetID;
        }
        

        void SelectKeyReleased(InputAction.CallbackContext ctx)
        {
            if (selectKeyDown)
            {
                selectionRect[2] =
                    new Vector3(targetPoint.x, targetPoint.y + heightOffset, targetPoint.z + heightOffset);
                selectKeyDown = false;


                if (Vector3.Distance(selectionRect[0], selectionRect[2]) <= minimumSelectionSize)
                {
                    if (targetObject != null)
                    {
                        if (targetObject.GetComponent<ISelectable>() != null )
                        {
                            ISelectable s = targetObject.GetComponent<ISelectable>();

                            //if (((UnitBase) s).owner == FindLocalPlayer() )
                            if (((UnitBase) s).ownerNetID == GetOwnerID())
                            {
                                s.OnSelected();
                                ApplyOutlineToObject(((MonoBehaviour)s).gameObject);
                                selectedIselectables.Add(s);
                            }

                        }
                    }
                }
                else
                {
                    DrawRectangle();
                    AdjustTriggerBox();
                    lineRenderer.SetPositions(new Vector3[4] {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero});
                    foreach (ISelectable s in iSelectablesInSelection)
                    {
                        if (((UnitBase) s).ownerNetID == GetOwnerID())
                        {
                            s.OnSelected();
                            ApplyOutlineToObject(((MonoBehaviour) s).gameObject);
                            selectedIselectables.Add(s);
                        }
                    }
                }

                foreach (ISelectable s in selectedIselectables)
                {
                    if(((MonoBehaviour)s).gameObject.GetComponent<Health>() != null)
                    {
                        
                        Health health = ((MonoBehaviour)s).gameObject.GetComponent<Health>();
                        health.deathEvent += IselectableDeadOrDestroyed;
                    }
                }
            }
            
            boxCollider.center = new Vector3(0,100000,0);
            boxCollider.size = Vector3.zero;

            if (selectedIselectables.Count > 0)
            {
                onSelectionEvent?.Invoke(selectedIselectables);
            }
        }

        void DrawRectangle()
        {
            selectionRect[1]= new Vector3(selectionRect[0].x,targetPoint.y + heightOffset,targetPoint.z);
            selectionRect[2]= new Vector3(targetPoint.x,targetPoint.y +heightOffset,targetPoint.z);
            selectionRect[3] = new Vector3(targetPoint.x,targetPoint.y + heightOffset,selectionRect[0].z);
            lineRenderer.SetPositions(selectionRect);
        }

        void AdjustTriggerBox()
        {
            boxCollider.center = Vector3.Lerp(selectionRect[0], selectionRect[2], 0.5f);
            boxCollider.size = new Vector3(Vector3.Distance( selectionRect[1],selectionRect[2]),10f,Vector3.Distance( selectionRect[0],selectionRect[1]));
        }
        
        // This is just detecting objects using a box Collider set to trigger, It doesn't work unless they have rigidbodies
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ISelectable>() != null && selectKeyDown)
            {
                
                ISelectable i = other.GetComponent<ISelectable>();
                
                 
                if(((MonoBehaviour)i).gameObject.GetComponent<Health>() != null)
                {
                        
                    Health health = ((MonoBehaviour)i).gameObject.GetComponent<Health>();
                    health.deathEvent += IselectableDeadOrDestroyed;
                }
                
                if (i.Selectable() &&  !iSelectablesInSelection.Contains(i))
                {
                    iSelectablesInSelection.Add(other.gameObject.GetComponent<ISelectable>());
                }
            }

            for (int i = 0; i < iSelectablesInSelection.Count; i++)
            {
                if (iSelectablesInSelection[i] == null)
                {
                    iSelectablesInSelection.RemoveAt(i);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ISelectable>() != null )
            {
                ISelectable i = other.GetComponent<ISelectable>();
                if (iSelectablesInSelection.Contains(i))
                {
                    iSelectablesInSelection.Remove(other.gameObject.GetComponent<ISelectable>());
                }
            }
        }
        
/// <summary>
/// This Just deselectes all the Iselectables.
/// </summary>
        public void DeselectAll()
        {
            if (selectedIselectables.Count > 0)
            {
                foreach (ISelectable s in selectedIselectables)
                {
                    var selectedGameObject = ((MonoBehaviour) s).gameObject;
                    if (selectedGameObject != null) RemoveOutlineFromObject(selectedGameObject);
                }

                List<ISelectable> removedIselectables = selectedIselectables;
                selectedIselectables.Clear();
                onDeselectionEvent?.Invoke(removedIselectables);
            }
        }

        void ApplyOutlineToObject(GameObject selectedGameObject)
        {
            
            if (selectedGameObject.GetComponent<Outline>() == null && selectedGameObject.GetComponent<MeshRenderer>() != null)
            {
                Outline o = selectedGameObject.AddComponent<Outline>();
                o.OutlineColor = outline.OutlineColor;
                o.OutlineMode = outline.OutlineMode;
                o.OutlineWidth = outline.OutlineWidth;
            }
            else if (selectedGameObject.GetComponentInChildren<Outline>() == null)
            {
                GameObject childOfSelected = selectedGameObject.GetComponentInChildren<MeshRenderer>().gameObject;
                Outline o = childOfSelected.AddComponent<Outline>();
                o.OutlineColor = outline.OutlineColor;
                o.OutlineMode = outline.OutlineMode;
                o.OutlineWidth = outline.OutlineWidth;
            }
        }

        void RemoveOutlineFromObject(GameObject selectedGameObject)
        {
            if (selectedGameObject.GetComponent<Outline>() != null)
            {
                Destroy(selectedGameObject.GetComponent<Outline>());
            }
            else if (selectedGameObject.GetComponentInChildren<Outline>() != null)
            {
                Destroy(selectedGameObject.GetComponentInChildren<Outline>());
            }
        }

        void DoAction(InputAction.CallbackContext ctx)
        {
            foreach (ISelectable S in selectedIselectables)
            {
                S.OnExecuteAction(targetPoint,targetObject);
            }
        }

        void CursorMove(InputAction.CallbackContext ctx)
        {
            cursorPoint = ctx.ReadValue<Vector2>();

            cursorPoint = cursorPoint.Clamp(Vector2.zero, new Vector2(mainCam.scaledPixelWidth, mainCam.scaledPixelHeight));
            
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(cursorPoint);

            if (Physics.Raycast(ray, out hit,1000,worldLayerMask,QueryTriggerInteraction.Ignore))
            {
                targetPoint = hit.point;
            }

            if (Physics.Raycast(ray, out hit, 1000, unitLayerMask, QueryTriggerInteraction.Ignore))
            {
                targetObject = hit.collider.gameObject;
                ISelectable targetIselectable = targetObject.GetComponent<ISelectable>();
                if (targetIselectable != null) mouseOverIselectable?.Invoke(targetIselectable);
            }
            else
            {
                targetObject = null;
            }

            if (selectKeyDown)
            {
                DrawRectangle();
                AdjustTriggerBox();
            }
            
        }

        private void OnDrawGizmos()
        {
           // Gizmos.DrawSphere(targetPoint, 1);
        }
    }
}

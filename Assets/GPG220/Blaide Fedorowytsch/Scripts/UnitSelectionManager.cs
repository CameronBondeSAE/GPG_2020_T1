using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Unit;
using QuickOutline;
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
        
        public InputActionAsset controls;
        [HideInInspector]
        public InputActionMap actionMap;
        [HideInInspector]
        public InputAction selectKeyPress;
        [HideInInspector]
        public InputAction selectKeyRelease;
        
        [SerializeField]
        private bool selectKeyDown;
        [HideInInspector]
        public InputAction actionKey;
        [HideInInspector]
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

        public GameManager gameManager;
        private RTSNetworkManager rtsNetworkManager;

        /// <summary>
        /// Called every time a selection is made.
        /// </summary>
        [HideInInspector]
        public event Action<List<ISelectable>> OnSelectionEvent;
        
        /// <summary>
        /// called every time any units are deselected, including when units die and leave the selection.
        /// </summary>
        [HideInInspector] 
        public event Action<List<ISelectable>> OnDeselectionEvent;
        
        /// <summary>
        /// called when the mouse moves while over an Iselectable;
        /// </summary>
        [HideInInspector]
        public event Action<ISelectable> MouseOverIselectable;
        
        [SerializeField]
        private List<ISelectable> iSelectablesInSelection;

        /// <summary>
        /// List of selected Iselectables, To get the UnitBases You Will need to cast it as UnitBase like (UnitBase)selectedIselectables[i].whatever
        /// 
        /// </summary>
        public List<ISelectable> selectedIselectables;
        
        private QuickOutline.Outline outline;

		/// <summary>
		/// Force ignoring network ID's just for testing
		/// </summary>
		public bool alwaysSelectDebug = false;

		// Start is called before the first frame update
        private void Awake()
        {
            actionMap = controls.actionMaps[0];
            selectKeyPress = actionMap.FindAction("SelectionKeyPress");
            selectKeyRelease = actionMap.FindAction("selectionKeyRelease");
            actionKey = actionMap.FindAction("ActionKeyPress");
            cursorMove = actionMap.FindAction("CursorMove");
            selectKeyPress.performed += SelectKeyPress;
            selectKeyRelease.performed += SelectActionRelease;
            //actionKey.performed += DoAction;
            cursorMove.performed += CursorMove;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            windowHasFocus = hasFocus;
        }

        private void OnEnable()
        {
            selectKeyPress.Enable();
            selectKeyRelease.Enable();
            actionKey.Enable();
            cursorMove.Enable();
        }

        private void OnDisable()
        {
            selectKeyPress.Disable();
            selectKeyRelease.Disable();
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
            rtsNetworkManager = FindObjectOfType<RTSNetworkManager>();

			// gameManager = FindObjectOfType<GameManager>();
			// GameManager isn't spawned until the game host starts
			rtsNetworkManager.OnStartedHost += () => gameManager = FindObjectOfType<GameManager>();
		}

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
              cursorOverUI =  UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();  
            }
            else
            {
                cursorOverUI = false;
            }
            hadFocusLastFrame = windowHasFocus;
        }

        void SelectKeyPress(InputAction.CallbackContext ctx)
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
            OnDeselectionEvent.Invoke(iList);
            
;        }

        PlayerBase FindLocalPlayer()
        {
            //HACK 
            //Todo Fix this crap,
            List<PlayerBase> playerBases = new List<PlayerBase>();
            if (gameManager != null)
            {
                if (gameManager.listofPlayerBases.Count > 0)
                {
                    playerBases = gameManager.listofPlayerBases;
                }
            }
            else
            {
                Debug.Log("GameManager was not set in UnitSelectionManager, manually searching for GameManager.");
                gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    playerBases = gameManager.listofPlayerBases;
                }
            }
            
            // if( gameManager == null || gameManager.listofPlayerBases.Count <= 0)
           if(playerBases.Count <= 0) 
           {
                playerBases = FindObjectsOfType<PlayerBase>().ToList();
                Debug.Log("UnitSelectionManager could not find GameManager or GameManager had no list of players, manually searching for players.");
            }

            
            
            PlayerBase retPlayerBase = null;
            if ( playerBases.Count > 0)
            { 
                retPlayerBase = playerBases[0];
                foreach (PlayerBase pbs in playerBases )
                {
                    if (pbs.isLocalPlayer)
                    {
                        retPlayerBase = pbs;
                    }
                }
            }
            else
            {
                Debug.Log("UnitSelectionManager Could not find playerBases.");
            }
            return retPlayerBase;
        }
        uint GetOwnerID()
        {
			PlayerBase findLocalPlayer = FindLocalPlayer();

			if (findLocalPlayer != null)
			{
				localnetID = findLocalPlayer.netId;
			}
			return localnetID;
        }
        

        void SelectActionRelease(InputAction.CallbackContext ctx)
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
                            if (((UnitBase) s).ownerNetID == GetOwnerID() || alwaysSelectDebug)
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
                        if (((UnitBase) s).ownerNetID == GetOwnerID() || alwaysSelectDebug)
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
                    s.OnSelected(selectedIselectables);
                }
                
                boxCollider.center = new Vector3(0,100000,0);
                boxCollider.size = Vector3.zero;

                if (selectedIselectables.Count > 0)
                {
                    OnSelectionEvent?.Invoke(selectedIselectables);
                }
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
                OnDeselectionEvent?.Invoke(removedIselectables);
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

/*        void DoAction(InputAction.CallbackContext ctx)
        {
            foreach (ISelectable S in selectedIselectables)
            {
                S.OnExecuteAction(targetPoint,targetObject);
            }
        }*/

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

            if (Physics.Raycast(ray, out hit, 1000, unitLayerMask, QueryTriggerInteraction.Collide))
            {
                targetObject = hit.collider.gameObject;
                ISelectable targetIselectable = targetObject.GetComponent<ISelectable>();
                if (targetIselectable != null) MouseOverIselectable?.Invoke(targetIselectable);
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

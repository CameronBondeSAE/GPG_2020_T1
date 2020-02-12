using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using Sirenix.OdinInspector;
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
    
        public InputAction selectKeyPressed;
        public InputAction selectKeyReleased;
        public bool selectKeyDown;
        public InputAction actionKey;
        public InputAction cursorMove;

        public Vector2 cursorPoint;
        public Vector3 targetPoint;
        public GameObject targetObject;
        public Vector3[] selectionRect;
        
    
        public BoxCollider boxCollider;
        public LineRenderer lineRenderer;
        public float heightOffset;

        public LayerMask worldLayerMask;
        public LayerMask unitLayerMask;


        public List<ISelectable> ISelectablesInSelection;
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
            selectionRect[0] = new Vector3(targetPoint.x,targetPoint.y + heightOffset,targetPoint.z);
            selectKeyDown = true;
        }

        void SelectKeyReleased(InputAction.CallbackContext ctx)
        {
            selectionRect[2] = new Vector3(targetPoint.x,targetPoint.y + heightOffset,targetPoint.z + heightOffset);
            selectKeyDown = false;
            DrawRectangle();
            CheckSelection();
            foreach (ISelectable S in ISelectablesInSelection)
            {
                S.OnSelected();
            }
        }

        void DrawRectangle()
        {
            selectionRect[1]= new Vector3(selectionRect[0].x,targetPoint.y + heightOffset,targetPoint.z);
            selectionRect[2]= new Vector3(targetPoint.x,targetPoint.y +heightOffset,targetPoint.z);
            selectionRect[3] = new Vector3(targetPoint.x,targetPoint.y + heightOffset,selectionRect[0].z);
            lineRenderer.SetPositions(selectionRect);
        }

        void CheckSelection()
        {
            boxCollider.center = Vector3.Lerp(selectionRect[0], selectionRect[2], 0.5f);
            boxCollider.size = new Vector3(Vector3.Distance( selectionRect[1],selectionRect[2]),10f,Vector3.Distance( selectionRect[0],selectionRect[1]));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ISelectable>() != null)
            {
                ISelectable i = other.GetComponent<ISelectable>();
                if (i.Selectable())
                {
                    ISelectablesInSelection.Add(other.gameObject.GetComponent<ISelectable>());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ISelectable>() != null)
            {
                ISelectable i = other.GetComponent<ISelectable>();
                if (i.Selectable())
                {
                    ISelectablesInSelection.Remove(other.gameObject.GetComponent<ISelectable>());
                }
            }
        }


        void DoAction(InputAction.CallbackContext ctx)
        {
            foreach (ISelectable S in ISelectablesInSelection)
            {
                S.OnExecuteAction(targetPoint,targetObject);
            }
        }

        void CursorMove(InputAction.CallbackContext ctx)
        {
            cursorPoint = ctx.ReadValue<Vector2>();
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(ctx.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out hit,1000,worldLayerMask,QueryTriggerInteraction.Ignore))
            {
                targetPoint = hit.point;
            }

            if (Physics.Raycast(ray, out hit, 1000, unitLayerMask, QueryTriggerInteraction.Ignore))
            {
                targetObject = hit.collider.gameObject;
            }
            else
            {
                targetObject = null;
            }

            if (selectKeyDown)
            {
                DrawRectangle();
                CheckSelection();
            }
        }
    
    }
}

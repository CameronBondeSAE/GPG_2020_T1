using System;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
namespace DevConsoleTest
{
    public class DeveloperConsoleBehaviour : MonoBehaviour
    {
        [SerializeField] private string prefix = String.Empty;
        [SerializeField] private  ConsoleCommand[] commands = new ConsoleCommand[0];

        [Header("UI")] 
        [SerializeField] private GameObject uICanvas = null;
        [SerializeField] private TMP_InputField inputField = null;

        public Camera camera;

        private float pausedTimeScale;
        public static DeveloperConsoleBehaviour instance;

        private DeveloperConsole developerConsole;

        private DeveloperConsole DeveloperConsole
        {
            get
            {
                if (developerConsole != null)
                {
                    return developerConsole;
                }

                return developerConsole = new DeveloperConsole(prefix, commands);
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            
            DontDestroyOnLoad(gameObject);
            camera = FindObjectOfType<Camera>();
        }
        public void Toggle(CallbackContext context)
        {
            if (!context.action.triggered)
            {
                return;
            }

            //take out timescale if u want time to continue while in console
            if (uICanvas.activeSelf)
            {
                Time.timeScale = pausedTimeScale;
                uICanvas.SetActive(false);
            }
            else
            {
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uICanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        public void InputCommand(string inputValue)
        {
            DeveloperConsole.ProcessCommand(inputValue);

            inputField.text = string.Empty;
        }
        
        //to test for raycast while in console
        public RaycastHit ShootRaycast(LayerMask toDestroy)
        {

            RaycastHit hitInfo;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, toDestroy))
            {
                Debug.Log(hitInfo.collider.name);
                return hitInfo;
            }
        
        
            return hitInfo;
        }
    }
}

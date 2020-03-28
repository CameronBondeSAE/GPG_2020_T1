// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/RTSCamera/CameraControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControls"",
    ""maps"": [
        {
            ""name"": ""CameraActionMap"",
            ""id"": ""d9442a2c-0517-45ac-a1eb-4f3eb7d7dff3"",
            ""actions"": [
                {
                    ""name"": ""MovementControls"",
                    ""type"": ""Button"",
                    ""id"": ""6a885cec-d473-47cc-a818-de6adb0ec958"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold,Press""
                },
                {
                    ""name"": ""SpeedUpCamera"",
                    ""type"": ""Button"",
                    ""id"": ""92cfb879-e785-45b5-b40e-ba9252ea2e0f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold,Press""
                },
                {
                    ""name"": ""RotateLeft"",
                    ""type"": ""Button"",
                    ""id"": ""bda5f40c-6228-4d62-b181-3637cfe16e7e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press,Hold""
                },
                {
                    ""name"": ""RotateRight"",
                    ""type"": ""Button"",
                    ""id"": ""20b703db-682d-41cc-92eb-43efec95ed04"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press,Hold""
                },
                {
                    ""name"": ""ZoomIn"",
                    ""type"": ""Button"",
                    ""id"": ""9d0e02e9-e655-422b-bfeb-9f0aa5b3febf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press,Hold""
                },
                {
                    ""name"": ""ZoomOut"",
                    ""type"": ""Button"",
                    ""id"": ""7d20796a-568b-46a9-abff-8e9cbea3c967"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press,Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Movement"",
                    ""id"": ""70621548-83d7-456f-bc7b-329c8589f132"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b2c5e9f8-2af8-4538-b5a2-833474cdcbaf"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""52e3e914-0071-498a-abfe-ac70ec41af80"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""53c71d3c-7446-4ad7-a878-576043a692d4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2865c2a5-c012-4cde-9deb-28a881cd13fe"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""MovementArrow"",
                    ""id"": ""904714b1-08ac-4e61-a6e2-0c6e771cd79c"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""81046b06-620d-4a85-abae-a5272cf4e4b2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3b29fbfc-1e78-4399-8162-2d5a5bd601fc"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27bfffcd-870e-432b-939a-8a92e54b2ed6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""46954874-86d6-4e1e-8f45-a7eab95d4604"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3346f96f-92d9-4604-b1a1-aeedd17683fc"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedUpCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf85c859-b56d-4f60-a59b-8658885be585"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Hold,Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0947c6b6-4822-4788-8142-5d219dd000e5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Hold,Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8054deee-2aeb-416c-8a81-5aa7678d716e"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cf81294-7893-425a-a738-f2060a41e3a5"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomOut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""CameraControls"",
            ""bindingGroup"": ""CameraControls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CameraActionMap
        m_CameraActionMap = asset.FindActionMap("CameraActionMap", throwIfNotFound: true);
        m_CameraActionMap_MovementControls = m_CameraActionMap.FindAction("MovementControls", throwIfNotFound: true);
        m_CameraActionMap_SpeedUpCamera = m_CameraActionMap.FindAction("SpeedUpCamera", throwIfNotFound: true);
        m_CameraActionMap_RotateLeft = m_CameraActionMap.FindAction("RotateLeft", throwIfNotFound: true);
        m_CameraActionMap_RotateRight = m_CameraActionMap.FindAction("RotateRight", throwIfNotFound: true);
        m_CameraActionMap_ZoomIn = m_CameraActionMap.FindAction("ZoomIn", throwIfNotFound: true);
        m_CameraActionMap_ZoomOut = m_CameraActionMap.FindAction("ZoomOut", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // CameraActionMap
    private readonly InputActionMap m_CameraActionMap;
    private ICameraActionMapActions m_CameraActionMapActionsCallbackInterface;
    private readonly InputAction m_CameraActionMap_MovementControls;
    private readonly InputAction m_CameraActionMap_SpeedUpCamera;
    private readonly InputAction m_CameraActionMap_RotateLeft;
    private readonly InputAction m_CameraActionMap_RotateRight;
    private readonly InputAction m_CameraActionMap_ZoomIn;
    private readonly InputAction m_CameraActionMap_ZoomOut;
    public struct CameraActionMapActions
    {
        private @CameraControls m_Wrapper;
        public CameraActionMapActions(@CameraControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementControls => m_Wrapper.m_CameraActionMap_MovementControls;
        public InputAction @SpeedUpCamera => m_Wrapper.m_CameraActionMap_SpeedUpCamera;
        public InputAction @RotateLeft => m_Wrapper.m_CameraActionMap_RotateLeft;
        public InputAction @RotateRight => m_Wrapper.m_CameraActionMap_RotateRight;
        public InputAction @ZoomIn => m_Wrapper.m_CameraActionMap_ZoomIn;
        public InputAction @ZoomOut => m_Wrapper.m_CameraActionMap_ZoomOut;
        public InputActionMap Get() { return m_Wrapper.m_CameraActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActionMapActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActionMapActions instance)
        {
            if (m_Wrapper.m_CameraActionMapActionsCallbackInterface != null)
            {
                @MovementControls.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovementControls;
                @MovementControls.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovementControls;
                @MovementControls.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovementControls;
                @SpeedUpCamera.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSpeedUpCamera;
                @SpeedUpCamera.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSpeedUpCamera;
                @SpeedUpCamera.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnSpeedUpCamera;
                @RotateLeft.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateLeft;
                @RotateLeft.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateLeft;
                @RotateLeft.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateLeft;
                @RotateRight.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateRight;
                @RotateRight.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateRight;
                @RotateRight.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateRight;
                @ZoomIn.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomIn;
                @ZoomIn.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomIn;
                @ZoomIn.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomIn;
                @ZoomOut.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomOut;
                @ZoomOut.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomOut;
                @ZoomOut.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomOut;
            }
            m_Wrapper.m_CameraActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementControls.started += instance.OnMovementControls;
                @MovementControls.performed += instance.OnMovementControls;
                @MovementControls.canceled += instance.OnMovementControls;
                @SpeedUpCamera.started += instance.OnSpeedUpCamera;
                @SpeedUpCamera.performed += instance.OnSpeedUpCamera;
                @SpeedUpCamera.canceled += instance.OnSpeedUpCamera;
                @RotateLeft.started += instance.OnRotateLeft;
                @RotateLeft.performed += instance.OnRotateLeft;
                @RotateLeft.canceled += instance.OnRotateLeft;
                @RotateRight.started += instance.OnRotateRight;
                @RotateRight.performed += instance.OnRotateRight;
                @RotateRight.canceled += instance.OnRotateRight;
                @ZoomIn.started += instance.OnZoomIn;
                @ZoomIn.performed += instance.OnZoomIn;
                @ZoomIn.canceled += instance.OnZoomIn;
                @ZoomOut.started += instance.OnZoomOut;
                @ZoomOut.performed += instance.OnZoomOut;
                @ZoomOut.canceled += instance.OnZoomOut;
            }
        }
    }
    public CameraActionMapActions @CameraActionMap => new CameraActionMapActions(this);
    private int m_CameraControlsSchemeIndex = -1;
    public InputControlScheme CameraControlsScheme
    {
        get
        {
            if (m_CameraControlsSchemeIndex == -1) m_CameraControlsSchemeIndex = asset.FindControlSchemeIndex("CameraControls");
            return asset.controlSchemes[m_CameraControlsSchemeIndex];
        }
    }
    public interface ICameraActionMapActions
    {
        void OnMovementControls(InputAction.CallbackContext context);
        void OnSpeedUpCamera(InputAction.CallbackContext context);
        void OnRotateLeft(InputAction.CallbackContext context);
        void OnRotateRight(InputAction.CallbackContext context);
        void OnZoomIn(InputAction.CallbackContext context);
        void OnZoomOut(InputAction.CallbackContext context);
    }
}

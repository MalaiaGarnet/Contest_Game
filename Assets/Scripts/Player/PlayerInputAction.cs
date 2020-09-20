// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""CharacterControl"",
            ""id"": ""ba66d8d4-394d-49c0-a9d1-2ad513337dbe"",
            ""actions"": [
                {
                    ""name"": ""Move_Control"",
                    ""type"": ""Button"",
                    ""id"": ""ae3d3fc3-5c6b-4bd3-9ef1-704e8a7e14a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""View_Control"",
                    ""type"": ""Button"",
                    ""id"": ""b12a2de8-bbf9-4420-9027-87c93fc606ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""a811d55b-bb9a-4fee-8edb-f46643a4ce90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool_1"",
                    ""type"": ""Button"",
                    ""id"": ""2bf63dca-6188-4452-af81-ff027e892d64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool_2"",
                    ""type"": ""Button"",
                    ""id"": ""dc3aea2c-8fee-413b-9e95-ad86fea2fc61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool_3"",
                    ""type"": ""Button"",
                    ""id"": ""271283bb-4344-420e-81f8-c2bf2d4d389d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool_4"",
                    ""type"": ""Button"",
                    ""id"": ""6b37dc3f-ed3d-408b-9865-dcb58bf60c8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Call_Menu"",
                    ""type"": ""Button"",
                    ""id"": ""082bdf7a-e309-4ed4-9b3e-37e911af8a93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""cbaab4a1-7af7-4080-970d-aba85c037688"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Control"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5f6f0fb4-dccc-47a1-a9fa-b8a979a473cb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""abaa8b64-602e-4ff4-89fb-d0913b6f7d27"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aa00e4d2-f108-40a9-b5de-c036d4d0d6b8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""65212452-14e4-4bdb-b739-1385fb255627"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""c802ff0c-4441-4a28-a1d1-0c67f4f92839"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move_Control"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c09c1f5e-cfb9-49a5-8dea-3763dfdfe67c"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e1397cb2-e0bc-489e-be29-88cb8ea8ad28"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b4422ac3-f653-44ae-89a8-6c74aa096c85"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a0e06c37-4453-4964-bab4-07a014b41d64"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""55a6cc97-08d2-487d-9fa8-a3a3fa44f0d4"",
                    ""path"": ""<Keyboard>/E"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a39f87ca-4b22-4c6f-9b5e-9d968f9d93ce"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6e7a9b0-530d-4b14-8b7e-bbe4a8849bfe"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Tool_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20313366-f6ad-496e-8076-6f08a54508e5"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a960cb2-ad7f-4c46-9871-00e63c7877df"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Tool_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50c988bc-85ad-487a-bc95-94831d0a4d22"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""835d6bd8-d36e-4913-88fd-8f59a16fa702"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Tool_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e63305f4-4ea4-427f-985c-2f4c9b505801"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8feb5131-b63d-4933-962e-746956013ff2"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Tool_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68451af9-701d-4e11-9c46-e1dd670e520f"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ab703d9-057d-40b6-951c-d26006cc31cf"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Call_Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97199a2e-7074-4d69-94c6-653bc15c984e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Call_Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""aad7543d-9f94-4a9a-85af-4e6b240b60a7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View_Control"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""62650d7d-da50-4963-8532-37547f756c10"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""View_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""203e8263-f26c-4a42-96ca-930d69f971f8"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""View_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6e78c32f-3751-47d9-9203-0e33cd0853ba"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""View_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ed1eee1-c79f-4c0a-9b23-8180fb3f7e1c"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""View_Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CharacterControl
        m_CharacterControl = asset.FindActionMap("CharacterControl", throwIfNotFound: true);
        m_CharacterControl_Move_Control = m_CharacterControl.FindAction("Move_Control", throwIfNotFound: true);
        m_CharacterControl_View_Control = m_CharacterControl.FindAction("View_Control", throwIfNotFound: true);
        m_CharacterControl_Interact = m_CharacterControl.FindAction("Interact", throwIfNotFound: true);
        m_CharacterControl_Tool_1 = m_CharacterControl.FindAction("Tool_1", throwIfNotFound: true);
        m_CharacterControl_Tool_2 = m_CharacterControl.FindAction("Tool_2", throwIfNotFound: true);
        m_CharacterControl_Tool_3 = m_CharacterControl.FindAction("Tool_3", throwIfNotFound: true);
        m_CharacterControl_Tool_4 = m_CharacterControl.FindAction("Tool_4", throwIfNotFound: true);
        m_CharacterControl_Call_Menu = m_CharacterControl.FindAction("Call_Menu", throwIfNotFound: true);
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

    // CharacterControl
    private readonly InputActionMap m_CharacterControl;
    private ICharacterControlActions m_CharacterControlActionsCallbackInterface;
    private readonly InputAction m_CharacterControl_Move_Control;
    private readonly InputAction m_CharacterControl_View_Control;
    private readonly InputAction m_CharacterControl_Interact;
    private readonly InputAction m_CharacterControl_Tool_1;
    private readonly InputAction m_CharacterControl_Tool_2;
    private readonly InputAction m_CharacterControl_Tool_3;
    private readonly InputAction m_CharacterControl_Tool_4;
    private readonly InputAction m_CharacterControl_Call_Menu;
    public struct CharacterControlActions
    {
        private @PlayerInputAction m_Wrapper;
        public CharacterControlActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move_Control => m_Wrapper.m_CharacterControl_Move_Control;
        public InputAction @View_Control => m_Wrapper.m_CharacterControl_View_Control;
        public InputAction @Interact => m_Wrapper.m_CharacterControl_Interact;
        public InputAction @Tool_1 => m_Wrapper.m_CharacterControl_Tool_1;
        public InputAction @Tool_2 => m_Wrapper.m_CharacterControl_Tool_2;
        public InputAction @Tool_3 => m_Wrapper.m_CharacterControl_Tool_3;
        public InputAction @Tool_4 => m_Wrapper.m_CharacterControl_Tool_4;
        public InputAction @Call_Menu => m_Wrapper.m_CharacterControl_Call_Menu;
        public InputActionMap Get() { return m_Wrapper.m_CharacterControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterControlActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterControlActions instance)
        {
            if (m_Wrapper.m_CharacterControlActionsCallbackInterface != null)
            {
                @Move_Control.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnMove_Control;
                @Move_Control.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnMove_Control;
                @Move_Control.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnMove_Control;
                @View_Control.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control;
                @View_Control.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control;
                @View_Control.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control;
                @Interact.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
                @Tool_1.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_1;
                @Tool_1.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_1;
                @Tool_1.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_1;
                @Tool_2.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_2;
                @Tool_2.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_2;
                @Tool_2.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_2;
                @Tool_3.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_3;
                @Tool_3.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_3;
                @Tool_3.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_3;
                @Tool_4.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_4;
                @Tool_4.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_4;
                @Tool_4.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnTool_4;
                @Call_Menu.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnCall_Menu;
                @Call_Menu.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnCall_Menu;
                @Call_Menu.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnCall_Menu;
            }
            m_Wrapper.m_CharacterControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move_Control.started += instance.OnMove_Control;
                @Move_Control.performed += instance.OnMove_Control;
                @Move_Control.canceled += instance.OnMove_Control;
                @View_Control.started += instance.OnView_Control;
                @View_Control.performed += instance.OnView_Control;
                @View_Control.canceled += instance.OnView_Control;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Tool_1.started += instance.OnTool_1;
                @Tool_1.performed += instance.OnTool_1;
                @Tool_1.canceled += instance.OnTool_1;
                @Tool_2.started += instance.OnTool_2;
                @Tool_2.performed += instance.OnTool_2;
                @Tool_2.canceled += instance.OnTool_2;
                @Tool_3.started += instance.OnTool_3;
                @Tool_3.performed += instance.OnTool_3;
                @Tool_3.canceled += instance.OnTool_3;
                @Tool_4.started += instance.OnTool_4;
                @Tool_4.performed += instance.OnTool_4;
                @Tool_4.canceled += instance.OnTool_4;
                @Call_Menu.started += instance.OnCall_Menu;
                @Call_Menu.performed += instance.OnCall_Menu;
                @Call_Menu.canceled += instance.OnCall_Menu;
            }
        }
    }
    public CharacterControlActions @CharacterControl => new CharacterControlActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICharacterControlActions
    {
        void OnMove_Control(InputAction.CallbackContext context);
        void OnView_Control(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnTool_1(InputAction.CallbackContext context);
        void OnTool_2(InputAction.CallbackContext context);
        void OnTool_3(InputAction.CallbackContext context);
        void OnTool_4(InputAction.CallbackContext context);
        void OnCall_Menu(InputAction.CallbackContext context);
    }
}

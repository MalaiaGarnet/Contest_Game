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
                    ""name"": ""View_Control_X"",
                    ""type"": ""Button"",
                    ""id"": ""23781dfb-f8d0-4184-b4de-5d7ed0f6635d"",
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
                    ""name"": ""Keyboard"",
                    ""id"": ""f5a95eaa-6f35-400f-b37c-5115fc4f58b2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""db1ae0c9-3737-4cb7-b799-7c3e1fb7032c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d605f487-5026-434e-95a0-35dbe070c1b0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7524610c-713c-4337-9cbb-4584b0ba8245"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e2235ab9-52f7-4080-a8fd-cc1fe485289b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Mouse"",
                    ""id"": ""b477015b-ab69-46ad-95d4-5f42b494b659"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9b85aec6-2047-450d-a3ec-e1f237f31392"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b965694a-6025-40eb-acc9-dd45f6101c21"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3c9a76a4-9d77-4c2c-bc4d-7e2d00b97d47"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4bf52d68-fc10-4724-a084-3d21d7e3d0d1"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""View_Control_X"",
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
        }
    ]
}");
        // CharacterControl
        m_CharacterControl = asset.FindActionMap("CharacterControl", throwIfNotFound: true);
        m_CharacterControl_Move_Control = m_CharacterControl.FindAction("Move_Control", throwIfNotFound: true);
        m_CharacterControl_View_Control_X = m_CharacterControl.FindAction("View_Control_X", throwIfNotFound: true);
        m_CharacterControl_Interact = m_CharacterControl.FindAction("Interact", throwIfNotFound: true);
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
    private readonly InputAction m_CharacterControl_View_Control_X;
    private readonly InputAction m_CharacterControl_Interact;
    public struct CharacterControlActions
    {
        private @PlayerInputAction m_Wrapper;
        public CharacterControlActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move_Control => m_Wrapper.m_CharacterControl_Move_Control;
        public InputAction @View_Control_X => m_Wrapper.m_CharacterControl_View_Control_X;
        public InputAction @Interact => m_Wrapper.m_CharacterControl_Interact;
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
                @View_Control_X.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control_X;
                @View_Control_X.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control_X;
                @View_Control_X.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnView_Control_X;
                @Interact.started -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CharacterControlActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_CharacterControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move_Control.started += instance.OnMove_Control;
                @Move_Control.performed += instance.OnMove_Control;
                @Move_Control.canceled += instance.OnMove_Control;
                @View_Control_X.started += instance.OnView_Control_X;
                @View_Control_X.performed += instance.OnView_Control_X;
                @View_Control_X.canceled += instance.OnView_Control_X;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
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
    public interface ICharacterControlActions
    {
        void OnMove_Control(InputAction.CallbackContext context);
        void OnView_Control_X(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}

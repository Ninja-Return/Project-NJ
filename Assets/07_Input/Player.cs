//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/07_Input/Player.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""ae36a0c9-72d8-4033-ab66-6cc428f4a657"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""fd2ca293-51c1-4276-b1b1-5cdc02b61239"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e6eb364d-20fc-4b8f-b4ff-59ebc6bde28c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""e4d67022-1d43-4f14-b60d-5aee0fa0f0b2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""1eaeaf39-20aa-4e3f-a0b2-d00b890fbdf3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveObject"",
                    ""type"": ""Button"",
                    ""id"": ""696244d7-345e-4a88-a5bd-9042d38c630a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InventoryActive"",
                    ""type"": ""Button"",
                    ""id"": ""b3d9eb7f-a82a-4281-b343-78c4b828377c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""UseHandObject"",
                    ""type"": ""Button"",
                    ""id"": ""4519d8a2-1795-4fb0-8783-74bc7b24178d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""d7787984-a035-4747-87c6-0f14d8426112"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9be83642-170c-4fb6-88a3-17e12a9eca09"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9cc41c08-aec4-4ac8-abd2-fe0416711aa2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f7286c68-216c-4c25-b3e4-07405cd72599"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9acae171-972e-4721-9f64-33f364b4c11d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d41d4c5c-2f8f-4d02-82b8-bd6190541285"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e3964ad-acd9-4598-ae35-5a6280659852"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76cc44fe-45b6-439a-9036-2e11f6926eb3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""172809ca-12df-480d-a085-2586feca378d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8af1c7bd-ade7-46b2-af22-b083f31ef58c"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryActive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eef8aae4-d4eb-41f6-9cec-8fc3bc31ed48"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyAndMouse"",
                    ""action"": ""UseHandObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyAndMouse"",
            ""bindingGroup"": ""KeyAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<VirtualMouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Move = m_PlayerMovement.FindAction("Move", throwIfNotFound: true);
        m_PlayerMovement_Jump = m_PlayerMovement.FindAction("Jump", throwIfNotFound: true);
        m_PlayerMovement_Camera = m_PlayerMovement.FindAction("Camera", throwIfNotFound: true);
        m_PlayerMovement_Interaction = m_PlayerMovement.FindAction("Interaction", throwIfNotFound: true);
        m_PlayerMovement_MoveObject = m_PlayerMovement.FindAction("MoveObject", throwIfNotFound: true);
        m_PlayerMovement_InventoryActive = m_PlayerMovement.FindAction("InventoryActive", throwIfNotFound: true);
        m_PlayerMovement_UseHandObject = m_PlayerMovement.FindAction("UseHandObject", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private List<IPlayerMovementActions> m_PlayerMovementActionsCallbackInterfaces = new List<IPlayerMovementActions>();
    private readonly InputAction m_PlayerMovement_Move;
    private readonly InputAction m_PlayerMovement_Jump;
    private readonly InputAction m_PlayerMovement_Camera;
    private readonly InputAction m_PlayerMovement_Interaction;
    private readonly InputAction m_PlayerMovement_MoveObject;
    private readonly InputAction m_PlayerMovement_InventoryActive;
    private readonly InputAction m_PlayerMovement_UseHandObject;
    public struct PlayerMovementActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerMovementActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerMovement_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerMovement_Jump;
        public InputAction @Camera => m_Wrapper.m_PlayerMovement_Camera;
        public InputAction @Interaction => m_Wrapper.m_PlayerMovement_Interaction;
        public InputAction @MoveObject => m_Wrapper.m_PlayerMovement_MoveObject;
        public InputAction @InventoryActive => m_Wrapper.m_PlayerMovement_InventoryActive;
        public InputAction @UseHandObject => m_Wrapper.m_PlayerMovement_UseHandObject;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
            @MoveObject.started += instance.OnMoveObject;
            @MoveObject.performed += instance.OnMoveObject;
            @MoveObject.canceled += instance.OnMoveObject;
            @InventoryActive.started += instance.OnInventoryActive;
            @InventoryActive.performed += instance.OnInventoryActive;
            @InventoryActive.canceled += instance.OnInventoryActive;
            @UseHandObject.started += instance.OnUseHandObject;
            @UseHandObject.performed += instance.OnUseHandObject;
            @UseHandObject.canceled += instance.OnUseHandObject;
        }

        private void UnregisterCallbacks(IPlayerMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
            @MoveObject.started -= instance.OnMoveObject;
            @MoveObject.performed -= instance.OnMoveObject;
            @MoveObject.canceled -= instance.OnMoveObject;
            @InventoryActive.started -= instance.OnInventoryActive;
            @InventoryActive.performed -= instance.OnInventoryActive;
            @InventoryActive.canceled -= instance.OnInventoryActive;
            @UseHandObject.started -= instance.OnUseHandObject;
            @UseHandObject.performed -= instance.OnUseHandObject;
            @UseHandObject.canceled -= instance.OnUseHandObject;
        }

        public void RemoveCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);
    private int m_KeyAndMouseSchemeIndex = -1;
    public InputControlScheme KeyAndMouseScheme
    {
        get
        {
            if (m_KeyAndMouseSchemeIndex == -1) m_KeyAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyAndMouse");
            return asset.controlSchemes[m_KeyAndMouseSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnMoveObject(InputAction.CallbackContext context);
        void OnInventoryActive(InputAction.CallbackContext context);
        void OnUseHandObject(InputAction.CallbackContext context);
    }
}

// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControll.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControll : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControll()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControll"",
    ""maps"": [
        {
            ""name"": ""PlayerKM"",
            ""id"": ""f0ab6f78-be26-4941-9f74-1805c57128d2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""72db7106-c59d-41a0-8228-542de9466169"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a49a7a30-13ce-4aa7-9aae-81ef3ba7875a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d4988b03-f71e-4cee-b956-53d77e84fa1f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08bf5a54-87a8-48a0-9910-61c0938aac4b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Hands"",
            ""id"": ""43f35142-91d4-4c19-b484-1b3408e8cd7f"",
            ""actions"": [
                {
                    ""name"": ""LeftHand"",
                    ""type"": ""Button"",
                    ""id"": ""c0715625-d965-44c4-bd8f-3c4a979f40f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightHand"",
                    ""type"": ""Button"",
                    ""id"": ""ab768bc6-39b1-4b1d-bf51-825decfa3458"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""42061839-88bc-4823-9441-66904b652816"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bac9be6-57d0-4092-8006-15985b12beef"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
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
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerKM
        m_PlayerKM = asset.FindActionMap("PlayerKM", throwIfNotFound: true);
        m_PlayerKM_Move = m_PlayerKM.FindAction("Move", throwIfNotFound: true);
        m_PlayerKM_Jump = m_PlayerKM.FindAction("Jump", throwIfNotFound: true);
        // Hands
        m_Hands = asset.FindActionMap("Hands", throwIfNotFound: true);
        m_Hands_LeftHand = m_Hands.FindAction("LeftHand", throwIfNotFound: true);
        m_Hands_RightHand = m_Hands.FindAction("RightHand", throwIfNotFound: true);
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

    // PlayerKM
    private readonly InputActionMap m_PlayerKM;
    private IPlayerKMActions m_PlayerKMActionsCallbackInterface;
    private readonly InputAction m_PlayerKM_Move;
    private readonly InputAction m_PlayerKM_Jump;
    public struct PlayerKMActions
    {
        private @PlayerControll m_Wrapper;
        public PlayerKMActions(@PlayerControll wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerKM_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerKM_Jump;
        public InputActionMap Get() { return m_Wrapper.m_PlayerKM; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerKMActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerKMActions instance)
        {
            if (m_Wrapper.m_PlayerKMActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerKMActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public PlayerKMActions @PlayerKM => new PlayerKMActions(this);

    // Hands
    private readonly InputActionMap m_Hands;
    private IHandsActions m_HandsActionsCallbackInterface;
    private readonly InputAction m_Hands_LeftHand;
    private readonly InputAction m_Hands_RightHand;
    public struct HandsActions
    {
        private @PlayerControll m_Wrapper;
        public HandsActions(@PlayerControll wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftHand => m_Wrapper.m_Hands_LeftHand;
        public InputAction @RightHand => m_Wrapper.m_Hands_RightHand;
        public InputActionMap Get() { return m_Wrapper.m_Hands; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HandsActions set) { return set.Get(); }
        public void SetCallbacks(IHandsActions instance)
        {
            if (m_Wrapper.m_HandsActionsCallbackInterface != null)
            {
                @LeftHand.started -= m_Wrapper.m_HandsActionsCallbackInterface.OnLeftHand;
                @LeftHand.performed -= m_Wrapper.m_HandsActionsCallbackInterface.OnLeftHand;
                @LeftHand.canceled -= m_Wrapper.m_HandsActionsCallbackInterface.OnLeftHand;
                @RightHand.started -= m_Wrapper.m_HandsActionsCallbackInterface.OnRightHand;
                @RightHand.performed -= m_Wrapper.m_HandsActionsCallbackInterface.OnRightHand;
                @RightHand.canceled -= m_Wrapper.m_HandsActionsCallbackInterface.OnRightHand;
            }
            m_Wrapper.m_HandsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftHand.started += instance.OnLeftHand;
                @LeftHand.performed += instance.OnLeftHand;
                @LeftHand.canceled += instance.OnLeftHand;
                @RightHand.started += instance.OnRightHand;
                @RightHand.performed += instance.OnRightHand;
                @RightHand.canceled += instance.OnRightHand;
            }
        }
    }
    public HandsActions @Hands => new HandsActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
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
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerKMActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IHandsActions
    {
        void OnLeftHand(InputAction.CallbackContext context);
        void OnRightHand(InputAction.CallbackContext context);
    }
}

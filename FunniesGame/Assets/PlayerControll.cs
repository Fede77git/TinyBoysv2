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
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""0c174efb-ace4-4163-984c-a425142bce20"",
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
                },
                {
                    ""name"": ""LeftHand"",
                    ""type"": ""Button"",
                    ""id"": ""27d49c38-4b66-423f-8998-41e0f3c31f26"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightHand"",
                    ""type"": ""Button"",
                    ""id"": ""b06d3ada-9748-4414-b188-d5f9e7b7a6dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7ce66b2a-1a94-4b02-8624-93217a44e03d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""asdw"",
                    ""id"": ""eaaa1c01-b642-4b4b-9471-b18da17173d1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7c9b3d0d-0587-4a5a-858c-2fb18b517a30"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""93032937-51bd-4c29-9811-8ebaed92d91e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""00a4cc6d-366a-4d88-9782-ff585f1a32bc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e0bc82d0-85d8-4a96-893e-44f150d51f88"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
        m_PlayerKM_Movement = m_PlayerKM.FindAction("Movement", throwIfNotFound: true);
        m_PlayerKM_Jump = m_PlayerKM.FindAction("Jump", throwIfNotFound: true);
        m_PlayerKM_LeftHand = m_PlayerKM.FindAction("LeftHand", throwIfNotFound: true);
        m_PlayerKM_RightHand = m_PlayerKM.FindAction("RightHand", throwIfNotFound: true);
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
    private readonly InputAction m_PlayerKM_Movement;
    private readonly InputAction m_PlayerKM_Jump;
    private readonly InputAction m_PlayerKM_LeftHand;
    private readonly InputAction m_PlayerKM_RightHand;
    public struct PlayerKMActions
    {
        private @PlayerControll m_Wrapper;
        public PlayerKMActions(@PlayerControll wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerKM_Movement;
        public InputAction @Jump => m_Wrapper.m_PlayerKM_Jump;
        public InputAction @LeftHand => m_Wrapper.m_PlayerKM_LeftHand;
        public InputAction @RightHand => m_Wrapper.m_PlayerKM_RightHand;
        public InputActionMap Get() { return m_Wrapper.m_PlayerKM; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerKMActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerKMActions instance)
        {
            if (m_Wrapper.m_PlayerKMActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnJump;
                @LeftHand.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnLeftHand;
                @LeftHand.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnLeftHand;
                @LeftHand.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnLeftHand;
                @RightHand.started -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnRightHand;
                @RightHand.performed -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnRightHand;
                @RightHand.canceled -= m_Wrapper.m_PlayerKMActionsCallbackInterface.OnRightHand;
            }
            m_Wrapper.m_PlayerKMActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @LeftHand.started += instance.OnLeftHand;
                @LeftHand.performed += instance.OnLeftHand;
                @LeftHand.canceled += instance.OnLeftHand;
                @RightHand.started += instance.OnRightHand;
                @RightHand.performed += instance.OnRightHand;
                @RightHand.canceled += instance.OnRightHand;
            }
        }
    }
    public PlayerKMActions @PlayerKM => new PlayerKMActions(this);
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
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLeftHand(InputAction.CallbackContext context);
        void OnRightHand(InputAction.CallbackContext context);
    }
}

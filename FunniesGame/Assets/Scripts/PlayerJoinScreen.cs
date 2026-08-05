using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerJoinScreen : MonoBehaviour
{
    [System.Serializable]
    public class PlayerJoinUI
    {
        public GameObject joinPrompt; // El texto que dice presiona para unirte
        public GameObject joinedState; // La imagen o panel del personaje con su color
    }

    public int maxPlayers = 4;
    public PlayerJoinUI[] playerPanels; 
    public Button nextButton; // Referencia al botón "Siguiente"
    public Button backButton; // Referencia al botón "Atrás"
    private int currentPlayerIndex = 0;
    
    private bool keyboard1Joined = false;
    private bool keyboard2Joined = false;

    void OnEnable()
    {
       
        DeviceAssigner.PlayerDevices.Clear();
        currentPlayerIndex = 0;
        keyboard1Joined = false;
        keyboard2Joined = false;
        
        // Resetear UI
        for (int i = 0; i < playerPanels.Length; i++)
        {
            if (playerPanels[i].joinPrompt != null) playerPanels[i].joinPrompt.SetActive(true);
            if (playerPanels[i].joinedState != null) playerPanels[i].joinedState.SetActive(false);
        }
        
        // Bloquear el botón de Siguiente al inicio
        if (nextButton != null)
        {
            nextButton.interactable = false;
        }
        
        Debug.Log("Pantalla de Join iniciada. Presiona cualquier botón en el teclado o joystick para unirte.");
    }

    void Update()
    {
        if (currentPlayerIndex >= maxPlayers) return;

        
        if (UnityEngine.EventSystems.EventSystem.current != null && backButton != null && 
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == backButton.gameObject)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        if (Keyboard.current != null)
        {
            bool p1JoinPress = Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame;
            bool p2JoinPress = Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;

            // Avanzar con Teclado (Enter)
            if (Keyboard.current.enterKey.wasPressedThisFrame && nextButton != null && nextButton.interactable)
            {
                nextButton.onClick.Invoke();
                return;
            }

            // Unirse con Teclado P1
            if (!keyboard1Joined && p1JoinPress)
            {
                keyboard1Joined = true;
                AssignDevice(Keyboard.current, true);
            }

            // Unirse con Teclado P2
            if (!keyboard2Joined && p2JoinPress)
            {
                keyboard2Joined = true;
                AssignDevice(Keyboard.current, true);
            }
        }

        foreach (var gamepad in Gamepad.all)
        {
            // Volver Atrás con Joystick (Círculo o B)
            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                if (backButton != null)
                {
                    backButton.onClick.Invoke();
                    return; // Salimos del update para que no procese nada más
                }
            }

            // Avanzar con Joystick (X o A) si ya está unido
            if (gamepad.buttonSouth.wasPressedThisFrame && DeviceAssigner.PlayerDevices.ContainsValue(gamepad))
            {
                if (nextButton != null && nextButton.interactable)
                {
                    nextButton.onClick.Invoke();
                    return;
                }
            }

            // Unirse con Joystick (Cualquier botón excepto el de Atrás)
            if (!DeviceAssigner.PlayerDevices.ContainsValue(gamepad))
            {
                bool pressed = false;
                foreach (var control in gamepad.allControls)
                {
                    if (control is ButtonControl button && button.wasPressedThisFrame)
                    {
                        // Ignoramos el Círculo/B para que no cuente como botón de unirse
                        if (button.name == "buttonEast") continue;
                        
                        pressed = true;
                        break;
                    }
                }

                if (pressed)
                {
                    AssignDevice(gamepad, false);
                }
            }
        }
    }

    private void AssignDevice(InputDevice device, bool isSharedKeyboard = false)
    {
        if (!isSharedKeyboard && DeviceAssigner.PlayerDevices.ContainsValue(device))
        {
            return;
        }

        DeviceAssigner.PlayerDevices[currentPlayerIndex] = device;
        Debug.Log("Jugador " + currentPlayerIndex + " se unió usando: " + device.displayName);

        if (currentPlayerIndex < playerPanels.Length)
        {
            if (playerPanels[currentPlayerIndex].joinPrompt != null) 
                playerPanels[currentPlayerIndex].joinPrompt.SetActive(false);
                
            if (playerPanels[currentPlayerIndex].joinedState != null) 
                playerPanels[currentPlayerIndex].joinedState.SetActive(true);
        }

        currentPlayerIndex++;
        
        // Desbloquear el botón de Siguiente si hay 2 o más jugadores
        if (nextButton != null && currentPlayerIndex >= 2)
        {
            nextButton.interactable = true;
        }
    }
}

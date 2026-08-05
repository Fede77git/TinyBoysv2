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

      
        if (Keyboard.current != null)
        {
            // Jugador 1 de teclado (WASD / Espacio)
            if (!keyboard1Joined && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame))
            {
                keyboard1Joined = true;
                AssignDevice(Keyboard.current, true);
            }
            // Jugador 2 de teclado (Flechas / Enter)
            if (!keyboard2Joined && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame))
            {
                keyboard2Joined = true;
                AssignDevice(Keyboard.current, true);
            }
        }

      
        foreach (var gamepad in Gamepad.all)
        {
            bool pressed = false;
            foreach (var control in gamepad.allControls)
            {
                if (control is ButtonControl button && button.wasPressedThisFrame)
                {
                    pressed = true;
                    break;
                }
            }

            if (pressed)
            {
                AssignDevice(gamepad);
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

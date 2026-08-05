using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceAssigner : MonoBehaviour
{
    private static DeviceAssigner instance;
    public static Dictionary<int, InputDevice> PlayerDevices = new Dictionary<int, InputDevice>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static InputDevice GetDeviceForPlayer(int playerIndex)
    {
       
        if (PlayerDevices.ContainsKey(playerIndex))
        {
            return PlayerDevices[playerIndex];
        }

        if (playerIndex >= 2)
        {
            int gamepadIndex = playerIndex - 2;
            if (Gamepad.all.Count > gamepadIndex)
            {
                return Gamepad.all[gamepadIndex];
            }
        }
        else
        {
            if (Keyboard.current != null)
            {
                return Keyboard.current;
            }
        }

        return null;
    }
}

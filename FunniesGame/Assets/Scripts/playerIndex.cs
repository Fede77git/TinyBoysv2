using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;


public class playerIndex : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;
        playerController = players.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }
}

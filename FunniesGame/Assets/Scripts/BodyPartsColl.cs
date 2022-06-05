
using UnityEngine;

public class BodyPartsColl : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }


    void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }
    
}

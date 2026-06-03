using UnityEngine;
using System.Collections.Generic;

public class RotateRound : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50f, 0);
    private HashSet<PlayerController> players = new HashSet<PlayerController>();

    void FixedUpdate()
    {
        Vector3 rotAngles = rotationSpeed * Time.fixedDeltaTime;
        Quaternion deltaRot = Quaternion.Euler(rotAngles);
        
        transform.rotation *= deltaRot;

        foreach (PlayerController pc in players)
        {
            if (pc != null && pc.isGrounded)
            {
                Vector3 offset = pc.pelvis.position - transform.position;
                Vector3 newOffset = deltaRot * offset;
                Vector3 movement = newOffset - offset;
                
                pc.platformVelocity = movement / Time.fixedDeltaTime;
                
                if (pc.moveAction == null || pc.moveAction.action.ReadValue<Vector2>() == Vector2.zero)
                {
                    pc.pelvis.rotation = deltaRot * pc.pelvis.rotation;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pc != null) players.Add(pc);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pc != null) players.Remove(pc);
        }
    }
}

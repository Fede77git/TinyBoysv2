using UnityEngine;
using System.Collections.Generic;

public class RotateRound : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50f, 0);
    private HashSet<PlayerController> players = new HashSet<PlayerController>();
    private HashSet<Rigidbody> items = new HashSet<Rigidbody>();

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

        items.RemoveWhere(rb => rb == null);
        foreach (Rigidbody rb in items)
        {
            if (!rb.isKinematic)
            {
                Vector3 offset = rb.position - transform.position;
                Vector3 newOffset = deltaRot * offset;
                Vector3 movement = newOffset - offset;
                
                rb.MovePosition(rb.position + movement);
                rb.MoveRotation(deltaRot * rb.rotation);
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
        else
        {
            Rigidbody rb = collision.collider.attachedRigidbody;
            if (rb != null && !rb.isKinematic) items.Add(rb);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponentInParent<PlayerController>();
            if (pc != null) players.Remove(pc);
        }
        else
        {
            Rigidbody rb = collision.collider.attachedRigidbody;
            if (rb != null) items.Remove(rb);
        }
    }
}

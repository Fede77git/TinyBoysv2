using UnityEngine;
using UnityEngine.InputSystem;

public class PunchMechanic : MonoBehaviour
{
    public InputActionReference punchAction;
    public Rigidbody[] punchRigidbodies;
    public float punchForce = 15f;
    public Animator animator;
    
    private bool isPunching;

    void OnEnable()
    {
        if (punchAction != null) punchAction.action.Enable();
    }

    void OnDisable()
    {
        if (punchAction != null) punchAction.action.Disable();
    }

    void Update()
    {
        if (punchAction != null && punchAction.action.WasPressedThisFrame())
        {
            Punch();
        }
    }

    void Punch()
    {
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        Vector3 punchDir = transform.forward;
        punchDir.y = 0;
        punchDir.Normalize();

        foreach (Rigidbody rb in punchRigidbodies)
        {
            if (rb != null)
            {
                rb.AddForce(punchDir * punchForce, ForceMode.Impulse);
            }
        }
        
        isPunching = true;
        Invoke(nameof(ResetPunch), 0.5f);
    }

    void ResetPunch()
    {
        isPunching = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPunching)
        {
            StuntPunch stunt = collision.collider.GetComponentInParent<StuntPunch>();
            if (stunt != null)
            {
                Vector3 hitDir = (stunt.transform.position - transform.position).normalized;
                hitDir.y = 0f; 
                hitDir.Normalize();
                stunt.ReceivePunch(hitDir, punchForce * 0.8f);
            }
        }
    }
}

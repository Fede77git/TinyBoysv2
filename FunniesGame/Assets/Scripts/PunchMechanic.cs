using UnityEngine;
using UnityEngine.InputSystem;

public class PunchMechanic : MonoBehaviour
{
    public InputActionReference punchAction;
    public Rigidbody[] punchRigidbodies;
    public float punchForce = 15f;
    public Animator animator;
    public float punchCooldown = 2f;
    
    private bool isPunching;
    private float lastPunchTime = -10f;

    void OnEnable()
    {
        if (punchAction != null && punchAction.action != null)
        {
            punchAction.action.Enable();

            PlayerController myController = GetComponentInParent<PlayerController>();
            if (myController != null)
            {
                int pIndex = myController.playerIndex;
                if (pIndex >= 2)
                {
                    int gamepadIndex = pIndex - 2;
                    UnityEngine.InputSystem.InputDevice[] deviceArray = new UnityEngine.InputSystem.InputDevice[0];
                    if (UnityEngine.InputSystem.Gamepad.all.Count > gamepadIndex)
                    {
                        deviceArray = new UnityEngine.InputSystem.InputDevice[] { UnityEngine.InputSystem.Gamepad.all[gamepadIndex] };
                    }
                    
                    var devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<UnityEngine.InputSystem.InputDevice>(deviceArray);
                    
                    if (punchAction.action.actionMap != null)
                        punchAction.action.actionMap.devices = devices;
                }
            }
        }
    }

    void OnDisable()
    {
        if (punchAction != null) punchAction.action.Disable();
    }

    void Update()
    {
        if (punchAction != null && punchAction.action.WasPressedThisFrame())
        {
            if (Time.time >= lastPunchTime + punchCooldown)
            {
                lastPunchTime = Time.time;
                Punch();
            }
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

                
                bool shouldStun = false;
                PlayerController myController = GetComponentInParent<PlayerController>();
                if (myController != null && myController.pelvis != null)
                {
                 
                    Vector3 horizVel = new Vector3(myController.pelvis.velocity.x, 0, myController.pelvis.velocity.z);
                    if (horizVel.magnitude > 4f)
                    {
                        shouldStun = true;
                    }
                }

                stunt.ReceivePunch(hitDir, punchForce * 0.8f, shouldStun);
            }

            if (collision.collider.CompareTag("egg") || collision.gameObject.CompareTag("egg"))
            {
                Rigidbody eggRb = collision.collider.attachedRigidbody;
                if (eggRb != null)
                {
                    Vector3 hitDir = (collision.transform.position - transform.position).normalized;
                    hitDir.y = 0.5f;
                    hitDir.Normalize();
                    eggRb.AddForce(hitDir * punchForce * 1.5f, ForceMode.Impulse);
                }
            }
        }
    }
}

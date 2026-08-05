using UnityEngine;
using UnityEngine.InputSystem;

public class PunchMechanic : MonoBehaviour
{
    public InputActionReference punchAction;
    private UnityEngine.InputSystem.InputAction runtimePunchAction;
    public Rigidbody[] punchRigidbodies;
    public float punchForce = 15f;
    public Animator animator;
    public float punchCooldown = 2f;
    
    private bool isPunching;
    private float lastPunchTime = -10f;

    void OnEnable()
    {
        RefreshInputs();
    }

    public void RefreshInputs()
    {
        PlayerController myController = GetComponentInParent<PlayerController>();
        if (myController != null && myController.runtimeActionMap != null && punchAction != null && punchAction.action != null)
        {
            if (runtimePunchAction != null) runtimePunchAction.Disable();
            
            string originalName = punchAction.action.name;
            string baseName = originalName.Substring(0, originalName.Length - 1);
            runtimePunchAction = myController.runtimeActionMap.FindAction(baseName + myController.actionSuffix);
            
            if (runtimePunchAction != null)
            {
                runtimePunchAction.Enable();
            }
        }
    }

    void OnDisable()
    {
        if (runtimePunchAction != null) runtimePunchAction.Disable();
    }

    void Update()
    {
        if (runtimePunchAction != null && runtimePunchAction.WasPressedThisFrame())
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

    private float lastHitSoundTime = -10f;

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
            else
            {
                if (collision.collider.CompareTag("egg") || collision.gameObject.CompareTag("egg"))
                {
                    Rigidbody eggRb = collision.collider.attachedRigidbody;
                    if (eggRb != null)
                    {
                        Vector3 hitDir = (collision.transform.position - transform.position).normalized;
                        hitDir.y = 0.5f;
                        hitDir.Normalize();
                        eggRb.AddForce(hitDir * punchForce * 1.5f, ForceMode.Impulse);

                        StuntPunch myStunt = GetComponentInParent<StuntPunch>();
                        if (myStunt != null && Time.time >= lastHitSoundTime + 0.2f)
                        {
                            lastHitSoundTime = Time.time;
                            myStunt.PlayPunchSound();
                        }
                    }
                }
            }
        }
    }
}

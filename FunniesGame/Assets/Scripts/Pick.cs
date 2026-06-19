using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{

  


    private bool hold;
    private Rigidbody grabbedRb;
    public UnityEngine.InputSystem.InputActionReference grabAction;
    public Animator animator;
    public bool RightHand;

    void OnEnable()
    {
        if (grabAction != null && grabAction.action != null)
            grabAction.action.Enable();
    }

    void OnDisable()
    {
        if (grabAction != null && grabAction.action != null)
            grabAction.action.Disable();
    }

    private bool brokenGrab = false;
    private float stealProgress = 0f;
    private Rigidbody targetToSteal;
    private PlayerController grabbedPlayer;

    private Renderer grabbedRenderer;
    private Material grabbedMaterial;
    private Color originalEmission;

    void Update()
    {
        if (grabAction != null && grabAction.action != null)
        {
            if (grabAction.action.IsPressed() && !brokenGrab)
            {
                if (RightHand) animator.SetBool("isRightHand", true);
                else animator.SetBool("isLeftHand", true);
                
                hold = true;

                FixedJoint currentJoint = GetComponent<FixedJoint>();
                if (currentJoint != null && grabbedRb != null && currentJoint.connectedBody == null)
                {
                    ReleaseGrab();
                }

                if (targetToSteal != null && grabbedRb == null)
                {
                    if (Vector3.Distance(transform.position, targetToSteal.position) < 5f)
                    {
                        Pick owner = null;
                        Pick[] allPicks = FindObjectsOfType<Pick>();
                        foreach (Pick p in allPicks)
                        {
                            if (p != this && p.grabbedRb == targetToSteal) { owner = p; break; }
                        }

                        if (owner != null)
                        {
                            stealProgress += Time.deltaTime;
                            targetToSteal.AddForce((transform.position - targetToSteal.position).normalized * 50f, ForceMode.Force);

                            if (stealProgress > 0.6f)
                            {
                                owner.ForceDrop();
                                DoGrab(targetToSteal);
                                targetToSteal = null;
                                stealProgress = 0f;
                            }
                        }
                        else
                        {
                            targetToSteal = null;
                            stealProgress = 0f;
                        }
                    }
                    else
                    {
                        targetToSteal = null;
                        stealProgress = 0f;
                    }
                }
            }
            else
            {
                if (RightHand) animator.SetBool("isRightHand", false);
                else animator.SetBool("isLeftHand", false);

                ReleaseGrab();

                if (!grabAction.action.IsPressed())
                {
                    brokenGrab = false;
                    stealProgress = 0f;
                    targetToSteal = null;
                }
            }
        }
    }

    void OnJointBreak(float breakForce)
    {
        ForceDrop();
    }

    public void ForceDrop()
    {
        brokenGrab = true;
        if (RightHand) animator.SetBool("isRightHand", false);
        else animator.SetBool("isLeftHand", false);
        ReleaseGrab();
    }

    void ReleaseGrab()
    {
        if (grabbedPlayer != null)
        {
            grabbedPlayer.grabbersCount--;
            if (grabbedPlayer.grabbersCount < 0) grabbedPlayer.grabbersCount = 0;
            grabbedPlayer = null;
        }
        if (grabbedMaterial != null && grabbedMaterial.HasProperty("_EmissionColor"))
        {
            grabbedMaterial.SetColor("_EmissionColor", originalEmission);
            grabbedMaterial = null;
        }
        grabbedRenderer = null;

        if (grabbedRb != null)
        {
            PaintTube tube = grabbedRb.GetComponent<PaintTube>();
            if (tube != null) tube.OnDropped();
        }

        hold = false;
        grabbedRb = null;
        FixedJoint fj = GetComponent<FixedJoint>();
        if (fj != null) Destroy(fj);
    }

    private PlayerController myController;

    void Start()
    {
        myController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionStay(Collision col)
    {
        if (hold && GetComponent<FixedJoint>() == null)
        {
            PlayerController otherController = col.collider.GetComponentInParent<PlayerController>();
            if (otherController != null && otherController == myController) return;

            Rigidbody rb = col.rigidbody;
            if (rb == null) rb = col.collider.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Pick owner = null;
                Pick[] allPicks = FindObjectsOfType<Pick>();
                foreach (Pick p in allPicks)
                {
                    if (p != this && p.grabbedRb == rb)
                    {
                        owner = p;
                        break;
                    }
                }

                if (owner != null)
                {
                    if (targetToSteal != rb)
                    {
                        targetToSteal = rb;
                        stealProgress = 0f;
                    }
                    return; 
                }

                DoGrab(rb);
            }
        }
    }

    private void DoGrab(Rigidbody rb)
    {
        FixedJoint fj = gameObject.AddComponent<FixedJoint>();
        fj.connectedBody = rb;
        grabbedRb = rb;

        PaintTube tube = rb.GetComponent<PaintTube>();
        if (tube != null)
        {
            fj.breakForce = Mathf.Infinity;
            fj.breakTorque = Mathf.Infinity;

            Renderer myRenderer = myController.GetComponentInChildren<Renderer>();
            Color myColor = myRenderer != null ? myRenderer.material.color : Color.white;
            tube.OnGrabbed(myController.playerIndex, myColor);
        }
        else
        {
            fj.breakForce = 600f;
            fj.breakTorque = 600f;
        }

        PlayerController otherController = rb.GetComponentInParent<PlayerController>();
        if (otherController != null)
        {
            grabbedPlayer = otherController;
            grabbedPlayer.grabbersCount++;
        }

        if (tube == null)
        {
            grabbedRenderer = rb.GetComponentInChildren<Renderer>();
            if (grabbedRenderer != null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level8")
            {
                grabbedMaterial = grabbedRenderer.material;
                if (grabbedMaterial.HasProperty("_EmissionColor"))
                {
                    originalEmission = grabbedMaterial.GetColor("_EmissionColor");
                    grabbedMaterial.EnableKeyword("_EMISSION");
                    grabbedMaterial.SetColor("_EmissionColor", Color.white * 0.5f);
                }
            }
        }
    }
}

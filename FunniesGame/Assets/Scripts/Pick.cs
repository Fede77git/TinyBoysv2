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
    private PlayerController grabbedPlayer;

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
            }
            else
            {
                if (RightHand) animator.SetBool("isRightHand", false);
                else animator.SetBool("isLeftHand", false);

                ReleaseGrab();

                if (!grabAction.action.IsPressed())
                {
                    brokenGrab = false;
                }
            }
        }
    }

    void OnJointBreak(float breakForce)
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

            Rigidbody rb = col.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                fj.breakForce = 2500f;
                fj.breakTorque = 2500f;
                fj.connectedBody = rb;
                grabbedRb = rb;

                if (otherController != null)
                {
                    grabbedPlayer = otherController;
                    grabbedPlayer.grabbersCount++;
                }
            }
        }
    }
}


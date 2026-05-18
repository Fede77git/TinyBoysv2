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

    void Update()
    {
        if (grabAction != null && grabAction.action != null)
        {
            if (grabAction.action.IsPressed())
            {
                if (RightHand) animator.SetBool("isRightHand", true);
                else animator.SetBool("isLeftHand", true);
                
                hold = true;

                FixedJoint currentJoint = GetComponent<FixedJoint>();
                if (currentJoint != null && grabbedRb != null && currentJoint.connectedBody == null)
                {
                    Destroy(currentJoint);
                    grabbedRb = null;
                }
            }
            else
            {
                if (RightHand) animator.SetBool("isRightHand", false);
                else animator.SetBool("isLeftHand", false);

                hold = false;
                grabbedRb = null;
                Destroy(GetComponent<FixedJoint>());
            }
        }
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
                fj.connectedBody = rb;
                grabbedRb = rb;
            }
        }
    }
}


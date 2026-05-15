using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{

  


    private bool hold;
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
            }
            else
            {
                if (RightHand) animator.SetBool("isRightHand", false);
                else animator.SetBool("isLeftHand", false);

                hold = false;
                Destroy(GetComponent<FixedJoint>());
            }
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (hold && GetComponent<FixedJoint>() == null)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("nocoll")) return;

            Rigidbody rb = col.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FixedJoint fj = gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = rb;
            }
            else
            {

                gameObject.AddComponent<FixedJoint>();
            }
        }
    }
}


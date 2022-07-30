using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick4 : MonoBehaviour
{
    private bool hold;
    public KeyCode grabKey;
    
    public Animator animator;
    public bool RightHand;

    void Update()
    {

        if (Input.GetKey(grabKey))
        {
            if (RightHand)
            {
                animator.SetBool("isRightHand", true);
            }
            else
            {
                animator.SetBool("isLeftHand", true);
            }
            hold = true;
        }
        else
        {
            if (RightHand)
            {
                animator.SetBool("isRightHand", false);
            }
            else
            {
                animator.SetBool("isLeftHand", false);
            }

            hold = false;
            Destroy(GetComponent<FixedJoint>());
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (hold/* && col.transform.tag == "Object"*/)
        {
            Rigidbody rb = col.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
                fj.connectedBody = rb;
            }
            else
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{

    //public Animator animator;
    //private GameObject objectG;
    //public Rigidbody rb;
    //public int leftOrRight;
    //public bool picking = false;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}


    //void Update()
    //{

    //    if (Input.GetMouseButtonDown(leftOrRight))
    //    {
    //        if (leftOrRight == 0)
    //        {
    //            animator.SetBool("isLeftHand", true);

    //        }
    //        else if (leftOrRight == 1)
    //        {
    //            animator.SetBool("isRightHand", true);
    //        }


    //        FixedJoint fx = objectG.AddComponent<FixedJoint>();
    //        fx.connectedBody = rb;
    //        fx.breakForce = 8000;
    //    }
    //    else if (Input.GetMouseButtonDown(leftOrRight))
    //    {
    //        if (leftOrRight == 0)
    //        {
    //            animator.SetBool("isLeftHand", false);

    //        }
    //        else if (leftOrRight == 1)
    //        {
    //            animator.SetBool("isRightHand", false);
    //        }

    //        if (objectG != null)
    //        {
    //            Destroy(objectG.GetComponent<FixedJoint>());
    //        }

    //        objectG = null;
    //    }

    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Object"))      /*sacar si quiero q se pueda agarrar cualquier cosa*/
    //    {
    //        objectG = other.gameObject;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{

    //    objectG = null;

    //}


    private bool hold;
    public KeyCode grabKey;
    public bool canGrab;
    public Animator animator;
    public bool RightHand;

    void Update()
    {
        //if (canGrab)
        //{
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
        //}
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



using UnityEngine;
using UnityEngine.InputSystem;

public class Picking2 : MonoBehaviour
{
    //private bool hold;

    //public Animator animator;
    //public bool RightHand;
    //public PlayerControll playerControls;
    //private InputAction leftHand;
    //private InputAction rightHand;


    //void Awake()
    //{
    //    playerControls = new PlayerControll();

    //}

    //private void OnEnable()
    //{
    //    leftHand = playerControls.Hands.LeftHand;
    //    leftHand.Enable();
    //    leftHand.performed += LeftHandOn;

    //    rightHand = playerControls.Hands.RightHand;
    //    rightHand.Enable();
    //    rightHand.performed += RightHandOn;
    //}

    //private void OnDisable()
    //{
    //    leftHand.Disable();
    //    leftHand.canceled += LeftHandOff;

    //    rightHand.Disable();
    //    rightHand.canceled += RightHandOff;

    //    //hold = false;
    //    //Destroy(GetComponent<FixedJoint>());
    //}

    //void LeftHandOn(InputAction.CallbackContext context)
    //{
    //    animator.SetBool("isLeftHand", true);
    //    hold = true;
    //}

    //void RightHandOn(InputAction.CallbackContext context)
    //{
    //    animator.SetBool("isRightHand", true);
    //    hold = true;
    //}

    //void LeftHandOff(InputAction.CallbackContext context)
    //{
    //    animator.SetBool("isLeftHand", false);
    //    hold = false;
    //    Destroy(GetComponent<FixedJoint>());
    //}
    //void RightHandOff(InputAction.CallbackContext context)
    //{
    //    animator.SetBool("isRightHand", false);
    //    hold = false;
    //    Destroy(GetComponent<FixedJoint>());
    //}


    ////void Update()
    ////{

    ////    if ()
    ////    {
    ////        if (RightHand)
    ////        {
    ////            animator.SetBool("isRightHand", true);
    ////        }
    ////        else
    ////        {
    ////            animator.SetBool("isLeftHand", true);
    ////        }
    ////        hold = true;
    ////    }
    ////    else
    ////    {
    ////        if (RightHand)
    ////        {
    ////            animator.SetBool("isRightHand", false);
    ////        }
    ////        else
    ////        {
    ////            animator.SetBool("isLeftHand", false);
    ////        }

    ////        hold = false;
    ////        Destroy(GetComponent<FixedJoint>());
    ////    }

    ////}

    //private void OnTriggerEnter(Collider col)
    //{
    //    if (hold/* && col.transform.tag == "Object"*/)
    //    {
    //        Rigidbody rb = col.transform.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
    //            fj.connectedBody = rb;
    //        }
    //        else
    //        {
    //            FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
    //        }
    //    }
    //}
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

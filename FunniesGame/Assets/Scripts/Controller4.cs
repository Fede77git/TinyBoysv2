using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Controller4 : MonoBehaviour
{
    public float speed = 150;
    public float strafeSpeed = 100;
    public float jumpForce;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;

    public Animator animator;
    public Text textWin;
    public Text textEsc;
    public Text textFell;

    public bool dead4;
    public void Start()
    {
        pelvis = GetComponent<Rigidbody>();
        dead4 = false;
    }

    public void Update()
    {
        if (transform.position.y < 2 || transform.position.y > 50)
        {
            dead4 = true;
            Dead();
            //textFell.text = "Green player fell of the map";

        }

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.I))
        {
            pelvis.AddForce(pelvis.transform.forward * speed);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.J))
        {
            pelvis.AddForce(-pelvis.transform.right * strafeSpeed);
            animator.SetBool("isWalking", true);
        }


        if (Input.GetKey(KeyCode.L))
        {
            pelvis.AddForce(pelvis.transform.right * strafeSpeed);
            animator.SetBool("isWalking", true);
        }

        if (Input.GetKey(KeyCode.K))
        {
            pelvis.AddForce(-pelvis.transform.forward * speed);
            animator.SetBool("isWalking", true);
        }

        if (isGrounded && floored)
        {
            if (Input.GetKey(KeyCode.RightAlt))
            {

                pelvis.AddForce(new Vector3(0, jumpForce, 0));
                isGrounded = false;

            }
        }

    }
    public void Dead()
    {
        //textWin.text = "Orange Player Wins";
        //textEsc.text = "Press Escape to continue";

        //Time.timeScale = 0;



    }


}

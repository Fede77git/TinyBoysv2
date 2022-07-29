using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller2Pj : MonoBehaviour
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
    public bool dead2;

    public  void Start()
    {
        dead2 = false;
        pelvis = GetComponent<Rigidbody>();
        
    }

    public void Update()
    {
        if (transform.position.y < 2 || transform.position.y > 50)
        {
            Dead2();
            dead2 = true;
            //textFell.text = "Purple player fell of the map";

        }
    }
    void FixedUpdate()
{
    if (Input.GetKey(KeyCode.W))
    {
        pelvis.AddForce(pelvis.transform.forward * speed);
        animator.SetBool("isWalking", true);
    }
    else
    {
        animator.SetBool("isWalking", false);
    }

    if (Input.GetKey(KeyCode.A))
    {
        pelvis.AddForce(-pelvis.transform.right * strafeSpeed);
        animator.SetBool("isWalking", true);
    }


    if (Input.GetKey(KeyCode.D))
    {
        pelvis.AddForce(pelvis.transform.right * strafeSpeed);
        animator.SetBool("isWalking", true);
    }

    if (Input.GetKey(KeyCode.S))
    {
        pelvis.AddForce(-pelvis.transform.forward * speed);
        animator.SetBool("isWalking", true);
    }

    if (isGrounded && floored)
    {
        if (Input.GetKey(KeyCode.Space))
        {

            pelvis.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;

        }
    }

}
    public void Dead2()
    {
        //textWin.text = "Orange Player Wins";
        //textEsc.text = "Press Escape to continue";

        //Time.timeScale = 0;



    }

   
}

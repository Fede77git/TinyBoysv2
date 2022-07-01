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
    private int cont;
    public Text textCollected;
    public Text textWin;

    void Start()
    {
        pelvis = GetComponent<Rigidbody>();
        cont = 0;
        textWin.text = "";
        SetText();
    }

    private void Update()
    {
        if (transform.position.y < 2)
        {
            Dead();
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
    public void Dead()
    {
        
        GameManager.gameOver = true;



    }

    private void SetText()
    {
        textCollected.text = " " + cont.ToString();
        if (cont >= 5)
        {

            textWin.text = "Purple Player Wins";
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coleccionable"))
        {
            cont = cont + 1;
            SetText();
            other.gameObject.SetActive(false);
        }
    }
}

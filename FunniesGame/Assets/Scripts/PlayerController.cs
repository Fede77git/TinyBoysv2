
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 150;
    public float strafeSpeed = 100;
    public float jumpForce;

    public Rigidbody pelvis;
    public bool isGrounded;

    
    void Start()
    {
        pelvis = GetComponent<Rigidbody>();

    }

    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pelvis.AddForce(pelvis.transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            pelvis.AddForce(-pelvis.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            pelvis.AddForce(pelvis.transform.right * strafeSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pelvis.AddForce(-pelvis.transform.forward * speed);
        }
    }


}

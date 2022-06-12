
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 150;
    public float strafeSpeed = 100;
    public float jumpForce;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;

    public Animator animator;

    
    
    Vector3 moveDirection = Vector3.zero;
    public InputAction playerControls;
    public InputAction playerJump;
    
    
    void Start()
    {
        pelvis = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

   

    
    
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        pelvis.velocity = new Vector3(moveDirection.x * strafeSpeed,0, moveDirection.y * speed);
    }

    //void FixedUpdate()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        pelvis.AddForce(pelvis.transform.forward * speed);
    //        animator.SetBool("isWalking", true);
    //    }
    //    else
    //    {
    //        animator.SetBool("isWalking", false);
    //    }

    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        pelvis.AddForce(-pelvis.transform.right * strafeSpeed);
    //        animator.SetBool("isWalking", true);
    //    }
       

    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        pelvis.AddForce(pelvis.transform.right * strafeSpeed);
    //        animator.SetBool("isWalking", true);
    //    }
        
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        pelvis.AddForce(-pelvis.transform.forward * speed);
    //        animator.SetBool("isWalking", true);
    //    }

       

    //}

    void Jump()
    {
        if (isGrounded && floored)
        {
            
            

                pelvis.AddForce(new Vector3(0, jumpForce, 0));
                isGrounded = false;

            
        }
    }
}

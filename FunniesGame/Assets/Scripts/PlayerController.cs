using UnityEngine.UI;
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

    
    
    int isWalking;
    
    Vector3 moveDirection ;
    public PlayerControll playerControls;
    private InputAction move;
    private InputAction jump;

   
    public Text textWin;


    void Awake()
    {
        playerControls = new PlayerControll();
    }

    void Start()
    {
        pelvis = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isWalking = Animator.StringToHash("isWalking");

    }

    private void OnEnable()
    {
        move = playerControls.PlayerKM.Move;
        move.Enable();
        animator.SetBool("isWalking", true);
        jump = playerControls.PlayerKM.Jump;
        jump.Enable();
        jump.performed += Jump;

    }

    private void OnDisable()
    {
        move.Disable();
        //animator.SetBool("isWalking", false);
        jump.Disable();
    }

   

    
    
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (transform.position.y < 2)
        {
            Dead();
        }
    }

    void FixedUpdate()
    {
        
        pelvis.velocity = new Vector3(moveDirection.x * speed,0, moveDirection.y * strafeSpeed);
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

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && floored)
        {

            pelvis.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;
            


        }
    }
    public void Dead()
    {

        textWin.text = "Purple Player Wins";
        Time.timeScale = 0;



    }
   
}

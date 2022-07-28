using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController3 : MonoBehaviour
{
    public float speed = 150;
    public float strafeSpeed = 100;
    public float jumpForce;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;

    public Animator animator;

    public int playerIndex = 0;

    int isWalking;

    Vector3 moveDirection;
    public PlayerControll1 playerControls1;
    private InputAction move;
    private InputAction jump;


    public Text textWin;
    public Text textEsc;
    public Text textFell;

    void Awake()
    {
        playerControls1 = new PlayerControll1();
    }

    void Start()
    {
        pelvis = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isWalking = Animator.StringToHash("isWalking");

    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
    private void OnEnable()
    {
        move = playerControls1.PlayerKM.Move;
        move.Enable();
        animator.SetBool("isWalking", true);
        jump = playerControls1.PlayerKM.Jump;
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

        if (transform.position.y < 2 || transform.position.y > 50)
        {
            Dead();
            textFell.text = "Orange player fell of the map";
        }
    }

    void FixedUpdate()
    {

        pelvis.velocity = new Vector3(moveDirection.x * strafeSpeed, 0, moveDirection.y * speed);

    }

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
        textEsc.text = "Press Escape to continue";

        Time.timeScale = 0;



    }

}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 15;
    public float strafeSpeed = 10;
    public float jumpForce = 2;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;
    public Animator animator;
    public bool isDead;

    public int playerIndex = 0;
    public UnityEngine.UI.Text textWin;
    public UnityEngine.UI.Text textEsc;
    public UnityEngine.UI.Text textFell;
    public bool dead1;

    private Vector2 moveDirection;
    private float jumpCooldown;
    
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        
        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += Jump;
        }
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        
        if (jumpAction != null)
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= Jump;
        }
    }

    void Update()
    {
        if (jumpCooldown > 0f) jumpCooldown -= Time.deltaTime;

        if (moveAction != null && moveAction.action != null)
        {
            moveDirection = moveAction.action.ReadValue<Vector2>();
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", moveDirection.sqrMagnitude > 0.01f);
        }

        if (pelvis != null && (pelvis.position.y < 2 || pelvis.position.y > 50))
        {
            if (!isDead)
            {
                Dead();
            }
        }
    }

    private void CheckGround()
    {
        if (pelvis == null) return;

        int layerMask = ~LayerMask.GetMask("nocoll");
        
        if (pelvis.velocity.y <= 0.1f && Physics.Raycast(pelvis.position, Vector3.down, 1.2f, layerMask))
        {
            isGrounded = true;
            floored = true;
        }
        else
        {
            isGrounded = false;
            floored = false;
        }
    }

    void FixedUpdate()
    {
        if (!isDead && pelvis != null)
        {
            CheckGround();

            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 direction = camForward * (moveDirection.y * speed) + camRight * (moveDirection.x * strafeSpeed);
            
            Vector3 targetVelocity = direction;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                pelvis.MoveRotation(Quaternion.Slerp(pelvis.rotation, targetRotation, Time.fixedDeltaTime * 15f));
                
                if (animator != null)
                {
                    animator.transform.rotation = pelvis.rotation;
                }
            }

            Rigidbody[] allBodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in allBodies)
            {
                if (rb.gameObject.name.Contains("Head") || rb.gameObject.name.Contains("Arm"))
                {
                    continue;
                }

                Vector3 newVel = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
                rb.velocity = newVel;
            }
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && floored && !isDead && pelvis != null && jumpCooldown <= 0f)
        {
            pelvis.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            jumpCooldown = 0.2f;
        }
    }

    public void Dead()
    {
        isDead = true;
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.PlayerDied(playerIndex);
        }
    }
}

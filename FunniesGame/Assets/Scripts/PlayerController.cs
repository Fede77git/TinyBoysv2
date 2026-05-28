using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 8;
    public float strafeSpeed = 7;
    public float jumpForce = 30;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;
    public Animator animator;
    public bool isDead;

    public int playerIndex = 0;
    public bool dead1;
    public float groundCheckDistance = 1.0f;
    public int grabbersCount = 0;

    private Vector2 moveDirection;
    private float jumpCooldown;
    private bool hasIsWalking;
    private bool hasIsGrounded;
    private Vector3 knockbackVelocity;
    
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    public float minHeightLimit = -10f;
    public float maxHeightLimit = 50f;
    public float maxFallSpeed = -15f;

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

    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
        if (playerAudioSource == null)
        {
            playerAudioSource = gameObject.AddComponent<AudioSource>();
        }

        Collider[] myColliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < myColliders.Length; i++)
        {
            for (int j = i + 1; j < myColliders.Length; j++)
            {
                Physics.IgnoreCollision(myColliders[i], myColliders[j]);
            }
        }

        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "isWalking") hasIsWalking = true;
                if (param.name == "isGrounded") hasIsGrounded = true;
            }
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
            if (hasIsWalking) animator.SetBool("isWalking", moveDirection.sqrMagnitude > 0.01f);
            if (hasIsGrounded) animator.SetBool("isGrounded", isGrounded);
        }

        if (pelvis != null && (pelvis.position.y < minHeightLimit || pelvis.position.y > maxHeightLimit))
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

        if (pelvis.velocity.y <= 0.1f && Physics.Raycast(pelvis.position, Vector3.down, groundCheckDistance, layerMask))
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
            
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
            Vector3 targetVelocity = direction + knockbackVelocity;

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

                if (grabbersCount > 0)
                {
                    rb.AddForce(targetVelocity * 10f, ForceMode.Force);
                }
                else
                {
                    Vector3 newVel = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
                    
                    if (newVel.y < maxFallSpeed) 
                    {
                        newVel.y = maxFallSpeed;
                    }

                    rb.velocity = newVel;
                }
            }
        }
    }


    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 1f;
    
    public AudioClip coinSound;
    [Range(0f, 1f)] public float coinVolume = 1f;
    
    private AudioSource playerAudioSource;
    public float minPitch = 0.85f;
    public float maxPitch = 1.15f;

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && floored && !isDead && pelvis != null && jumpCooldown <= 0f)
        {
            pelvis.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            jumpCooldown = 0.2f;

            if (jumpSound != null && playerAudioSource != null)
            {
                playerAudioSource.pitch = Random.Range(minPitch, maxPitch);
                playerAudioSource.PlayOneShot(jumpSound, jumpVolume);
            }
        }
    }

    public void Dead()
    {
        isDead = true;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.PlayerDied(playerIndex);
        }
        else if (LevelManager2.Instance != null)
        {
            LevelManager2.Instance.PlayerDied(playerIndex);
        }
        else if (LevelManager3.Instance != null)
        {
            LevelManager3.Instance.PlayerDied(playerIndex);
        }
        else if (LevelManager5.Instance != null)
        {
            LevelManager5.Instance.PlayerDied(playerIndex);
        }
        else if (LevelManager6.Instance != null)
        {
            LevelManager6.Instance.PlayerDied(playerIndex);
        }
    }

    public void AddKnockback(Vector3 force)
    {
        knockbackVelocity += force;
    }

    public void PlayCoinSound()
    {
        if (coinSound != null && playerAudioSource != null)
        {
            playerAudioSource.pitch = 1f;
            playerAudioSource.PlayOneShot(coinSound, coinVolume);
        }
    }
}

using UnityEngine;

public class StampedeObstacle : MonoBehaviour
{
    public float speed = 10f;
    public float pushForce = 500f;
    public Vector3 moveDirection = new Vector3(-1, 0, 0);
    public bool useLocalDirection = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null)
        {
            myCollider.isTrigger = true;
        }
        
        Vector3 dir = useLocalDirection ? transform.TransformDirection(moveDirection.normalized) : moveDirection.normalized;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        Vector3 dir = useLocalDirection ? transform.TransformDirection(moveDirection.normalized) : moveDirection.normalized;
        Vector3 newPos = rb.position + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                if (player.pelvis != null)
                {
                    player.pelvis.AddExplosionForce(pushForce * 5f, transform.position, 5f);
                }

                Vector3 pushDir = useLocalDirection ? transform.TransformDirection(moveDirection.normalized) : moveDirection.normalized;
                player.AddKnockback(pushDir * (pushForce * 0.1f));
            }
        }
    }
}

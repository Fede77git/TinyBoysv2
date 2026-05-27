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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDir = useLocalDirection ? transform.TransformDirection(moveDirection.normalized) : moveDirection.normalized;
                playerRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
            }
        }
    }
}

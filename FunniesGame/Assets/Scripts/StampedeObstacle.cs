using UnityEngine;

public class StampedeObstacle : MonoBehaviour
{
    public float speed = 10f;
    public float pushForce = 500f;
    public bool useLocalZ = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = useLocalZ ? transform.forward : Vector3.forward;
        Vector3 newPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = useLocalZ ? transform.forward : Vector3.forward;
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}

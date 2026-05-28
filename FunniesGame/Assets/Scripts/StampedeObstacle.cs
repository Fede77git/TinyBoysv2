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
            if (player != null && player.pelvis != null)
            {
                Vector3 localPos = transform.InverseTransformPoint(player.pelvis.position);
                
                float absX = Mathf.Abs(localPos.x);
                float absY = Mathf.Abs(localPos.y);
                float absZ = Mathf.Abs(localPos.z) * 0.5f;

                if (absY > absX && absY > absZ && localPos.y > 0)
                {
                    player.pelvis.AddForce(Vector3.up * (pushForce * 0.15f), ForceMode.Impulse);
                }
                else if (absZ > absX && localPos.z > 0)
                {
                    Vector3 pushDir = transform.forward;
                    player.AddKnockback(pushDir * (pushForce * 0.1f));
                    player.pelvis.AddForce(Vector3.up * (pushForce * 0.05f), ForceMode.Impulse);
                }
                else
                {
                    Vector3 pushDir = Mathf.Sign(localPos.x) * transform.right;
                    player.AddKnockback(pushDir * (pushForce * 0.05f));
                }
            }
        }
    }
}

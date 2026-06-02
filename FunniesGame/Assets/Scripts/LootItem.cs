using UnityEngine;

public class LootItem : MonoBehaviour
{
    public int pointsValue = 1;
    public float weight = 1f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = weight;
        }
    }
}

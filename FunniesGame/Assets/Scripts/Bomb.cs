
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10f;
    public float radius = 5f;
    public float force = 1f;
   
    void Start()
    {
        
    }

   
    void FixedUpdate()
    {
        if (bomb == enabled)
        {
            Invoke("Explode", 10);
            
        }
    }

    void Explode()
    {
        Vector3 explosionP = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionP, radius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionP, radius, force, ForceMode.Impulse);
            }
            

        }
    }
}

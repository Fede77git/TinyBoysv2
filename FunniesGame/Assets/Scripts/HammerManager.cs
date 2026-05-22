using UnityEngine;
using System.Collections;

public class HammerManager : MonoBehaviour
{
    public Transform[] impactPoints;
    public GameObject indicatorPrefab;
    public float initialDelay = 3f;
    public float delayMultiplier = 0.95f;
    public float minDelay = 0.5f;
    
    public float explosionForce = 5000f;
    public float explosionRadius = 5f;

    private float currentDelay;
    private GameObject currentIndicator;
    private Transform currentTarget;

    void Start()
    {
        currentDelay = initialDelay;
        StartCoroutine(HammerRoutine());
    }

    IEnumerator HammerRoutine()
    {
        while (true)
        {
            if (impactPoints != null && impactPoints.Length > 0)
            {
                currentTarget = impactPoints[Random.Range(0, impactPoints.Length)];
                
                if (indicatorPrefab != null)
                {
                    currentIndicator = Instantiate(indicatorPrefab, currentTarget.position, Quaternion.identity);
                }

                yield return new WaitForSeconds(currentDelay);

                HitTarget(currentTarget.position);

                if (currentIndicator != null)
                {
                    Destroy(currentIndicator);
                }

                currentDelay *= delayMultiplier;
                if (currentDelay < minDelay)
                {
                    currentDelay = minDelay;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    void HitTarget(Vector3 position)
    {
        transform.position = position;

        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, position, explosionRadius);

                PlayerController player = hit.GetComponentInParent<PlayerController>();
                if (player != null && !player.isDead)
                {
                    player.Dead();
                }
            }
        }
    }
}

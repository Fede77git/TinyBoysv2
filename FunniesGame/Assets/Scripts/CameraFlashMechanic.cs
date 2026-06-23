using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashMechanic : MonoBehaviour
{
    public Light spotlight;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public List<Transform> waypoints;
    public float moveSpeed = 2f;
    public float cycleTime = 15f;
    public float warningTime = 3f;
    public float freezeDuration = 3f;
    public Material grayMaterial;

    private int currentWaypointIndex = 0;
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    private bool isFlashing = false;

    private void Start()
    {
        if (spotlight != null)
        {
            spotlight.color = normalColor;
        }
        StartCoroutine(FlashCycleRoutine());
    }

    private void Update()
    {
        if (!isFlashing && waypoints.Count > 0)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrigger.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrigger.Remove(other.gameObject);
        }
    }

    private IEnumerator FlashCycleRoutine()
    {
        while (true)
        {
            isFlashing = false;
            if (spotlight != null) spotlight.color = normalColor;

            yield return new WaitForSeconds(cycleTime - warningTime);

            if (spotlight != null) spotlight.color = warningColor;

            yield return new WaitForSeconds(warningTime);

            isFlashing = true;

            foreach (GameObject player in playersInTrigger)
            {
                if (player != null)
                {
                    StartCoroutine(FreezePlayerRoutine(player));
                }
            }

            if (waypoints.Count > 0)
            {
                int randomIndex = Random.Range(0, waypoints.Count);
                currentWaypointIndex = randomIndex;
                transform.position = waypoints[randomIndex].position;
            }
        }
    }

    private IEnumerator FreezePlayerRoutine(GameObject player)
    {
        Rigidbody[] rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        
        Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Renderer rend in renderers)
        {
            originalMaterials[rend] = rend.materials;
            Material[] grayMats = new Material[rend.materials.Length];
            for (int i = 0; i < grayMats.Length; i++)
            {
                grayMats[i] = grayMaterial;
            }
            rend.materials = grayMats;
        }

        yield return new WaitForSeconds(freezeDuration);

        if (player != null)
        {
            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }

            foreach (Renderer rend in renderers)
            {
                if (rend != null && originalMaterials.ContainsKey(rend))
                {
                    rend.materials = originalMaterials[rend];
                }
            }
        }
    }
}

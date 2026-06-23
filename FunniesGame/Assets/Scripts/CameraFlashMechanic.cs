using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashMechanic : MonoBehaviour
{
    public Light spotlight;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public BoxCollider movementArea;
    public float moveSpeed = 2f;
    public float cycleTime = 15f;
    public float warningTime = 3f;
    public float freezeDuration = 3f;
    public Material grayMaterial;

    private Vector3 currentTargetPosition;
    private HashSet<GameObject> playersInTrigger = new HashSet<GameObject>();
    private bool isFlashing = false;

    private void Start()
    {
        if (spotlight != null)
        {
            spotlight.color = normalColor;
        }
        PickNewRandomTarget();
        StartCoroutine(FlashCycleRoutine());
    }

    private void Update()
    {
        if (!isFlashing && movementArea != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
            {
                PickNewRandomTarget();
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

            PickNewRandomTarget();
            transform.position = currentTargetPosition;
        }
    }

    private void PickNewRandomTarget()
    {
        if (movementArea != null)
        {
            Bounds bounds = movementArea.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);
            currentTargetPosition = new Vector3(randomX, transform.position.y, randomZ);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashMechanic : MonoBehaviour
{
    public Light spotlight;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public BoxCollider movementArea;
    public Transform cameraModel;
    public Light lensFlashLight;
    public float moveSpeed = 2f;
    public float cycleTime = 15f;
    public float warningTime = 3f;
    public float freezeDuration = 3f;
    public float trackingOffset = 0f;
    public float chargeSpeed = 15f;
    public Material grayMaterial;

    private Vector3 currentTargetPosition;
    private Dictionary<GameObject, int> playersInTrigger = new Dictionary<GameObject, int>();
    private bool isFlashing = false;

    private void Start()
    {
        if (spotlight != null)
        {
            spotlight.color = normalColor;
        }
        if (lensFlashLight != null)
        {
            lensFlashLight.enabled = false;
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

        if (cameraModel != null)
        {
            cameraModel.position = new Vector3(cameraModel.position.x, cameraModel.position.y, transform.position.z + trackingOffset);
        }

        if (!isFlashing && LevelManager10.Instance != null)
        {
            foreach (GameObject player in new List<GameObject>(playersInTrigger.Keys))
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null && !pc.isDead)
                {
                    LevelManager10.Instance.AddPlayerCharge(pc.playerIndex, chargeSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            if (playersInTrigger.ContainsKey(pc.gameObject))
                playersInTrigger[pc.gameObject]++;
            else
                playersInTrigger[pc.gameObject] = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pc = other.GetComponentInParent<PlayerController>();
        if (pc != null && playersInTrigger.ContainsKey(pc.gameObject))
        {
            playersInTrigger[pc.gameObject]--;
            if (playersInTrigger[pc.gameObject] <= 0)
            {
                playersInTrigger.Remove(pc.gameObject);
            }
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

            foreach (GameObject player in new List<GameObject>(playersInTrigger.Keys))
            {
                if (player != null)
                {
                    StartCoroutine(FreezePlayerRoutine(player));
                }
            }

            if (lensFlashLight != null) lensFlashLight.enabled = true;
            
            yield return new WaitForSeconds(0.5f);
            
            if (lensFlashLight != null) lensFlashLight.enabled = false;

            PickNewRandomTarget();
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
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        Animator[] animators = player.GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.speed = 0;
        }

        Rigidbody[] rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        
        Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Renderer rend in renderers)
        {
            if (rend is ParticleSystemRenderer || rend is TrailRenderer) continue;

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
            if (pc != null) pc.enabled = true;

            foreach (Animator anim in animators)
            {
                if (anim != null) anim.speed = 1;
            }

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

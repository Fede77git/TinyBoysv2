using UnityEngine;

public class Eraser : MonoBehaviour
{
    public BoxCollider brushCollider;
    public AudioClip eraseSfx;
    [Range(0f, 1f)] public float eraseSfxVolume = 1f;
    private AudioSource audioSource;

    private bool isGrabbed = false;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
        
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider col in colliders)
        {
            if (col.isTrigger)
            {
                brushCollider = col;
                break;
            }
        }

        if (brushCollider != null)
        {
            brushCollider.enabled = false;
        }
    }

    public void OnGrabbed()
    {
        isGrabbed = true;

        if (brushCollider != null)
        {
            brushCollider.enabled = true;
        }
    }

    public void OnDropped()
    {
        isGrabbed = false;
        if (brushCollider != null)
        {
            brushCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            transform.position = initialPosition;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            return;
        }

        if (isGrabbed)
        {
            if (brushCollider != null && brushCollider.enabled)
            {
                Vector3 halfExtents = Vector3.Scale(brushCollider.size * 0.5f, brushCollider.transform.lossyScale) + (Vector3.one * 0.3f);
                Vector3 center = brushCollider.transform.TransformPoint(brushCollider.center);
                Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, brushCollider.transform.rotation);
                bool anyErasedThisFrame = false;
                
                foreach (var hitCollider in hitColliders)
                {
                    PaintableTile tile = hitCollider.GetComponent<PaintableTile>();
                    if (tile != null)
                    {
                        if (tile.Borrar())
                        {
                            anyErasedThisFrame = true;
                        }
                    }
                }

                if (anyErasedThisFrame && eraseSfx != null)
                {
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(eraseSfx, eraseSfxVolume);
                }
            }
        }
    }
}

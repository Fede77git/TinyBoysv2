using UnityEngine;
using UnityEngine.UI;

public class PaintTube : MonoBehaviour
{
    public float lifetime = 10f;
    public SphereCollider brushCollider;
    public GameObject tubeCap;
    public Renderer tubeRenderer;

    private int currentPlayerID = 0;
    private Color currentColor;
    private bool isGrabbed = false;
    private float currentLife;
    private Vector3 initialScale;

    private void Awake()
    {
        currentLife = lifetime;
        if (tubeRenderer != null) initialScale = tubeRenderer.transform.localScale;
        else initialScale = transform.localScale;
        if (brushCollider != null)
        {
            brushCollider.enabled = false;
        }
    }

    public void OnGrabbed(int newPlayerID, Color newColor)
    {
        currentPlayerID = newPlayerID;
        currentColor = newColor;
        isGrabbed = true;

        if (brushCollider != null)
        {
            brushCollider.enabled = true;
        }

        if (tubeRenderer != null)
        {
            foreach (Material mat in tubeRenderer.materials)
            {
                mat.color = newColor;
            }
        }

        if (tubeCap != null)
        {
            tubeCap.transform.SetParent(null);
            Rigidbody capRb = tubeCap.GetComponent<Rigidbody>();
            if (capRb == null) capRb = tubeCap.AddComponent<Rigidbody>();
            capRb.AddForce(Vector3.up * 2f + Random.insideUnitSphere * 1f, ForceMode.Impulse);
            Destroy(tubeCap, 3f);
            tubeCap = null;
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
        if (isGrabbed)
        {
            currentLife -= Time.deltaTime;

            if (brushCollider != null && brushCollider.enabled)
            {
                float radius = brushCollider.radius * Mathf.Max(brushCollider.transform.lossyScale.x, brushCollider.transform.lossyScale.y, brushCollider.transform.lossyScale.z);
                radius += 0.3f; 
                Collider[] hitColliders = Physics.OverlapSphere(brushCollider.transform.position, radius);
                foreach (var hitCollider in hitColliders)
                {
                    PaintableTile tile = hitCollider.GetComponent<PaintableTile>();
                    if (tile != null)
                    {
                        tile.Pintar(currentPlayerID, currentColor);
                    }
                }
            }

            float flattenedY = Mathf.Lerp(0.05f, initialScale.y, currentLife / lifetime);
            if (tubeRenderer != null)
            {
                tubeRenderer.transform.localScale = new Vector3(initialScale.x, flattenedY, initialScale.z);
            }
            else
            {
                transform.localScale = new Vector3(initialScale.x, flattenedY, initialScale.z);
            }

            if (currentLife <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

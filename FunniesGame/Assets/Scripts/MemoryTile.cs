using UnityEngine;
using UnityEngine.UI;

public class MemoryTile : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public Image tileImage;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (tileImage == null) tileImage = GetComponentInChildren<Image>();
        
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Setup(int newId, Sprite sprite)
    {
        id = newId;
        tileImage.sprite = sprite;
    }

    public void ShowImage(bool show)
    {
        tileImage.enabled = show;
    }

    public void Drop()
    {
        rb.isKinematic = false;
    }

    public void ResetTile()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PaintTube : MonoBehaviour
{
    public float lifetime = 10f;
    public SphereCollider brushCollider;
    public Image inkBar;

    private int currentPlayerID = 0;
    private Color currentColor;
    private bool isGrabbed = false;
    private float currentLife;

    private void Awake()
    {
        currentLife = lifetime;
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

        if (inkBar != null)
        {
            inkBar.color = newColor;
            inkBar.fillAmount = currentLife / lifetime;
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

    private void OnTriggerStay(Collider other)
    {
        PaintableTile tile = other.GetComponent<PaintableTile>();
        if (tile != null)
        {
            tile.Pintar(currentPlayerID, currentColor);
        }
    }

    private void Update()
    {
        if (isGrabbed)
        {
            currentLife -= Time.deltaTime;

            if (inkBar != null)
            {
                inkBar.fillAmount = currentLife / lifetime;
            }

            if (currentLife <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

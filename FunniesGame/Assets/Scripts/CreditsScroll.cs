using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    private RectTransform rectTransform;
    private float initialY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialY = rectTransform.anchoredPosition.y;
    }

    private void OnEnable()
    {
        ResetPosition(initialY);
    }

    private void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

    public void ResetPosition(float startY)
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY);
        }
    }
}

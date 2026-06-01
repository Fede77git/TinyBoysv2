using UnityEngine;
using UnityEngine.UI;

public class MemoryTile : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public Image tileImage;

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
}

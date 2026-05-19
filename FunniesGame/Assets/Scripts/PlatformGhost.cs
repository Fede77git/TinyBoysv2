
using UnityEngine;

public class PlatformGhost : MonoBehaviour
{
    bool isFalling = false;
    float speedD = 0f;
    public float fallSpeedDivider = 150f;
    public Color newColor;
    public Renderer render;
    public Color defaultColor;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            Destroy(gameObject, 10);
        }
    }

    private void Update()
    {
        if (isFalling)
        {
            speedD += Time.deltaTime / fallSpeedDivider;
            transform.position = new Vector3(transform.position.x, transform.position.y - speedD, transform.position.z);
            render = GetComponent<Renderer>();
            render.material.color = newColor;
        }
    }
}

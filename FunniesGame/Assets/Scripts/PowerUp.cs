using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedBoost = 3f;
    public float strafeSpeedBoost = 2f;
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            isCollected = true;
            player.speed += speedBoost;
            player.strafeSpeed += strafeSpeedBoost;
            gameObject.SetActive(false);
        }
    }
}

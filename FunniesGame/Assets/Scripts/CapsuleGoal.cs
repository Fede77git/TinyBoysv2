using UnityEngine;

public class CapsuleGoal : MonoBehaviour
{
    public int teamIndex; 
    public AudioSource goalSound;
    public ParticleSystem goalVFX;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("egg"))
        {
            if (LevelManager4.Instance != null)
            {
                LevelManager4.Instance.AddScore(teamIndex);
            }

            if (goalSound != null) goalSound.Play();
            if (goalVFX != null) goalVFX.Play();

            other.gameObject.SetActive(false);
        }
    }
}

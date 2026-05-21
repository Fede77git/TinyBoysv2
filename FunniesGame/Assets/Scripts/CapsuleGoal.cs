using UnityEngine;

public class CapsuleGoal : MonoBehaviour
{
    public int teamIndex; 

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("egg"))
        {
            if (LevelManager4.Instance != null)
            {
                LevelManager4.Instance.AddScore(teamIndex);
            }
            other.gameObject.SetActive(false);
        }
    }
}

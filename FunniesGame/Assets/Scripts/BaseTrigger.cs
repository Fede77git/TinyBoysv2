using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    public int playerIndex;
    public ScoringManager scoringManager;

    public GameObject fbxEffect;
    public AudioClip soundFx;
    [Range(0f, 1f)] public float hitVolume = 1f;
    
    public float cooldownTime = 1f;
    private float lastTriggerTime = -Mathf.Infinity;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LootItem loot = other.GetComponent<LootItem>();
        if (loot != null && scoringManager != null)
        {
            scoringManager.AddScore(playerIndex, loot.pointsValue);

            if (Time.time >= lastTriggerTime + cooldownTime)
            {
                lastTriggerTime = Time.time;
                
                if (fbxEffect != null)
                {
                    GameObject effectInstance = Instantiate(fbxEffect, transform.position, Quaternion.identity);
                    Destroy(effectInstance, 1f);
                }

                if (audioSource != null && soundFx != null)
                {
                    audioSource.PlayOneShot(soundFx, hitVolume);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LootItem loot = other.GetComponent<LootItem>();
        if (loot != null && scoringManager != null)
        {
            scoringManager.RemoveScore(playerIndex, loot.pointsValue);
        }
    }
}

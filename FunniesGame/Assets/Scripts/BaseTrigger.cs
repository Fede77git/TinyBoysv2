using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    public int playerIndex;
    public ScoringManager scoringManager;

    private void OnTriggerEnter(Collider other)
    {
        LootItem loot = other.GetComponent<LootItem>();
        if (loot != null && scoringManager != null)
        {
            scoringManager.AddScore(playerIndex, loot.pointsValue);
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

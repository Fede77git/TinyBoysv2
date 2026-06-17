using UnityEngine;

public class PaintableTile : MonoBehaviour
{
    private int ownerID = -1;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Pintar(int playerID, Color playerColor)
    {
        if (ownerID == playerID) return;

        ownerID = playerID;
        meshRenderer.material.color = playerColor;
    }
}

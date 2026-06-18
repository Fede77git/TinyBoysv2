using UnityEngine;

public class PaintableTile : MonoBehaviour
{
    private int ownerID = -1;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public bool Pintar(int playerID, Color playerColor)
    {
        if (ownerID == playerID) return false;

        ownerID = playerID;
        meshRenderer.material.color = playerColor;
        return true;
    }
}

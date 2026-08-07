using UnityEngine;

public class PaintableTile : MonoBehaviour
{
    private int ownerID = -1;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public int OwnerID { get { return ownerID; } }

    public bool Pintar(int playerID, Color playerColor)
    {
        if (ownerID == playerID) return false;

        int oldOwner = ownerID;
        ownerID = playerID;
        meshRenderer.material.color = playerColor;

        if (LevelManager9.Instance != null)
        {
            LevelManager9.Instance.OnTilePainted(oldOwner, playerID);
        }

        return true;
    }

    public bool Borrar()
    {
        if (ownerID == -1) return false;

        int oldOwner = ownerID;
        ownerID = -1;
        meshRenderer.material.color = Color.white;

        if (LevelManager9.Instance != null)
        {
            LevelManager9.Instance.OnTilePainted(oldOwner, -1);
        }

        return true;
    }
}

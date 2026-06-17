using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 10;
    public int columns = 10;
    public float spacing = 1.1f;

    [ContextMenu("Generar Grilla")]
    public void GenerateGrid()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (tilePrefab == null) return;

        Vector3 offset = new Vector3((columns - 1) * spacing / 2f, 0, (rows - 1) * spacing / 2f);

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 spawnPos = transform.position + new Vector3(x * spacing, 0, z * spacing) - offset;
                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                newTile.name = $"Tile_{x}_{z}";
            }
        }
    }
}

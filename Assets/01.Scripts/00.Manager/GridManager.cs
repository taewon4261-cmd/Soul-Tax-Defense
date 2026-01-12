using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 4;
    public float cellSize = 1f;
    public GameObject tilePrefab;

    private void Start()
    {
        InstantiateGrid();
    }

    void InstantiateGrid()
    {
        for (int x = 1; x < width; x++)
        {
            for (float y = -0.5f; y < height; y++)
            {
                Vector3 spawnPosition = new Vector3(x * cellSize, y * cellSize, 0);

                Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
            }

        }
    }

}
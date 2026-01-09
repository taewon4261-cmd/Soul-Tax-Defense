using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 9;
    public int height = 6;
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
            for (int y = -1; y < height; y++)
            {
                Vector3 spawnPosition = new Vector3(x * cellSize, y * cellSize, 0);

                Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
            }

        }
    }

}



//public static GridManager Instance { get; private set; }

//public int width = 9;
//public int height = 5;
//public float cellSize = 1.1f;
//public GameObject tilePrefab;

//private Tile[,] tiles;

//private void Awake()
//{
//    Instance = this;
//}

//private void Start()
//{
//    GenerateGrid();
//}

//void GenerateGrid()
//{
//    tiles = new Tile[width, height];

//    for (int x = 0; x < width; x++)
//    {
//        for (int y = 0; y < height; y++)
//        {
//            Vector3 spawnPosition = new Vector3(x * cellSize, y * cellSize, 0);
//            GameObject go = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);

//            Tile tile = go.GetComponent<Tile>();
//            if (tile == null)
//            {
//                Debug.LogError("tilePrefab에 Tile 컴포넌트가 없습니다!");
//                continue;
//            }

//            tile.gridPos = new Vector2Int(x, y);
//            tiles[x, y] = tile;
//        }
//    }
//}

//public Tile GetTile(Vector2Int pos)
//{
//    if (tiles == null) return null;
//    if (pos.x < 0 || pos.x >= width) return null;
//    if (pos.y < 0 || pos.y >= height) return null;
//    return tiles[pos.x, pos.y];
//}
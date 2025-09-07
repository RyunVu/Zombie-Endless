using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTilemap : MonoBehaviour
{
    [SerializeField] private TileBase[] groundTiles; // Your basic ground tile types
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private int chunkSize = 32;

    private Dictionary<Vector2Int, bool> generatedChunks;

    public Vector3 GetInitialSpawmPosition()
    {
        GeneratedChunk(Vector2Int.zero);

        float centerX = (chunkSize * tileSize) / 2f;
        float centerY = (chunkSize * tileSize) / 2f;

        return new Vector3(centerX, centerY, 0f);
    }

    private void GeneratedChunk(Vector2Int chunkCoord)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int tilePosition = new Vector3Int(chunkCoord.x * chunkSize + x, chunkCoord.y * chunkSize + y, 0);
                TileBase selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];
                tilemap.SetTile(tilePosition, selectedTile);
            }
        }
    }
}
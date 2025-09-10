using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkTilemapSystem : SingletonMonobehaviour<ChunkTilemapSystem>
{
    [Header("Chunk Settings")]
    [SerializeField] private int _chunkSize = 16;
    [SerializeField] private float _tileSize = 1f;
    [SerializeField] private int _renderRadius = 1;

    [Header("Chunk Pool Settings")]
    [SerializeField] private List<ChunkDataSO> _chunkPrefabs = new List<ChunkDataSO>(); // 4-5 different chunk types
    [SerializeField] private int _poolMultiplier = 4; // Multiply each prefab by this amount

    // Internal
    private Dictionary<Vector2Int, GameObject> _activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, ChunkDataSO> _chunkDataMap = new Dictionary<Vector2Int, ChunkDataSO>();
    private OptimizedChunkPool _chunkPool;
    private Vector2Int _currentChunkCoord;
    private Vector2Int _lastChunkCoord;
    [SerializeField] private Grid _grid;

    // Player reference
    private Transform _player;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer().transform;
        InitializeChunkPool();

        // Start from player’s current chunk
        _currentChunkCoord = Vector2Int.zero;
        _lastChunkCoord = _currentChunkCoord;

        // Spawn 3×3 grid
        SpawnChunksAround(_currentChunkCoord);

        _player.position = GetChunkCenterMiddlePoint(_currentChunkCoord);
    }

    private void Update()
    {
        _currentChunkCoord = WorldToChunkCoord(_player.position);

        if (_currentChunkCoord != _lastChunkCoord)
        {
            UpdateChunks(_currentChunkCoord);
            _lastChunkCoord = _currentChunkCoord;
        }
    }


    #region CHUNK POOL INITIALIZATION
    private void InitializeChunkPool()
    {
        if (_chunkPrefabs.Count == 0)
        {
            Debug.LogError("No chunk prefabs assigned in ChunkTilemapSystem.");
            return;
        }

        _chunkPool = new OptimizedChunkPool();
        List<ChunkDataSO> allChunks = new List<ChunkDataSO>();

        foreach (var chunkPrefab in _chunkPrefabs)
        {
            for (int i = 0; i < _poolMultiplier; i++)
            {
                allChunks.Add(chunkPrefab);
            }
        }

        _chunkPool.InitializePool(allChunks);
        Debug.Log($"Initialized chunk pool with {_chunkPool.AvailableCount} chunks.");
    }

    #endregion

    #region CHUNK COORDINATE UTILITIES
    private Vector2Int WorldToChunkCoord(Vector3 worldPos)
    {
        float size = _chunkSize * _tileSize;
        int x = Mathf.FloorToInt((worldPos.x + size / 2f) / size);
        int y = Mathf.FloorToInt((worldPos.y + size / 2f) / size);
        return new Vector2Int(x, y);
    }

    private Vector3 ChunkCoordToWorldPos(Vector2Int chunkCoord)
    {
        float size = _chunkSize * _tileSize;
        return new Vector3(chunkCoord.x * size, chunkCoord.y * size, 0f);
    }

    #endregion

    #region CHUNK SPAWNING / CLEANUP
    private void SpawnChunksAround(Vector2Int center)
    {
        for (int dx = -_renderRadius; dx <= _renderRadius; dx++)
        {
            for (int dy = -_renderRadius; dy <= _renderRadius; dy++)
            {
                Vector2Int coord = center + new Vector2Int(dx, dy);
                if (!_activeChunks.ContainsKey(coord))
                {
                    SpawnChunk(coord);
                }
            }
        }
    }

    private void UpdateChunks(Vector2Int newCenter)
    {
        HashSet<Vector2Int> needed = new HashSet<Vector2Int>();

        // Determine which chunks we need
        for (int dx = -_renderRadius; dx <= _renderRadius; dx++)
        {
            for (int dy = -_renderRadius; dy <= _renderRadius; dy++)
            {
                needed.Add(newCenter + new Vector2Int(dx, dy));
            }
        }

        // Remove chunks that are no longer needed
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var coord in _activeChunks.Keys)
        {
            if (!needed.Contains(coord))
            {
                DespawnChunk(coord);
                toRemove.Add(coord);
            }
        }
        foreach (var coord in toRemove)
        {
            _activeChunks.Remove(coord);
            _chunkDataMap.Remove(coord);
        }

        // Spawn missing chunks
        foreach (var coord in needed)
        {
            if (!_activeChunks.ContainsKey(coord))
            {
                SpawnChunk(coord);
            }
        }
    }

    private void SpawnChunk(Vector2Int chunkCoord)
    {
        ChunkDataSO chunkData = _chunkPool.GetRandomChunk();

        if (chunkData == null)
        {
            Debug.LogWarning($"No chunks available in pool! Cannot spawn chunk at {chunkCoord}");
            return;
        }

        Vector3 worldPos = ChunkCoordToWorldPos(chunkCoord);
        GameObject chunkObject = Instantiate(chunkData.chunkPrefab, worldPos, Quaternion.identity);
        chunkObject.name = $"Chunk_{chunkCoord.x}_{chunkCoord.y}";

         if (_grid != null)
        {
            chunkObject.transform.SetParent(_grid.transform, true); // worldPositionStays = true
        }

        _activeChunks[chunkCoord] = chunkObject;
        _chunkDataMap[chunkCoord] = chunkData;

        Debug.Log($"Spawned chunk at {chunkCoord} | Pool remaining: {_chunkPool.AvailableCount}");
    }

    private void DespawnChunk(Vector2Int chunkCoord)
    {
        if (_activeChunks.TryGetValue(chunkCoord, out GameObject chunkObject))
        {
            // Return chunk data to pool
            if (_chunkDataMap.TryGetValue(chunkCoord, out ChunkDataSO chunkData))
            {
                _chunkPool.ReturnChunk(chunkData);
                Debug.Log($"Returned chunk to pool | Pool count: {_chunkPool.AvailableCount}");
            }

            // Destroy the game object
            Destroy(chunkObject);
            Debug.Log($"Despawned chunk at {chunkCoord}");
        }
    }

    #endregion

    #region PLAYER SPAWN POSITION
    public Vector3 GetInitialSpawnPosition()
    {
        // Return center of current chunk
        return new Vector3(
            _currentChunkCoord.x * _chunkSize * _tileSize + _chunkSize * _tileSize / 2f,
            _currentChunkCoord.y * _chunkSize * _tileSize + _chunkSize * _tileSize / 2f,
            0f
        );
    }
    #endregion

    #region PUBLIC UTILITIES
    public int GetActiveChunkCount() => _activeChunks.Count;
    public int GetPoolAvailableCount() => _chunkPool?.AvailableCount ?? 0;
    public bool IsChunkActive(Vector2Int coord) => _activeChunks.ContainsKey(coord);
    public Vector2Int GetCurrentChunkCoord() => _currentChunkCoord;
    public Vector3 GetChunkCenterMiddlePoint(Vector2Int chunkCoord)
{
    // Directly return the chunk center (no half offset needed)
    return ChunkCoordToWorldPos(chunkCoord);
}

    // Force respawn all chunks (useful for testing)
    [ContextMenu("Respawn All Chunks")]
    public void RespawnAllChunks()
    {
        // Clear all active chunks
        foreach (var chunk in _activeChunks.Values)
        {
            Destroy(chunk);
        }
        _activeChunks.Clear();

        // Return all chunk data to pool
        foreach (var chunkData in _chunkDataMap.Values)
        {
            _chunkPool.ReturnChunk(chunkData);
        }
        _chunkDataMap.Clear();

        // Respawn around current position
        SpawnChunksAround(_currentChunkCoord);
    }
    #endregion

    #region DEBUGGING
    private void OnDrawGizmos()
    {
        if (_player == null) return;

        // Draw active chunks
        foreach (var coord in _activeChunks.Keys)
        {
            Vector3 chunkCenter = ChunkCoordToWorldPos(coord);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(chunkCenter, new Vector3(_chunkSize * _tileSize, _chunkSize * _tileSize, 1));
        }

        // Highlight current player chunk
        Vector2Int playerChunk = WorldToChunkCoord(_player.position);
        Vector3 playerChunkCenter = ChunkCoordToWorldPos(playerChunk);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerChunkCenter, new Vector3(_chunkSize * _tileSize, _chunkSize * _tileSize, 1));

        // Draw render radius boundary
        Gizmos.color = Color.red;
        float renderSize = (_renderRadius * 2 + 1) * _chunkSize * _tileSize;
        Gizmos.DrawWireCube(playerChunkCenter, new Vector3(renderSize, renderSize, 1));
    }

    private void OnGUI()
    {
        if (!Application.isPlaying) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.Label($"Active Chunks: {GetActiveChunkCount()}");
        GUILayout.Label($"Pool Available: {GetPoolAvailableCount()}");
        GUILayout.Label($"Player Chunk: {_currentChunkCoord}");
        
        if (GUILayout.Button("Respawn All Chunks"))
        {
            RespawnAllChunks();
        }
        GUILayout.EndArea();
    }
    #endregion
}

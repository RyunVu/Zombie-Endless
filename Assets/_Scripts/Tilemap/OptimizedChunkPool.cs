using System.Collections.Generic;
using UnityEngine;

public class OptimizedChunkPool
{
    private Dictionary<ChunkDataSO, Queue<GameObject>> _pool = new Dictionary<ChunkDataSO, Queue<GameObject>>();
    private List<ChunkDataSO> _chunks = new List<ChunkDataSO>();

    public void InitializePool(List<ChunkDataSO> chunkPrefabs, int prewarmCount)
{
    foreach (var chunkData in chunkPrefabs)
    {
        if (!_pool.ContainsKey(chunkData))
        {
            _pool[chunkData] = new Queue<GameObject>();
            _chunks.Add(chunkData);
        }

        for (int i = 0; i < prewarmCount; i++)
        {
            GameObject obj = GameObject.Instantiate(chunkData.chunkPrefab);
            obj.SetActive(false);
            _pool[chunkData].Enqueue(obj);
        }
    }
}
    public GameObject GetRandomChunk()
    {
        if (_chunks.Count == 0) return null;

        ChunkDataSO randomData = _chunks[UnityEngine.Random.Range(0, _chunks.Count)];
        return GetChunk(randomData);
    }

    public GameObject GetChunk(ChunkDataSO data)
    {
        if (_pool[data].Count > 0)
        {
            GameObject obj = _pool[data].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Pool exhausted → create a new one
        return GameObject.Instantiate(data.chunkPrefab);
    }

    public void ReturnChunk(ChunkDataSO data, GameObject obj)
    {
        obj.SetActive(false);
        _pool[data].Enqueue(obj);
    }

    public int AvailableCount
    {
        get
        {
            int total = 0;
            foreach (var q in _pool.Values) total += q.Count;
            return total;
        }
    }
}

using System.Collections.Generic;

public class OptimizedChunkPool
{
    private HashSet<ChunkDataSO> _availableChunks;
    private List<ChunkDataSO> _chunksList;

    public OptimizedChunkPool()
    {
        _availableChunks = new HashSet<ChunkDataSO>();
        _chunksList = new List<ChunkDataSO>();
    }

    public void InitializePool(List<ChunkDataSO> chunks)
    {
        foreach (var chunk in chunks)
        {
            _availableChunks.Add(chunk);
            _chunksList.Add(chunk);
        }
    }

    public ChunkDataSO GetRandomChunk()
    {
        if (_chunksList.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, _chunksList.Count);
        ChunkDataSO selectedChunk = _chunksList[randomIndex];

        RemoveChunk(selectedChunk);
        return selectedChunk;
    }

    public void ReturnChunk(ChunkDataSO chunk)
    {
        if (_availableChunks.Add(chunk))
        {
            _chunksList.Add(chunk);
        }
    }

    private void RemoveChunk(ChunkDataSO chunk)
    {
        _availableChunks.Remove(chunk);


        for (int i = 0; i < _chunksList.Count; i++)
        {
            if (_chunksList[i] == chunk)
            {
                _chunksList[i] = _chunksList[_chunksList.Count - 1];
                _chunksList.RemoveAt(i);
                break;
            }
        }
    }

    public int AvailableCount => _chunksList.Count;
    public bool IsEmpty => _chunksList.Count == 0;
    
}
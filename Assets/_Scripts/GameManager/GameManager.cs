using UnityEngine;
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Header GAMEOBJECT REFERENCES
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    #endregion Header GAMEOBJECT REFERENCES

    private PlayerDetailsSO _playerDetails;
    private Player _player;
    private ProceduralTilemap _proceduralTileMap;

    protected override void Awake()
    {
        base.Awake();

        _playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        _proceduralTileMap = FindAnyObjectByType<ProceduralTilemap>();
        InstantiatePlayer();
    }

    void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerDetails.playerPrefab);
        _player = playerGameObject.GetComponent<Player>();
        _player.Initialize(_playerDetails);

        Vector3 spawnPosition = _proceduralTileMap.GetInitialSpawmPosition();
        _player.transform.position = spawnPosition;
    }

    public Player GetPlayer()
    {
        return _player;
    }
}
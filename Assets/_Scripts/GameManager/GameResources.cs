using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources _instance;

    public static GameResources Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GameResources>("GameResources");

            return _instance;
        }
    }

    #region Header PLAYER
    [Space(10)]
    [Header("PLAYER")]
    #endregion Header PLAYER
    [Tooltip("Player details list - populate the list with the playerdetails scriptable objects")]
    public List<PlayerDetailsSO> playerDetailsList;

    [Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
    public CurrentPlayerSO currentPlayer;


    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion

    [Tooltip("The ammo icon prefab")]
    public GameObject ammoIconPrefab;

    public WeaponDetailsSO testWeapon;

}
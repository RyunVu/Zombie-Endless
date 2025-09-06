using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Object/Movement/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header MOVEMENT DETAILS
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    #endregion Header
    #region Tooltip
    [Tooltip("The minimum move speed. The GetMoveSpeed method calculates a random value between the minimum and maximum")]
    #endregion Tooltip
    public float minMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("The maximum move speed. The GetMoveSpeed method calculates a random value between the minimum and maximum")]
    #endregion Tooltip
    public float maxMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("If there is a roll movement- this is the roll speed")]
    #endregion
    public float dashSpeed;                 // for Player 

    #region Tooltip
    [Tooltip("If there is a roll movement- this is the roll speed")]
    #endregion
    public float dashDistance;              // for Player

    #region Tooltip
    [Tooltip("If there is a roll movement- this is the cooldown time between rolls")]
    #endregion
    public float dashCooldownTime;          // for Player
    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed) return minMoveSpeed;
        else return Random.Range(minMoveSpeed, maxMoveSpeed);
    }

}
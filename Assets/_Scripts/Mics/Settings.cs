using UnityEngine;

public static class Settings
{
    #region UNITS
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region ANIMATOR PARAMETERS
    // Animator parameters - Player
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int isDashing = Animator.StringToHash("isDashing");
    public static int use = Animator.StringToHash("use");
    public static float baseSpeedForPlayerAnimations = 8f;
    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    public const string playerWeaponTag = "PlayerWeapon";
    #endregion


    #region FIRING CONTROL
    // the distance from the player to the aim target at which the aim angle used changes from player aim angle (close) to weapon aim angle (far)
    public const float useAimAngleDistance = 3.5f;
    #endregion
}
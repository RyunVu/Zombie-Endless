using UnityEngine;

public static class Settings
{


    #region ANIMATOR PARAMETERS
    // Animator parameters - Player
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int isDashing = Animator.StringToHash("isDashing");
    public static int use = Animator.StringToHash("use");
    public static float baseSpeedForPlayerAnimations = 8f;
    #endregion
}
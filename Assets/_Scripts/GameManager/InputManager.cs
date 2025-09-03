using UnityEngine;

public static class InputManager
{

    public static Vector2 MoveInput
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.moveInput;
            return Vector2.zero;
        }
    }

    public static float GetVerticalRaw()
    {
        if (PlayerInput.Instance != null)
            return PlayerInput.Instance.GetVerticalInput();
        return 0f;
    }

    public static float GetHorizontalRaw()
    {
        if (PlayerInput.Instance != null)
            return PlayerInput.Instance.GetHorizontalInput();
        return 0f;
    }

    public static bool IsMoving()
    {
        if (PlayerInput.Instance != null)
            return PlayerInput.Instance.IsMovingVertically() || PlayerInput.Instance.IsMovingHorizontally();
        return false;
    }
    
}
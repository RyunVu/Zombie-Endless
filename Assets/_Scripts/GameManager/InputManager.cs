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

    public static bool DashWasPressed
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.dashWasPressed;
            return false;
        }
    }

    public static bool AttackWasPressed
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.attackWasPressed;
            return false;
        }
    }

    public static bool AttackIsHeld
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.attackIsHeld;
            return false;
        }
    }

    public static bool AttackWasReleased
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.attackWasReleased;
            return false;
        }
    }

    public static bool ReloadWasPressed
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.reloadWasPressed;
            return false;
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

    public static bool SelectWeapon1WasPressed
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.selectWeapon1WasPressed;
            return false;
        }
    }

    public static bool SelectWeapon2WasPressed
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.selectWeapon2WasPressed;
            return false;
        }
    }

    public static float MouseScrollInput
    {
        get
        {
            if (PlayerInput.Instance != null)
                return PlayerInput.Instance.mouseScrollInput;
            return 0f;
        }
    }
    
}
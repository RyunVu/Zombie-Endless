using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenCursor : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = false;
    }
    
    private void Update() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        transform.position = mousePosition;
    }
}
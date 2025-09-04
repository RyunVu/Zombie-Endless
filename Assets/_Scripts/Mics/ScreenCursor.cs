using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = false;
    }
    
    private void Update() {
        transform.position = Input.mousePosition;
    }
}
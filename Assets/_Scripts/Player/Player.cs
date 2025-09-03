using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerInput playerInput;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }
}

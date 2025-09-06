using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementToPositionEvent))]
public class MovementToPosition : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private MovementToPositionEvent _movementToPositionEvent;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    void OnEnable()
    {
        _movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;
    }

    void OnDisable()
    {
        _movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;
    }

    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent sender, MovementToPositionArgs args)
    {
        MoveRigidbody(args.movePosition, args.currentPosition, args.moveSpeed);
    }

    private void MoveRigidbody(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        Vector3 newPosition = Vector3.Normalize(movePosition - currentPosition);

        _rigidbody2D.MovePosition(_rigidbody2D.position + new Vector2(newPosition.x, newPosition.y) * moveSpeed * Time.fixedDeltaTime);
    }
}
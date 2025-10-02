using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
public class Idle : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private IdleEvent _idleEvent;

    void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _idleEvent = GetComponent<IdleEvent>();
    }

    void OnEnable()
    {
        _idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    void OnDisable()
    {

        _idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        MoveRigidBody();
    }

    private void MoveRigidBody()
    {
        _rigidBody2D.linearVelocity = Vector2.zero;
    }
}
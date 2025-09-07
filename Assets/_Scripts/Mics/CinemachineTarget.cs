using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup _cinemachineTargetGroup;

    [Tooltip("Populate with the CursorTarget GameObject")]
    [SerializeField] private Transform cursorTarget;

    void Awake()
    {
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Start()
    {
        SetCinemachineTargetGroup();
    }

    void SetCinemachineTargetGroup()
    {
        _cinemachineTargetGroup.AddMember(GameManager.Instance.GetPlayer().transform, 1f, 2f);
        _cinemachineTargetGroup.AddMember(cursorTarget, 1f, 1f);
    }

    void Update()
    {
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
    }
}
using Unity.VisualScripting;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private TickTimer _tickTimer;
    
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _fieldOfView;

    private Camera mainCamera;

    private Transform _currentTarget;

    private void OnEnable()
    {
        _tickTimer.OnTick += FindCurrentTarget;
    }

    private void OnDisable()
    {
        _tickTimer.OnTick -= FindCurrentTarget;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    public void Button()
    {
        FindBestTarget();
    }

    private void FindCurrentTarget() // This target is used for enabling particles on the targets themselves 
    {
        Transform newTarget = FindBestTarget();
        
        print(newTarget);
        if (_currentTarget == newTarget) return;

        // Turn off particle on previous target
        if (_currentTarget != null)
        {
            var oldTargetable = _currentTarget.GetComponent<TargetableObject>();
            if (oldTargetable != null)
                oldTargetable.OnTargetLost();
        }

        _currentTarget = newTarget;

        // Turn on particle on new target
        if (_currentTarget != null)
        {
            var newTargetable = _currentTarget.GetComponent<TargetableObject>();
            if (newTargetable != null)
                newTargetable.OnCurrentTarget();
        }
    }

    public Transform FindBestTarget()
    {
        var targets = TargetManager.Instance.GetTargets();
        Debug.Log("Evaluating targets: " + targets.Count);
        
        Transform bestTarget = null;
        float smallestAngle = float.MaxValue;

        foreach (var target in targets)
        {
            Transform targetTransform = target.TargetTransform;
            float distance = Vector3.Distance(mainCamera.transform.position, targetTransform.position);
            if (distance > _maxDistance)
                continue;

            Vector3 directionToTarget = (targetTransform.position - mainCamera.transform.position).normalized;
            float angle = Vector3.Angle(mainCamera.transform.forward, directionToTarget);

            if (angle > _fieldOfView * 0.5f)
                continue;

            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                bestTarget = targetTransform;
            }
        }
        
        return bestTarget;
    }
    
}

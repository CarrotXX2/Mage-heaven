using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _fieldOfView;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Button()
    {
        FindBestTarget();
    }

    public Transform FindBestTarget()
    {
        var targets = TargetManager.Instance.GetTargets();

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

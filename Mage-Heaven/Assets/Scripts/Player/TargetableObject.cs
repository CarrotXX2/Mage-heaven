using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour, ITargetable
{
    public Transform TargetTransform => transform;
    private void OnEnable()
    {
        TargetManager.Instance?.RegisterTarget(this);
    }

    private void OnDisable()
    {
        TargetManager.Instance?.UnregisterTarget(this);
    }
}

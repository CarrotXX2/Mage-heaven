using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour, ITargetable
{
    public Transform TargetTransform => transform;
    [SerializeField] private GameObject particleEffect;
    
    private void OnEnable()
    {
        TargetManager.Instance?.RegisterTarget(this);
    }

    private void OnDisable()
    {
        TargetManager.Instance?.UnregisterTarget(this);
    }

    public void OnCurrentTarget()
    {
        print("Play particle");

        if (particleEffect != null)
        {
            particleEffect?.SetActive(true);
        }
    }

    public void OnTargetLost()
    {
        print("remove particle");

        if (particleEffect != null)
        {
             particleEffect?.SetActive(false);
        }
    }
}

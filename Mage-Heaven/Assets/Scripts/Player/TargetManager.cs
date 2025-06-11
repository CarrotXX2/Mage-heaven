using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    public List<ITargetable> targets = new List<ITargetable>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void RegisterTarget(ITargetable target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
        
        print(target);
    }

    public void UnregisterTarget(ITargetable target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    public List<ITargetable> GetTargets()
    {
        return targets;
    }
}

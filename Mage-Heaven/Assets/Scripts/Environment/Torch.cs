using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour , ITargetable
{
    [SerializeField] private GameObject _torchParticle;
    public Transform TargetTransform { get; }
    
    public void LightTorch()
    {
        _torchParticle.SetActive(true);
    }
}

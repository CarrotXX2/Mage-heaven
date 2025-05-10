using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour, IDamageable, ITargetable
{
    // After hitting the stalactite a few times it falls on the boss
    [SerializeField] private int _health;
    public Transform TargetTransform { get; }
    public void TakeDamage(int damage)
    {
        if (_health <= 0)
        {
            Fall();
        }
    }

    private void Fall()
    {
        
    }
}

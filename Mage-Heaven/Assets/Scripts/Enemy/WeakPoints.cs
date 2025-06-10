using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeakPoints : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private WeakPoints secondWeakPoint;
    [SerializeField] private int health;
    
    public bool isBroken = false;
    
    [SerializeField] private Animation anim; 
    [SerializeField] private Animation alternateAnim;
    
    [SerializeField] private EnemyAnimations _enemyAnim;
    
    public void TakeDamage()
    {
        health --;

        if (health <= 0)
        {
            // play destroy particle 
            enemyHealth.TakeDamage();

            PlayAnimation();
        }
    }
    
    private void PlayAnimation()
    {
        if (secondWeakPoint != null) // If stage has 2 weakpoints play alternate animation if both weakpoints are broken
        {
            if (secondWeakPoint.isBroken)
            {
                _enemyAnim.PlayAnimation(alternateAnim.name);
            }
            else
            {
                _enemyAnim.PlayAnimation(anim.name);
            }
        }
        else
        {
            _enemyAnim.PlayAnimation(anim.name);
        }
    }
}



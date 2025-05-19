using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoints : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyHealth enemyHealth;
    private int health;
    
    private void TakeDamage()
    {
        health --;

        if (health <= 0)
        {
            // play destroy particle 
            enemyHealth.TakeDamage();
          
        }
    }
}

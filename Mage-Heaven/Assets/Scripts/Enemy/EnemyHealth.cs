using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITargetable
{
   [SerializeField] private int _health;
   public Transform TargetTransform { get; }

   private void TakeDamage(int damage)
   {
      _health -= damage;
      
      if (_health <= 0)
      {
         // Next stage or die smth,
         // just wanted some quick structure will work on it later
         
      }
   }
}

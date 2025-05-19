using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   [SerializeField] private int _health;
   [SerializeField] private GameObject _teleportPoint;
   public Transform TargetTransform { get; }
   
   public void TakeDamage()
   {
      _health --;
      
      if (_health <= 0)
      {
         // Stagger animation
         _teleportPoint.SetActive(true);
         // Next stage or die smth,
         // just wanted some quick structure will work on it later

      }
   }
}

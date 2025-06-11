using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   [SerializeField] private int _health;
   [SerializeField] private GameObject _teleportPoint;
   
   [SerializeField] private EnemyAnimations enemyAnim;
   [SerializeField] private AnimationClip animationClip;
   
   public Transform TargetTransform { get; }
   
   public void TakeDamage()
   {
      _health --;
      
      if (_health <= 0)
      {
         // Stagger animation
         enemyAnim.PlayAnimation(animationClip.name);
         
         _teleportPoint.SetActive(true);
         // Next stage or die smth,
         // just wanted some quick structure will work on it later
      }
   }
}

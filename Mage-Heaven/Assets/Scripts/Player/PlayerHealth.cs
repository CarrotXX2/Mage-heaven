using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
     public int[] hitPoints; // Amount of time player is allowed to be hit before he is game over
     private int hitPointsIndex;

     public void OnGameStart()
     {
          hitPointsIndex = Difficulty.instance.GetDifficulty();
     }
     private void TakeDamage() 
     {
          hitPoints[hitPointsIndex]--;
          
          if (hitPoints[hitPointsIndex] == 0)
          {
               // Game over screen or something 
               // Play death scream
          }
          else
          {
               // Play grunt sfx
               // activate vignette 
          }
     }
     public void OnTriggerEnter(Collider other)
     {
          // if(other = boss )
          TakeDamage();
     }
}

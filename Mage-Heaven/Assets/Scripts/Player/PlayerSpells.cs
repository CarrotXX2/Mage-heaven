using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WandStates
{
    ArcaneProjectile,
    Torch,
}
public class PlayerSpells : MonoBehaviour
{
  // Player has two spells that can be cast 
  // A barrier with the left controller
  // Shooting projectiles with the right
  // The wand has two states, one where you can shoot projectiles and one where it functions as a torch 
  
  [Header("General")]
  [SerializeField] private float currentMana;
  [SerializeField] private float maxMana;
  
  [Header("Barrier")]
  [SerializeField] private float barrierCost; // In seconds
  [SerializeField] private GameObject barrier;
  
  private bool isUsinBarrier = false;

  [Header("Wand Spells")] 
  private WandStates wandState;
  
  [SerializeField] private float projectileCost; // In seconds
  [SerializeField] private float torchCost; // In seconds
  
  // I want the wand to cast projectiles through motion to make the game feel more interactive
  // The projectiles will have homing based on what the target was looked at while casting
  
  [SerializeField] private Transform lastProjectileSpawn; // Keep track of the last point that shot a projectile
  [SerializeField] private float projectileTreshold; // Difference in distance needed for next projectile to spawn 
  
  private bool isUsingWand = false;
  private void Update()
  {
      if (isUsinBarrier)
      {
          HandleBarrier();
      }

      if (isUsingWand)
      {
          HandleWand();
      }
  }

  #region Barrier logic

  public void UseBarrier() // When ctx is starting call this 
  {
      isUsinBarrier = true;
      barrier.SetActive(true);
    
      // Barrier activation sound effect 
  }

  public void StopBarrier() // When ctx is cancelled call this 
  {
      isUsinBarrier = false;
      barrier.SetActive(false);
  }

  private void HandleBarrier()
  {
      currentMana -= barrierCost * Time.deltaTime;
          
      if (currentMana <= 0)
      {
          StopBarrier();
      }
  }

  #endregion

  #region WandLogic

  public void SwitchState() // Switches between the two states 
  {
      switch (wandState)
      {
          case WandStates.ArcaneProjectile:
              wandState = WandStates.Torch;
              // change wand visuals
              // Play particle
              // Play sfx
              break;
          
          case WandStates.Torch:
              wandState = WandStates.ArcaneProjectile;
              // change wand visuals
              // Play particle
              // Play sfx
              break;
          
      }
  }

  public void UseWand()
  {
      isUsingWand = true;

      switch (wandState)
      {
          case WandStates.Torch:
              break;
          case WandStates.ArcaneProjectile:
              // do nothing ;P
              break;
      }
  }

  public void StopWand()
  {
      isUsingWand = false;
  }

  private void HandleWand()
  { 
      switch (wandState)
      {
          case WandStates.Torch:
              
              break;
          case WandStates.ArcaneProjectile:
              
              break;
      }
  }
  
  // When wand is being used and player waves with the wand instantiate projectiles 
  private void WandMotionDetection()
  {
      
  }
  #endregion
}

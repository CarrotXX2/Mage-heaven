using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

  [Header("General")] // Mana resource in vr is kinda weird so probably won't use this
  [SerializeField] private Transform _rightHand;
  private Transform target;
  
  [Header("Barrier")]
  [SerializeField] private float barrierCost; // In seconds
  [SerializeField] private GameObject barrier;
  
  private bool isUsinBarrier = false;

  [Header("Wand Spells")] 
  private WandStates wandState;
  
  [SerializeField] private float projectileCost; // In seconds
  [SerializeField] private float torchCost; // In seconds
  
  private bool _litTorch = false;
  
  // I want the wand to cast projectiles through motion to make the game feel more interactive
  // The projectiles will have homing based on what target was looked at while casting
  [SerializeField] private GameObject _projectile;
  [SerializeField] private Transform _projectileSpawnPoint;
  
  [SerializeField] private Transform lastProjectileSpawn; // Keep track of the last point that shot a projectile
  [SerializeField] private float projectileTreshold; // Difference in distance needed for next projectile to spawn 
  
  private bool isUsingWand = false;
  private void Update()
  {
      if (isUsinBarrier)
      {
          // HandleBarrier();
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
              _litTorch = true;
              // Increase fire particle 
              // Torch can now light other torches 
              // Fire sfx loop
              break;
          case WandStates.ArcaneProjectile:
              lastProjectileSpawn = _rightHand;
              ShootProjectile();
              
              // Increase magic aura to notify player you can shoot projectiles 
              // magic aura sfx 
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
              // No need for this, logic is in OnTriggerEnter
              break;
          case WandStates.ArcaneProjectile:
              WandMotionDetection();
              break;
      }
  }
  
  // When wand is being used and player waves with the wand instantiate projectiles 
  private void WandMotionDetection()
  {
     float distance = Vector3.Distance(lastProjectileSpawn.position, _rightHand.position);

     if (distance < projectileTreshold)
     {
         ShootProjectile();
     }
  }

  private void ShootProjectile()
  {
      GameObject projectile = Instantiate(_projectile, _projectileSpawnPoint.position, quaternion.identity);
      
      projectile.GetComponent<Projectile>().target = gameObject.GetComponent<TargetFinder>().FindBestTarget();
      // Hapticfeedback
  }
  
  private void OnTriggerEnter(Collider other)
  {
      // if(other == torch)
      // Light other torches
  }
  #endregion
}

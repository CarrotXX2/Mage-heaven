using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParryDirection
{
    Right,
    Left,
}
public class Parry : MonoBehaviour
{
    // The idea is that the player needs to swipe both hand either to the left or right 
    // That way they can perform a parry and parry the straight punches from the enemy to the side

    #region Variables

    [Header("Parry Detection")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private Vector3 previousLeftHandPos;
    private Vector3 previousRightHandPos;
    
    [SerializeField] private float parrySpeedThreshold; // How fast the player has to swipe their hands
    [SerializeField] private float maxVerticalMovement; // Keep the vertical movement to a minimum

    [Header("Parry")] 
    // The IK target is in front of the player with the pivot point on the player 
    // The target should rotate smoothly based on if the player parried left or right
    // The IK target is only active if the parry is succesfull
    
    [SerializeField] private GameObject ikTarget; 
    [SerializeField] private GameObject ikTargetPivot; 
    
    private GameObject objectToParry;
    
    #endregion
  
    #region Unity Methods
    private void Start()
    {
        previousLeftHandPos = leftHand.position;
        previousRightHandPos = rightHand.position;
    }

    private void Update()
    {
        DetectParry();
    }
    
    #endregion
    
    #region DetectParry
    private void DetectParry()
    {
        Vector3 leftDelta = leftHand.position - previousLeftHandPos;
        Vector3 rightDelta = rightHand.position - previousRightHandPos;

        // Focus mainly on horizontal (X axis) movement
        float leftX = leftDelta.x;
        float rightX = rightDelta.x;

        bool bothMovingRight = leftX > parrySpeedThreshold && rightX > parrySpeedThreshold;
        bool bothMovingLeft = leftX < -parrySpeedThreshold && rightX < -parrySpeedThreshold;

        bool minimalVerticalMovement = Mathf.Abs(leftDelta.y) < maxVerticalMovement && Mathf.Abs(rightDelta.y) < maxVerticalMovement;

        if (minimalVerticalMovement)
        {
            if (bothMovingLeft)
            {
                Debug.Log("Parry left detected!");
                PerformParry(ParryDirection.Left);
            }
            else if (bothMovingRight)
            {
                Debug.Log("Parry right detected!");
                PerformParry(ParryDirection.Right);
            }
        }

        previousLeftHandPos = leftHand.position;
        previousRightHandPos = rightHand.position;
    }
    #endregion
    private void PerformParry(ParryDirection direction)
    {
        if (direction == ParryDirection.Left)
        {
            // Do left parry
            // Rotate the IK pivot to the left 
        }
        else if (direction == ParryDirection.Right)
        {
            // Do right parry
            // Rotate the IK pivot to the right
        }
    }
}

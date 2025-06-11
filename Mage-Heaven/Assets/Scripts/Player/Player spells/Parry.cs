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
    [Header("Enemy")] 
    [SerializeField] private EnemyAI _currentEnemyAI;
    
    [Header("Parry Detection")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    private Vector3 _previousLeftHandPos;
    private Vector3 _previousRightHandPos;
    
    [SerializeField] private float _parrySpeedThreshold; // How fast the player has to swipe their hands
    [SerializeField] private float _maxVerticalMovement; // Keep the vertical movement to a minimum

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
        _previousLeftHandPos = _leftHand.position;
        _previousRightHandPos = _rightHand.position;
    }

    private void Update()
    {
        DetectParry();
    }
    
    #endregion
    
    #region DetectParry
    private void DetectParry()
    {
        Vector3 leftDelta = _leftHand.position - _previousLeftHandPos;
        Vector3 rightDelta = _rightHand.position - _previousRightHandPos;

        // Focus mainly on horizontal (X axis) movement
        float leftX = leftDelta.x;
        float rightX = rightDelta.x;

        bool bothMovingRight = leftX > _parrySpeedThreshold && rightX > _parrySpeedThreshold;
        bool bothMovingLeft = leftX < -_parrySpeedThreshold && rightX < -_parrySpeedThreshold;

        bool minimalVerticalMovement = Mathf.Abs(leftDelta.y) < _maxVerticalMovement && Mathf.Abs(rightDelta.y) < _maxVerticalMovement;

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

        _previousLeftHandPos = _leftHand.position;
        _previousRightHandPos = _rightHand.position;
    }
    #endregion
    private void PerformParry(ParryDirection direction)
    {
       // if (!_currentEnemyAI.isParryAble) return;
        
        if (direction == ParryDirection.Left)
        {
            // Do left parry
            // Move the IK to the left 
        }
        else if (direction == ParryDirection.Right)
        {
            // Do right parry
            // Move the IK to the right
        }
    }
}

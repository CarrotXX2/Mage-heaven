using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint headLook;
    [SerializeField] private float lookSpeed = 5f;
    
    private bool shouldLookAtPlayer;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
   /* private void Update()
    {
        // Smoothly increase/decrease influence
        float targetWeight = shouldLookAtPlayer ? 1f : 0f;
        headLook.weight = Mathf.MoveTowards(headLook.weight, targetWeight, Time.deltaTime * lookSpeed);
    }

    public void LookAtPlayer()
    {
        if (shouldLookAtPlayer)
        {
            shouldLookAtPlayer = false;
        }
        else
        {
            shouldLookAtPlayer = true;
        }
    }*/

    public void PlayAnimation(string animation)
    {
        _animator.SetTrigger(animation);
    }
}

using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Attack Data")]
    [SerializeField] private EnemyAttackData[] attacks;
    private EnemyAttackData _currentAttack;
    
    [Header("Timers")]
    [SerializeField] private float _timeBetweenAttacksMin;
    [SerializeField] private float _timeBetweenAttacksMax;
    
    private float _timeBetweenAttacks;
    private float _attackTimer;

    [Header("Animations")]
    [SerializeField] private Transform _ikHand; 
    [SerializeField] private Transform _ikTarget; // Target for inverse Kinematics 
    [SerializeField] private float _maxIKDistance;
    [SerializeField] private float _ikFadeDuration;
    
    private float _ikFadeTimer;
    
    private bool _isPunching;
    public bool isParryAble;
    
    private float ikWeight;
    private float targetWeight;
    
    [SerializeField] private float transitionSpeed;
    private Animator _animator;
    
    [Header("Particles")]
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        
        if (_attackTimer >= _timeBetweenAttacks)
        {
            PerformAttack();
        }

        if (!_currentAttack.parryAble) return;
        HandleIK();
    }
    
    private void PerformAttack()
    {
        // New attack timer after every attack
        _timeBetweenAttacks = Random.Range(_timeBetweenAttacksMin, _timeBetweenAttacksMax);
        
        EnemyAttackData data = GetRandomWeightedAttack();
        
        _animator.SetTrigger(data.clip.name);
    }
    
    private EnemyAttackData GetRandomWeightedAttack() // Returns an attackData based on the weight of the attacks
    {
        float totalWeight = 0f;

        // Get the total weight of all attacks
        foreach (var attack in attacks)
        {
            totalWeight += attack.weight;
        }

        // Pick a random number between 0 and the total weight
        float randomValue = Random.Range(0, totalWeight);

        // Go through the list and subtract weights until you find the attack
        float cumulativeWeight = 0f;
        foreach (var attack in attacks)
        {
            cumulativeWeight += attack.weight;

            if (randomValue <= cumulativeWeight)
            {
                return attack;
            }
        }

        return null; // Should never happen unless list is empty or weights are 0
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (_currentAttack != null && _currentAttack.parryAble)
        {
            // Enable IK influence
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
            
            _animator.SetIKPosition(AvatarIKGoal.RightHand, _ikTarget.position);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(_ikTarget.position - transform.position));
        }
    }

    #region Inverse Kinematics
    
    private void HandleIK()
    {
        float distance = Vector3.Distance(_ikHand.position, _ikTarget.position);

        // If punching, build up IK weight
        if (_isPunching)
        {
            _ikFadeTimer = _ikFadeDuration; // Reset fade timer
            float target = Mathf.Clamp01(1f - (distance / _maxIKDistance));
            ikWeight = Mathf.MoveTowards(ikWeight, target, transitionSpeed * Time.deltaTime);
        }
        else if (_ikFadeTimer > 0) 
        {
            _ikFadeTimer -= Time.deltaTime;

            // Gradually fade out IK weight so the animation doesn't snap in a weird way
            ikWeight = Mathf.MoveTowards(ikWeight, 0f, transitionSpeed * Time.deltaTime);
        }
        else
        {
            ikWeight = 0f;
        }
    }
    
    // The next 5 functions get called trough the animator for more specific timings
    public void StartPunch() // Turn on for smooth fade in 
    {
        _isPunching = true;
    }

    public void PullBackPunch() // Turn off bool for smooth Fade out 
    {
        _isPunching = false;
    }

    public void IsParryAble() // Call this to enable parry window
    {
        isParryAble = true;
        
    }

    public void StopParryAble() // Call this to disable parry window 
    {
        isParryAble = false;
    }

    public void ResetAttackData() // Call this to set attack data to null
    {
        _currentAttack = null;
    }
    #endregion
}

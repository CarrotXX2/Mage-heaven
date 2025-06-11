using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAI : MonoBehaviour
{
    [Header("Attack Data")] 
    private bool startFight;
    [SerializeField] private EnemyAttackData[] attacks;
    private EnemyAttackData _currentAttack;
    
    [Header("Projectile Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwOrigin; // Hand or spawn point
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float projectileForce;

    private GameObject currentThrownObject;
    
    [Header("Timers")]
    [SerializeField] private float _timeBetweenAttacksMin;
    [SerializeField] private float _timeBetweenAttacksMax;

    private float _timeBetweenAttacks;
    private float _attackTimer;

    [Header("Rigging")]
    [SerializeField] private TwoBoneIKConstraint rightArmIK;
    [SerializeField] private Transform _ikTarget; // IK target to follow
    [SerializeField] private Transform _ikHint;   // Hint to guide bend direction
    [SerializeField] private float _maxIKDistance = 2f;
    [SerializeField] private float _ikFadeDuration = 1f;
    [SerializeField] private float transitionSpeed = 5f;

    private float _ikFadeTimer;
    private bool _isPunching;
    public bool isParryAble;

    [Header("Particles")]
    [SerializeField] private GameObject _parryAura;

    private void Start()
    {
        _timeBetweenAttacks = Random.Range(_timeBetweenAttacksMin, _timeBetweenAttacksMax);
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _timeBetweenAttacks)
        {
            PerformAttack();
        }

        HandleIK();
    }

    private void PerformAttack()
    {
        _timeBetweenAttacks = Random.Range(_timeBetweenAttacksMin, _timeBetweenAttacksMax);
        _attackTimer = 0;
        
        EnemyAttackData data = GetRandomWeightedAttack();
        _currentAttack = data;

        GetComponent<Animator>().SetTrigger(data.clip.name);
        print(data.clip.name);
    }

    private EnemyAttackData GetRandomWeightedAttack()
    {
        float totalWeight = 0f;
        foreach (var attack in attacks)
        {
            totalWeight += attack.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var attack in attacks)
        {
            cumulativeWeight += attack.weight;
            if (randomValue <= cumulativeWeight)
                return attack;
        }

        return null;
    }

    public void GrabRock()
    {
        if (projectilePrefab == null || throwOrigin == null || playerTarget == null)
        {
            Debug.LogWarning("Missing projectilePrefab, throwOrigin, or playerTarget.");
            return;
        }

        currentThrownObject = Instantiate(projectilePrefab, throwOrigin.position, Quaternion.identity);
    }
    public void ThrowProjectile()
    {
        Rigidbody rb = currentThrownObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (playerTarget.position - throwOrigin.position).normalized;
            rb.velocity = direction * projectileForce;
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing a Rigidbody.");
        }
    }
    
    private void HandleIK()
    {
        float distance = Vector3.Distance(rightArmIK.data.tip.position, _ikTarget.position);
        float targetWeight = 0f;

        if (_isPunching)
        {
            _ikFadeTimer = _ikFadeDuration;
            targetWeight = Mathf.Clamp01(1f - (distance / _maxIKDistance));
        }
        else if (_ikFadeTimer > 0)
        {
            _ikFadeTimer -= Time.deltaTime;
            targetWeight = 0f;
        }

        float currentWeight = rightArmIK.weight;
        rightArmIK.weight = Mathf.MoveTowards(currentWeight, targetWeight, transitionSpeed * Time.deltaTime);
    }

    public void Stagger()
    {
        _attackTimer = 0;
        _timeBetweenAttacks = Random.Range(_timeBetweenAttacksMin, _timeBetweenAttacksMax);
    }
    public void StartFight()
    {
        startFight = true;
    }
    
    // Called by animation events
    public void StartPunch() => _isPunching = true;
    public void PullBackPunch() => _isPunching = false;

    public void IsParryAble()
    {
        isParryAble = true;
        _parryAura.SetActive(true);
    }

    public void StopParryAble()
    {
        isParryAble = false;
        _parryAura.SetActive(false);
    }

    public void ResetAttackData() => _currentAttack = null;
}

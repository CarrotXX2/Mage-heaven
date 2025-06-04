using UnityEngine;

public class Projectile : MonoBehaviour
{   
    public Transform target;
    [HideInInspector] public Vector3 inheritedVelocity;

    [SerializeField] private float _homingStrength;
    [SerializeField] private float _maxSpeed;
    
    private Rigidbody _rb;
    private void Start() // Initialize object 
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = inheritedVelocity;
    }

    private void FixedUpdate() // Use FixedUpdate for physics
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 force = directionToTarget * _homingStrength;

        _rb.AddForce(force);

        // Optional: Clamp speed
        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage();
        }
    }
}

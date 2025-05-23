using UnityEngine;

public enum VelocitySpace
{
    Local,
    World
}

[RequireComponent(typeof(ParticleSystem))]
public class ApplyVelocityOnMovement : MonoBehaviour
{
    [Header("Velocity Settings")]
    public Vector3 velocityDirection = new Vector3(0f, 0f, -1f); // default: back
    public float velocityStrength = 1f;
    public VelocitySpace velocitySpace = VelocitySpace.Local;

    [Header("Behavior")]
    public bool onlyWhenMoving = true;
    public float movementThreshold = 0.01f;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private Vector3 lastPosition;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        lastPosition = transform.position;

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        particles = new ParticleSystem.Particle[main.maxParticles];
    }

    void LateUpdate()
    {
        Vector3 movementDelta = transform.position - lastPosition;
        float movementSpeed = movementDelta.magnitude / Time.deltaTime;
        bool shouldApply = !onlyWhenMoving || movementSpeed > movementThreshold;

        if (shouldApply)
        {
            Vector3 finalDirection = (velocitySpace == VelocitySpace.Local)
                ? transform.TransformDirection(velocityDirection)
                : velocityDirection;

            Vector3 appliedVelocity = finalDirection.normalized * velocityStrength * Time.deltaTime;

            int count = ps.GetParticles(particles);
            for (int i = 0; i < count; i++)
            {
                particles[i].velocity += appliedVelocity;
            }

            ps.SetParticles(particles, count);
        }

        lastPosition = transform.position;
    }
}

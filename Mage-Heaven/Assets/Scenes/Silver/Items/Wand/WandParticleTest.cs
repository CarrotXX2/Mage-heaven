using UnityEngine;

public class MagicShooterAndBullet : MonoBehaviour
{
    [Header("Shooter Settings")]
    public bool shouldShoot = false;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform splitPoint;  // Control point voor curve (optioneel)
    public Transform endPoint;
    public float bulletSpeed = 5f;
    public GameObject explosionPrefab;

    private bool isShooting = false;

    void Update()
    {
        if (shouldShoot && !isShooting)
        {
            isShooting = true;
            Shoot();
            shouldShoot = false;
            isShooting = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletPrefab.transform.rotation);

        MagicBulletPart mb = bullet.AddComponent<MagicBulletPart>();

        // Als splitPoint niet ingevuld, gebruiken we firePoint als control point (rechte lijn)
        Vector3 controlPoint = splitPoint != null ? splitPoint.position : firePoint.position;

        mb.Init(firePoint.position, controlPoint, endPoint.position, bulletSpeed, explosionPrefab);
    }

    private class MagicBulletPart : MonoBehaviour
    {
        private Vector3 p0; // start
        private Vector3 p1; // control point
        private Vector3 p2; // end

        private float speed;
        private GameObject explosionPrefab;

        private float t = 0f;
        private bool hasExploded = false;

        public void Init(Vector3 start, Vector3 control, Vector3 end, float spd, GameObject explosion)
        {
            p0 = start;
            p1 = control;
            p2 = end;
            speed = spd;
            explosionPrefab = explosion;

            transform.position = p0;
        }

        void Update()
        {
            if (hasExploded) return;

            // Verhoog t op basis van speed en afstand
            // Om t in [0,1] te houden en snelheid te schalen, gebruiken we een simpele formule:
            float dist = Vector3.Distance(p0, p2);
            t += (speed / dist) * Time.deltaTime;
            t = Mathf.Clamp01(t);

            // Bereken Bézier positie
            Vector3 bezierPos = CalculateQuadraticBezierPoint(t, p0, p1, p2);
            Vector3 moveDir = (bezierPos - transform.position).normalized;

            transform.position = bezierPos;

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-moveDir);
            }

            if (t >= 1f)
            {
                Explode();
            }
        }

        Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return uu * p0 + 2 * u * t * p1 + tt * p2;
        }

        void Explode()
        {
            hasExploded = true;

            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, p2, Quaternion.identity);
                Destroy(explosion, 5f);
            }

            Destroy(gameObject);
        }
    }
}

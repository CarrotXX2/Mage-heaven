using UnityEngine;
using System.Collections.Generic;

public class MagicShooterAndBullet : MonoBehaviour
{
    [Header("Shooter Settings")]
    public bool shouldShoot = false;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform[] controlPoints; // Meerdere control points
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

        List<Vector3> pathPoints = new List<Vector3> { firePoint.position };
        foreach (var cp in controlPoints)
        {
            if (cp != null)
                pathPoints.Add(cp.position);
        }
        pathPoints.Add(endPoint.position);

        mb.Init(pathPoints.ToArray(), bulletSpeed, explosionPrefab);
    }

    private class MagicBulletPart : MonoBehaviour
    {
        private Vector3[] path;
        private float speed;
        private GameObject explosionPrefab;

        private float t = 0f;
        private bool hasExploded = false;

        public void Init(Vector3[] curvePoints, float spd, GameObject explosion)
        {
            path = curvePoints;
            speed = spd;
            explosionPrefab = explosion;

            transform.position = path[0];
        }

        void Update()
        {
            if (hasExploded || path.Length < 2) return;

            float dist = GetTotalCurveLength();
            t += (speed / dist) * Time.deltaTime;
            t = Mathf.Clamp01(t);

            Vector3 curvePos = GetPointOnBezierCurve(path, t);
            Vector3 moveDir = (curvePos - transform.position).normalized;

            transform.position = curvePos;

            if (moveDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(-moveDir);

            if (t >= 1f)
                Explode();
        }

        void Explode()
        {
            hasExploded = true;

            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, path[^1], Quaternion.identity);
                Destroy(explosion, 5f);
            }

            Destroy(gameObject);
        }

        Vector3 GetPointOnBezierCurve(Vector3[] points, float t)
        {
            // De Casteljau’s algorithm voor n-punts Bézier
            List<Vector3> temp = new List<Vector3>(points);

            while (temp.Count > 1)
            {
                for (int i = 0; i < temp.Count - 1; i++)
                {
                    temp[i] = Vector3.Lerp(temp[i], temp[i + 1], t);
                }
                temp.RemoveAt(temp.Count - 1);
            }

            return temp[0];
        }

        float GetTotalCurveLength()
        {
            // Benader de lengte van de curve
            float length = 0f;
            Vector3 prev = path[0];
            for (int i = 1; i <= 30; i++)
            {
                float sampleT = i / 30f;
                Vector3 pos = GetPointOnBezierCurve(path, sampleT);
                length += Vector3.Distance(prev, pos);
                prev = pos;
            }
            return length;
        }
    }
}

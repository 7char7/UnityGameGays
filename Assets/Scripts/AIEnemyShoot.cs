using UnityEngine;

public class AIEnemyShoot : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform firePoint;     // Where bullets spawn
    public GameObject projectile;   // Bullet prefab

    [Header("Settings")]
    public float shootRange = 10f;
    public float shootCooldown = 0.8f;
    public float projectileForce = 30f;

    private float cooldownTimer;

    void Update()
    {
        if (player == null || firePoint == null || projectile == null)
            return;

        cooldownTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootRange && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        // Aim at player
        Vector3 direction = (player.position - firePoint.position).normalized;

        // Create projectile
        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(direction));

        // Add force
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(direction * projectileForce, ForceMode.Impulse);
        }
    }
}
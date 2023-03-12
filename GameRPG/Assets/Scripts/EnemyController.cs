using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 5;
    public GameObject bulletPrefab;
    public Transform bulletPoint;
    public float bulletAttackRate = 1f;
    public float nextBulletAttackTime = 0f;
    public int health = 5;
    public bool canShootBullets = true;

    // Boss movement
    public float maxDistanceFromPlayer = 8f;
    public float minDistanceFromPlayer = 3f;
    public float retreatSpeed = 1f;
    private bool isMovingTowardsPlayer = true;
    private Vector2 retreatDirection;

    private Rigidbody2D rb;
    private Transform player;
    private PlayerController playerController;
    private bool isPlayerAlive = true;

    // Delay after shooting before the boss can shoot again
    public float shootingDelayTime = 2f;
    private bool isShootingDelayed = false;

    // Phase 1
    public float phase1MoveSpeed = 2f;
    public float phase1BulletAttackRate = 1f;
    public float phase1HealthThreshold = 0.5f;

    // Phase 2
    public float phase2MoveSpeed = 3f;
    public float phase2BulletAttackRate = 2f;
    public float phase2HealthThreshold = 0.5f;

    // Phase 3
    public float phase3MoveSpeed = 3f;
    public float phase3BulletAttackRate = 2f;
    public float phase3HealthThreshold = 0.5f;

    // Phase 4
    public float phase4MoveSpeed = 3f;
    public float phase4BulletAttackRate = 2f;
    public float phase4HealthThreshold = 0.5f;

    // Phase 5
    public float phase5MoveSpeed = 3f;
    public float phase5BulletAttackRate = 2f;
    public float phase5HealthThreshold = 0.5f;

    // Current Phase
    private int currentPhase = 1;
    private float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        // Check if the player is still alive
        if (isPlayerAlive && canShootBullets && !isShootingDelayed)
        {
            // Bullet attack
            if (Time.time >= nextBulletAttackTime)
            {
                nextBulletAttackTime = Time.time + 1f / bulletAttackRate;
                Attack();
                isShootingDelayed = true;
                StartCoroutine(ShootingDelay());
            }
        }

        // Check if the boss should transition to a new phase
        if (currentPhase == 1 && currentHealth <= phase1HealthThreshold)
        {
            currentPhase = 2;
            // Transition to phase 2
            if (currentPhase == 2 && currentHealth <= phase2HealthThreshold)
            {
                currentPhase = 3;
                // Transition to phase 3
                if (currentPhase == 3 && currentHealth <= phase3HealthThreshold)
                {
                    currentPhase = 4;
                    // Transition to phase 4
                    if (currentPhase == 4 && currentHealth <= phase4HealthThreshold)
                    {
                        currentPhase = 5;
                        // Transition to phase 5

                    }
                }
            }
        }

    }

    void FixedUpdate()
    {

        // Boss movement
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceFromPlayer > maxDistanceFromPlayer)
        {
            isMovingTowardsPlayer = true;
        }
        else if (distanceFromPlayer < minDistanceFromPlayer)
        {
            isMovingTowardsPlayer = false;
            retreatDirection = (transform.position - player.position).normalized;
        }

        if (isMovingTowardsPlayer)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + retreatDirection * retreatSpeed * Time.fixedDeltaTime);
        }

        // Phase 1
        if (currentPhase == 1)
        {

        }

        // Phase 2
        if (currentPhase == 2)
        {

        }

        // Phase 3
        if (currentPhase == 3)
        {

        }

        // Phase 4
        if (currentPhase == 4)
        {

        }

        // Phase 5
        if (currentPhase == 5)
        {

        }
    }

    void Attack()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.direction = (player.position - bulletPoint.position).normalized;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy taking " + damage + " damage.");
        health -= damage;
        Debug.Log("Enemy health: " + health);
        if (health <= 0)
        {
            health = 0; // Make sure health does not go negative
            Die();
        }
    }

    private void Die()
    {
        // Destroy the enemy
        Destroy(gameObject);
    }

    // Method called when the player dies
    public void PlayerDied()
    {
        // Stop attacking if the player is dead
        isPlayerAlive = false;
        canShootBullets = false;
    }

    IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(shootingDelayTime);
        isShootingDelayed = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 10;
    public GameObject bulletPrefab;
    public Transform bulletPoint;
    public float bulletAttackRate = 1f;
    public float nextBulletAttackTime = 0f;
    public int health = 10;
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

    // Phase control
    public float healthThresholdPhase2 = 0.5f;
    public float healthThresholdPhase3 = 0.2f;

    public int currentPhase = 1;

    // End scene
    public string SceneName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        health = maxHealth;
        canShootBullets = false;
        StartCoroutine(EnableBulletShooting());
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

        // Check for phase transitions
        if (currentPhase == 1)
        {
            // Boss behavior for phase 1
            if (health <= healthThresholdPhase2)
            {
                // Transition to phase 2
                currentPhase = 2;
                moveSpeed = 7f;
                bulletAttackRate = 2f;
                Debug.Log("Transitioning to phase 2");
            }
        }
        else if (currentPhase == 2)
        {
            // Boss behavior for phase 2
            if (health <= healthThresholdPhase3)
            {
                // Transition to phase 3
                currentPhase = 3;
                moveSpeed = 9f;
                bulletAttackRate = 3f;
            }
        }
        else if (currentPhase == 3)
        {
            // Boss behavior for phase 3
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
    }

    IEnumerator EnableBulletShooting()
    {
        yield return new WaitForSeconds(3f);
        canShootBullets = true;
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
        SceneManager.LoadScene(SceneName);
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

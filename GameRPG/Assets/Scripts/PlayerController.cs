using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Health
    public int maxHealth = 10;
    public int currentHealth;

    // Sword
    public GameObject swordPrefab;
    public Transform swordPointUp;
    public Transform swordPointDown;
    public Transform swordPointLeft;
    public Transform swordPointRight;
    public Transform swordPointUpRight;
    public Transform swordPointDownRight;
    public Transform swordPointUpLeft;
    public Transform swordPointDownLeft;
    public float swordAttackRate = 0.5f;
    public float nextSwordAttackTime = 0f;

    // Blink
    public float blinkInterval = 0.1f;
    public float blinkTime = 0.1f;
    public int blinkCount = 10;

    // Invincibility
    public float invincibilityTime = 2f;
    private bool isInvincible = false;

    // Sprite
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Movement
    public float moveSpeed = 5f;
    private float diagonalMoveModifier = 0.75f;
    public float diagonalMoveSpeedMultiplier = 0.7f;
    private Vector2 movement;
    public Vector2 moveDirection;
    public Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Set move direction
        if (movement != Vector2.zero)
        {
            moveDirection = movement.normalized;
        }

        // Sword attack input
        if (Input.GetButtonDown("Fire1") && Time.time >= nextSwordAttackTime)
        {
            nextSwordAttackTime = Time.time + 1f / swordAttackRate;
            Attack();
        }

        Animate();

        // Toggle invincibility
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    isInvincible = !isInvincible;
        //    if (isInvincible)
        //    {
        //        StartCoroutine(BlinkSprite());
        //    }
        //}
    }


    void FixedUpdate()
    {
        // Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

        float speed = moveSpeed;
        if (horizontalInput != 0 && verticalInput != 0)
        {
            speed = moveSpeed * diagonalMoveModifier;
        }
        rb.MovePosition(rb.position + movementDirection * speed * diagonalMoveSpeedMultiplier * Time.fixedDeltaTime);
    }



    void Attack()
    {
        if (moveDirection.x == 0 && moveDirection.y > 0)
        {
            Debug.Log("Attacking up");
            GameObject sword = Instantiate(swordPrefab, swordPointUp.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, 0);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.x == 0 && moveDirection.y < 0)
        {
            Debug.Log("Attacking down");
            GameObject sword = Instantiate(swordPrefab, swordPointDown.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, 180);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.y == 0 && moveDirection.x < 0)
        {
            Debug.Log("Attacking left");
            GameObject sword = Instantiate(swordPrefab, swordPointLeft.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, 90);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.y == 0 && moveDirection.x > 0)
        {
            Debug.Log("Attacking right");
            GameObject sword = Instantiate(swordPrefab, swordPointRight.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, -90);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.x < 0 && moveDirection.y > 0)
        {
            Debug.Log("Attacking up left");
            GameObject sword = Instantiate(swordPrefab, swordPointUpLeft.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, 45);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.x > 0 && moveDirection.y > 0)
        {
            Debug.Log("Attacking up right");
            GameObject sword = Instantiate(swordPrefab, swordPointUpRight.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, -45);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.x < 0 && moveDirection.y < 0)
        {
            Debug.Log("Attacking down left");
            GameObject sword = Instantiate(swordPrefab, swordPointDownLeft.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, 135);
            Destroy(sword, 0.5f);
        }
        else if (moveDirection.x > 0 && moveDirection.y < 0)
        {
            Debug.Log("Attacking down right");
            GameObject sword = Instantiate(swordPrefab, swordPointDownRight.position, Quaternion.identity);
            sword.transform.rotation = Quaternion.Euler(0, 0, -135);
            Destroy(sword, 0.5f);
        }
    }




    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BlinkSprite());
            StartCoroutine(InvincibilityTimer());
        }
    }

    void Die()
    {
        // handle player death
        Destroy(gameObject);
    }

    IEnumerator InvincibilityTimer()
    {
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    IEnumerator BlinkSprite()
    {
        isInvincible = true;
        float endTime = Time.time + invincibilityTime;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
        isInvincible = false;
    }


    void Animate()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 5; // Amount of damage the sword deals

    // Sword points
    public Transform swordPointUp;
    public Transform swordPointDown;
    public Transform swordPointLeft;
    public Transform swordPointRight;

    public float swingDelay = 0.2f; // Delay between sword swings
    public float swingActiveTime = 0.1f; // Time the sword stays active after swinging
    public float swingCooldown = 0.2f; // Cooldown period after swinging before the sword can be used again

    private bool canSwing = true; // Prevent multiple swings in quick succession
    private bool canDealDamage = true; // Prevent multiple hits with a single swing

    private void Update()
    {
        // Rotate the sword based on the direction the player is facing
        if (transform.parent != null)
        {
            Vector3 playerScale = transform.parent.localScale;
            if (playerScale.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }

        // Enable or disable the sword based on whether it can swing or is in the swingActiveTime window
        if (canSwing || Time.time < nextSwingTime + swingActiveTime)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        // Handle the sword cooldown period
        if (!canSwing && Time.time >= nextSwingTime + swingCooldown)
        {
            canSwing = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null && canDealDamage) // Make sure canDealDamage flag is true before applying damage
        {
            enemy.TakeDamage(damage);
            canDealDamage = false; // Set the flag to false to prevent multiple hits with a single swing
        }
    }






    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            canDealDamage = true; // Reset the flag when the sword leaves the enemy's collider
        }
    }

    private float nextSwingTime = 0f;

    // Call this method to swing the sword
    public void Swing()
    {
        if (canSwing && canDealDamage)
        {
            canSwing = false;
            canDealDamage = false; // To prevent multiple hits with a single swing
            nextSwingTime = Time.time + swingDelay;
        }
    }

}

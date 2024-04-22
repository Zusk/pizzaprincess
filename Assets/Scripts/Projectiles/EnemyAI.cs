using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject projectilePrefab; // Projectile prefab to fire
    public float fireCooldown = 3f; // Cooldown duration in seconds
    public float raycastDistance = 10f; // Distance of the raycast
    public AudioSource aggroMusic; // Reference to the AudioSource for the aggro music

    private float cooldownTimer = 0f; // Timer to track cooldown
    public PlayerHealth playerHP;

    void Start()
    {
        // Ensure the music is not playing at start
        aggroMusic.Stop();
    }

    void Update()
    {
        if(playerHP.isKnockedOut == true){
            // Stop the music if the player is no longer detected
            if (aggroMusic.isPlaying)
            {
                aggroMusic.Stop();
            }
            return;
        }
        // Decrease the cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // Perform a raycast forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // Check if the raycast hits the player
            if (hit.collider.CompareTag("Player"))
            {
                // Play aggro music if not already playing
                if (!aggroMusic.isPlaying)
                {
                    aggroMusic.Play();
                }

                // Fire a projectile
                FireProjectile();

                // Reset the cooldown timer
                cooldownTimer = fireCooldown;
            }
            else
            {
                // Stop the music if the player is no longer detected
                if (aggroMusic.isPlaying)
                {
                    aggroMusic.Stop();
                }
            }
        }
        else
        {
            // Stop the music if the player is no longer detected
            if (aggroMusic.isPlaying)
            {
                aggroMusic.Stop();
            }
        }
    }

    void FireProjectile()
    {
        // Instantiate the projectile and set its position and rotation
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}

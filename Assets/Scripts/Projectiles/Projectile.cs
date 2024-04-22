using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 1; // Damage dealt by the projectile
    public GameObject particleGameObject; // Particle system attached to the projectile

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hits the player
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // If the player has a PlayerHealth component, apply damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Handle the particle effect
            HandleParticleEffect();

            // Destroy the projectile
            Destroy(gameObject);
        }
    }

    private void HandleParticleEffect()
    {
        if (particleGameObject != null)
        {
            // Detach the particle system from the projectile
            particleGameObject.transform.parent = null;

            // Stop the particle system from looping
            var particleSystem = particleGameObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.loop = false;
            }

            // Destroy the particle system after a delay
            Destroy(particleGameObject, 3f);
        }
    }
}

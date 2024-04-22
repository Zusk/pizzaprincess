using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public GameObject particleGameObject; // Particle system attached to the projectile
    public GameObject magicalGib; // Magical gib prefab

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Display the tag of the object collided with
        Debug.Log("Projectile hit object with tag: " + other.tag);
        
        // Check if the projectile hits the enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to access the EnemyManager from the collided object
            EnemyManager enemyManager = other.GetComponent<EnemyManager>();

            // If not found on the collided object, check the parent object
            if (enemyManager == null && other.transform.parent != null)
            {
                enemyManager = other.transform.parent.GetComponent<EnemyManager>();
            }

            // Reduce health of the enemy if the EnemyManager component is found
            if (enemyManager != null)
            {
                enemyManager.ReduceHealth(1);
            }

            LegacyAIDisableMethod(other);
            HandleParticleEffect();
            HandleMagicalGib(other);
        }
    }
    //This method encapsulates our old behavior for AI.
    public void LegacyAIDisableMethod (Collider other){
        EnemyAI enemyAI = other.GetComponent<EnemyAI>();
        Obj_LookAt objLookAt = other.GetComponent<Obj_LookAt>();
        AudioSource audioSource = other.GetComponent<AudioSource>();

        // Only start the coroutine if both components are currently enabled
        if (enemyAI != null && objLookAt != null && enemyAI.enabled && objLookAt.enabled)
        {
            // Stop the audio if AudioSource component is present
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            enemyAI.enabled = false;
            objLookAt.enabled = false;

            // Re-enable the scripts after some seconds
            StartCoroutine(ReEnableScripts(enemyAI, objLookAt));
        }
    }

    private IEnumerator ReEnableScripts(EnemyAI enemyAI, Obj_LookAt objLookAt)
    {
        yield return new WaitForSeconds(4);

        if (enemyAI != null)
        {
            enemyAI.enabled = true;
        }

        if (objLookAt != null)
        {
            objLookAt.enabled = true;
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
            Destroy(particleGameObject, 1f);
        }
    }

    private void HandleMagicalGib(Collider enemyCollider)
    {
        Debug.Log("[PlayerProjectile]: Attempting to spawn Gibs");
        if (magicalGib != null)
        {
            // Approximate collision point on the surface of the enemy collider
            Vector3 collisionPoint = enemyCollider.ClosestPoint(transform.position);

            // Spawn the magicalGib at the collision point
            GameObject gibInstance = Instantiate(magicalGib, collisionPoint, Quaternion.identity);

            // Make the gib face the enemy
            gibInstance.transform.LookAt(enemyCollider.transform);

            // Trigger particle emission
            ParticleSystem gibParticles = gibInstance.GetComponent<ParticleSystem>();
            if (gibParticles != null)
            {
                gibParticles.Play();
                Debug.Log("[PlayerProjectile]: Spawned Gibs");
            }

            // Destroy the gib instance after some time if needed
            Destroy(gibInstance, 5f); // Note: Destroy gibInstance, not gibParticles
        }
    }
}

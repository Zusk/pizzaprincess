using UnityEngine;

public class CarBoom : MonoBehaviour
{
    public GameObject particlePrefab; // Assign this in the inspector
    public Vector3 movementVector; // Set this in the inspector
    public float spinSpeed = 360.0f; // Degrees per second, adjust as needed

    private GameObject spawnedParticle;
    private bool init = false;

    void Update()
    {
        if (init)
        {
            // Move and spin while init is true
            transform.Translate(movementVector * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            init = true;
            if (particlePrefab != null && spawnedParticle == null)
            {
                spawnedParticle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}

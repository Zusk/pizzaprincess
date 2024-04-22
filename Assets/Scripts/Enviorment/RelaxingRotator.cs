using UnityEngine;

public class RelaxingRotator : MonoBehaviour
{
    [SerializeField] private Vector3 minRotationSpeed = new Vector3(-50f, -50f, -50f);
    [SerializeField] private Vector3 maxRotationSpeed = new Vector3(50f, 50f, 50f);

    private Vector3 rotationSpeed;
    private Vector3 targetRotationSpeed;

    private void Start()
    {
        // Set a random initial rotation
        transform.rotation = RandomRotation();

        // Initialize rotation speeds
        rotationSpeed = GetRandomRotationSpeed();
        targetRotationSpeed = GetRandomRotationSpeed();
    }

    private void Update()
    {
        // Smoothly interpolate towards the target rotation speed using unscaledDeltaTime
        rotationSpeed = Vector3.Lerp(rotationSpeed, targetRotationSpeed, Time.unscaledDeltaTime * 0.1f);

        // Apply rotation using unscaledDeltaTime
        transform.Rotate(rotationSpeed * Time.unscaledDeltaTime);

        // Randomly change target rotation speed over time
        if (Random.value < 0.01) // Adjust this value for more or less frequent changes
        {
            targetRotationSpeed = GetRandomRotationSpeed();
        }
    }

    private Vector3 GetRandomRotationSpeed()
    {
        // Random speed clamped between minRotationSpeed and maxRotationSpeed for each axis
        return new Vector3(
            Random.Range(minRotationSpeed.x, maxRotationSpeed.x),
            Random.Range(minRotationSpeed.y, maxRotationSpeed.y),
            Random.Range(minRotationSpeed.z, maxRotationSpeed.z)
        );
    }

    private Quaternion RandomRotation()
    {
        // Generate a random rotation
        return Quaternion.Euler(
            Random.Range(0f, 360f), 
            Random.Range(0f, 360f), 
            Random.Range(0f, 360f)
        );
    }
}

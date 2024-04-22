using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius for detecting the player
    private NavMeshAgent agent; // Component for enemy movement and pathfinding
    private Animator animator; // Component for animating the enemy
    private Transform target; // Reference to the targeted player
    private Vector3 lastPosition; // Stores the enemy's position in the last frame

    public float attackDistance = 2.5f; // Distance within which the enemy can attack the player
    private PlayerHealth playerHealth; // Reference to the player's health script for applying damage

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Only initialize components if NavMeshAgent is present
        if (agent != null)
        {
            InitializeComponents(); // Initialize necessary components on start
        }
        else
        {
            ValidateComponent(animator, "Animator"); // Only initialize Animator if NavMeshAgent is missing
        }

        lastPosition = transform.position; // Set initial position for movement calculation
        InvokeRepeating("DetectPlayer", 0f, 0.2f); // Repeatedly check for player detection
    }

    void Update()
    {
        if (agent != null)
        {
            UpdateAnimatorVelocity(); // Update animation based on movement
        }
    }

    private void InitializeComponents()
    {
        // Validate the presence of required components
        ValidateComponent(agent, "NavMeshAgent");
        ValidateComponent(animator, "Animator");
    }

    private bool ValidateComponent(Component component, string componentName)
    {
        // Check if component is missing and log an error
        if (component == null)
        {
            Debug.LogError($"{componentName} component is missing on {gameObject.name}. Disabling script.");
            enabled = false;
            return false;
        }
        return true;
    }

    private void DetectPlayer()
    {
        // Detect and handle player targeting
        if (target == null)
        {
            FindTargetPlayer(); // Search for the player within detection range
        }
        else
        {
            if (agent != null)
            {
                MoveToTarget(); // Approach the targeted player only if NavMeshAgent is available
            }
            CheckForAttackOpportunity(); // Assess if the player is within attack range
        }
    }

    private void FindTargetPlayer()
    {
        // Detect the player within a specified radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                target = hitCollider.transform; // Set the player as the target
                playerHealth = hitCollider.GetComponent<PlayerHealth>(); // Get the PlayerHealth script
                break;
            }
        }
    }

    private void MoveToTarget()
    {
        // Move towards the player if a target is set and NavMeshAgent is present
        if (this.gameObject.activeSelf == true && target != null && agent != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void UpdateAnimatorVelocity()
    {
        // Calculate and update the movement speed for animation
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        animator.SetFloat("Velocity", velocity.magnitude);
        lastPosition = transform.position;
    }

    private void CheckForAttackOpportunity()
    {
        // Check if the player is close enough to attack
        if (target != null && playerHealth != null && playerHealth.health >= 1)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer < attackDistance)
            {
                playerHealth.TakeDamage(1); // Inflict damage on the player
            }
        }
    }

    // Visualize the detection and attack radii in the Unity Editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Detection radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance); // Attack radius
    }
}

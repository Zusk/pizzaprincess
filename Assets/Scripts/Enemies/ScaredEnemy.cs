using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ScaredEnemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the object that the enemy is scared of.")]
    private Transform threatObject;

    [SerializeField]
    [Tooltip("The minimum duration for which the enemy remains in a scared state before considering returning to idle.")]
    private float minimumScaredDuration = 2f;

    [Header("Distance Settings")]
    [SerializeField]
    [Tooltip("The distance the enemy tries to maintain when fleeing.")]
    private float fleeDistance = 20f;

    [SerializeField]
    [Tooltip("The distance within which the enemy considers itself unsafe and starts fleeing.")]
    private float safeDistance = 8f;

    [Header("Idle Walk Settings")]
    [SerializeField]
    [Tooltip("The distance the enemy will walk when idling.")]
    private float idleWalkDistance = 1f;

    [SerializeField]
    [Tooltip("The minimum time between idle walks.")]
    private float idleTimeMin = 4f;

    [SerializeField]
    [Tooltip("The maximum time between idle walks.")]
    private float idleTimeMax = 8f;

    [SerializeField]
    [Tooltip("If true and the threat object is not assigned, the player will be used as the default threat.")]
    private bool DefaultThreatIsPlayer = true;

    // Other fields
    private NavMeshAgent agent; // The NavMeshAgent component for navigation.
    private Animator animator; // The Animator component for controlling animations.
    private bool isFleeing = false; // Flag to determine if the enemy is currently fleeing.
    private Coroutine currentBehaviorCoroutine; // Reference to the currently running behavior coroutine.
    private float lastScaredTime; // The last time the enemy was scared.

    void Start()
    {
        // Initialize the NavMeshAgent and Animator components.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Error handling if components are missing or not assigned.
        if (agent == null)
        {
            Debug.LogError("[Scared Enemy]: NavMeshAgent component is missing.");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("[Scared Enemy]: Animator component is missing.");
            return;
        }

        // If the threat object is not assigned and DefaultThreatIsPlayer is true, search for the player.
        if (threatObject == null && DefaultThreatIsPlayer)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                threatObject = playerObject.transform;
            }
            else
            {
                Debug.LogError("[Scared Enemy]: Threat object is not defined in the inspector, and the player could not be found.");
                return;
            }
        }
        else if (threatObject == null)
        {
            Debug.LogError("[Scared Enemy]: Threat object is not defined in the inspector.");
            return;
        }

        // Start the idle behavior coroutine.
        currentBehaviorCoroutine = StartCoroutine(IdleWalkBehavior());
    }


    void Update()
    {
        // Update the animation speed based on the agent's velocity.
        if (animator != null && agent != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        // Check the distance to the threat and update behavior accordingly.
        float distanceToThreat = Vector3.Distance(transform.position, threatObject.position);
        if (distanceToThreat <= safeDistance && !isFleeing)
        {
            // If the threat is too close and the enemy is not already fleeing, start the flee behavior.
            if (currentBehaviorCoroutine != null)
            {
                StopCoroutine(currentBehaviorCoroutine);
            }
            isFleeing = true;
            lastScaredTime = Time.time;
            currentBehaviorCoroutine = StartCoroutine(FleeBehavior());
        }
        else if (distanceToThreat > safeDistance && isFleeing && Time.time - lastScaredTime > minimumScaredDuration)
        {
            // If the enemy is far enough from the threat and has been scared for a minimum duration, revert to idle behavior.
            if (currentBehaviorCoroutine != null)
            {
                StopCoroutine(currentBehaviorCoroutine);
            }
            isFleeing = false;
            currentBehaviorCoroutine = StartCoroutine(IdleWalkBehavior());
        }
    }

    IEnumerator FleeBehavior()
    {
        // Coroutine for flee behavior.
        while (isFleeing)
        {
            //Initializes a few variables for our coroutine
            Vector3 bestFleePoint = Vector3.zero;
            float maxDistance = 0;
            bool validPointFound = false;

            // Check multiple directions to find the best flee point.
            for (int i = 0; i < 360; i += 36) // Check in increments of 36 degrees.
            {
                //This gets our current 'rotation slice' we are checking.
                Quaternion rotation = Quaternion.Euler(0, i, 0);
                //A direction ahead from the rotation angle.
                Vector3 direction = rotation * Vector3.forward;
                //Derive a potential flee point from the direction, our current position, and the fleedistance constant.
                Vector3 potentialFleePoint = transform.position + direction * fleeDistance;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(potentialFleePoint, out hit, fleeDistance, NavMesh.AllAreas))
                {
                    float distanceToThreat = Vector3.Distance(hit.position, threatObject.position);
                    if (distanceToThreat > maxDistance && IsPathClear(hit.position) && !DoesPathCrossThreat(hit.position))
                    {
                        maxDistance = distanceToThreat;
                        bestFleePoint = hit.position;
                        validPointFound = true;
                    }
                }
            }

            // Set the destination to the best flee point found.
            if (validPointFound)
            {
                agent.SetDestination(bestFleePoint);
            }
            else
            {
                Debug.LogWarning("[Scared Enemy]: Unable to find a valid flee position in any direction.");
            }

            yield return new WaitForSeconds(1f); // Wait for a bit before recalculating the flee path.
        }
    }

    bool IsPathClear(Vector3 destination)
    {
        // Checks if there is a clear path to the given destination.
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }

    bool DoesPathCrossThreat(Vector3 destination)
    {
        // Checks if the path to the given destination crosses the threat's position.
        Vector3 directionToDestination = destination - transform.position;
        Ray ray = new Ray(transform.position, directionToDestination.normalized);

        // Define a radius around the threat to check for intersection.
        float threatRadius = 1.0f;
        return Physics.SphereCast(ray, threatRadius, directionToDestination.magnitude, LayerMask.GetMask("Threat"));
    }

    IEnumerator IdleWalkBehavior()
    {
        // Coroutine for idle walking behavior.
        while (!isFleeing)
        {
            Vector3 randomDirection = Random.insideUnitSphere * idleWalkDistance;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, idleWalkDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("[Scared Enemy]: Unable to find a valid idle walk position.");
            }

            // Wait for a random duration within the specified range before the next idle walk.
            yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        }
    }
}

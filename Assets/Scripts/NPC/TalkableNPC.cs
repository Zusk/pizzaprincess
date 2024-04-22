using UnityEngine;
using System.Collections.Generic; // For using List

public class TalkableNPC : MonoBehaviour
{
    public Transform target; // The target to look at
    public float height = 5f; // The height of the up/down motion
    public float speed = 1f; // Speed of the motion
    public GameObject player; // Reference to the player GameObject
    public List<ButtonType> startingConversation; // List of ButtonTypes for the starting conversation


    private float startLocalY;
    private float endLocalY;
    private float timer;
    private bool goingUp = true;
    private Renderer renderer; // Renderer to control visibility

    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        // Error checking
        if (target == null)
        {
            Debug.LogError("[Talkable NPC]: Target is not assigned in the inspector.");
        }
        if (player == null)
        {
            Debug.LogError("[Talkable NPC]: Player is not assigned in the inspector.");
        }
        if (height <= 0)
        {
            Debug.LogError("[Talkable NPC]: Height must be greater than 0.");
        }
        if (speed <= 0)
        {
            Debug.LogError("[Talkable NPC]: Speed must be greater than 0.");
        }

        startLocalY = transform.localPosition.y;
        endLocalY = startLocalY + height;

        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("[Talkable NPC]: Renderer component missing on the GameObject.");
        }
        else
        {
            renderer.enabled = false; // Initially hide the NPC
        }
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("[Talkable NPC]: DialogueManager not found in the scene.");
        }
    }

    void Update()
    {
        if (renderer != null && renderer.enabled)
        {
            // Look at the target
            if (target != null)
            {
                transform.LookAt(target);
            }

            // Calculate the next local Y position using SmoothStep for smoother motion
            float newLocalY = Mathf.SmoothStep(startLocalY, endLocalY, timer);

            // Update the local position
            transform.localPosition = new Vector3(transform.localPosition.x, newLocalY, transform.localPosition.z);

            // Update the timer
            if (goingUp)
            {
                timer += Time.deltaTime * speed;
                if (timer > 1f)
                {
                    timer = 1f;
                    goingUp = false;
                }
            }
            else
            {
                timer -= Time.deltaTime * speed;
                if (timer < 0f)
                {
                    timer = 0f;
                    goingUp = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
            Debug.Log("[Talkable NPC]: Player entered the trigger at position: " + other.transform.position);
            if (renderer != null)
            {
                renderer.enabled = true; // Show the NPC
            }
            else
            {
                Debug.LogError("[Talkable NPC]: Renderer component missing on the GameObject.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            Debug.Log("[Talkable NPC]: Player left the trigger at position: " + other.transform.position);
            if (renderer != null)
            {
                renderer.enabled = false; // Hide the NPC
            }
            else
            {
                Debug.LogError("[Talkable NPC]: Renderer component missing on the GameObject.");
            }
        }
    }
}

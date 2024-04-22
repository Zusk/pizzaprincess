using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ENTERED LEVEL EXIT!!!");
            DialogueManager dialogueManager = other.GetComponent<DialogueManager>();
            if (dialogueManager != null)
            {
                dialogueManager.LevelExit();
            }
        }
    }
}

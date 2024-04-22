using UnityEngine;
using System.Collections.Generic; // Needed for using List
using System.Linq;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public List<MonoBehaviour> scriptsToDisable; // Assign your scripts here in the inspector
    public GameObject dialogueObject; // Assign your dialogue GameObject here in the inspector

    private bool isDialogueActive = false;
    public static bool inDialogue = false;

    public float interactionRadius = 0.1f; // Interaction radius for detecting NPCs
    public bool disableEInput = false;
    public List<ButtonType> endGameDialogue; //What to display when we end the game.
    public GameObject endGameFlair;

    void Awake(){
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called every time a scene is loaded
        inDialogue = false;
        disableEInput = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && disableEInput == false)
        {
            TryStartDialogue();
        }
    }
    public void LevelExit()
    {
        disableEInput = true;
        endGameFlair.SetActive(true);

        // Convert the List<ButtonType> to object[]
        object[] dialogueItems = endGameDialogue.Cast<object>().ToArray();

        EnterDialogue(dialogueItems);
    }

    private void TryStartDialogue()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);
        List<TalkableNPC> npcsInRange = new List<TalkableNPC>();

        foreach (var hitCollider in hitColliders)
        {
            TalkableNPC npc = hitCollider.GetComponent<TalkableNPC>();
            if (npc != null)
            {
                npcsInRange.Add(npc);
            }
        }

        if (npcsInRange.Count > 0)
        {
            int randomIndex = Random.Range(0, npcsInRange.Count);
            TalkableNPC selectedNPC = npcsInRange[randomIndex];

            if (selectedNPC.startingConversation.Count > 0)
            {
                object[] conversationItems = new object[selectedNPC.startingConversation.Count];
                for (int i = 0; i < selectedNPC.startingConversation.Count; i++)
                {
                    conversationItems[i] = selectedNPC.startingConversation[i];
                }

                EnterDialogue(conversationItems);
            }
        }
    }

    public void EnterDialogue(object[] items)
    {
        if(PauseManager.isPaused){
            return;
        }
        ToggleDialogue();

        // Assuming ButtonManager is part of the dialogueObject
        if (dialogueObject != null)
        {
            ButtonManager buttonManager = dialogueObject.GetComponent<ButtonManager>();
            if (buttonManager != null)
            {
                buttonManager.BuildButtonMenu(items);
            }
        }
    }

    public void ToggleDialogue(object[] items = null)
    {
        Debug.Log("ToggleDialogue called");
        isDialogueActive = !isDialogueActive;
        inDialogue = isDialogueActive;

        Cursor.lockState = isDialogueActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isDialogueActive;

        if (scriptsToDisable != null)
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (script != null)
                {
                    script.enabled = !isDialogueActive;
                }
            }
        }

        if (dialogueObject != null)
        {
            dialogueObject.SetActive(isDialogueActive);
            if (isDialogueActive && items != null)
            {
                ButtonManager buttonManager = dialogueObject.GetComponent<ButtonManager>();
                if (buttonManager != null)
                {
                    buttonManager.BuildButtonMenu(items);
                }
            }
        }

        Time.timeScale = isDialogueActive ? 0 : 1;
    }


}

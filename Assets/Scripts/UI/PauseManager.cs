using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public GameObject levelScene_WeaponCamera;
    public GameObject levelScene_Weapon;
    public GameObject levelScene_OldCanvas;
    public MonoBehaviour levelScene_FirstPersonController;
    public static bool isPaused = false;

    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    // Initialize and configure the game object
    void Awake(){
        Debug.Log("[Pause Manager]: Initializing and setting cursor state.");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Handle input for pausing and resuming
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Debug.Log("[Pause Manager]: Escape key pressed. Resuming game.");
                ResumeGame();
            }
            else
            {
                if(DialogueManager.inDialogue){
                    return;
                }
                Debug.Log("[Pause Manager]: Escape key pressed. Pausing game.");
                PauseGame();
            }
        }
    }

    // Pause the game and handle related changes
    void PauseGame()
    {
        Time.timeScale = 0;
        
        if(levelScene_Weapon.activeInHierarchy)
        {
            levelScene_WeaponCamera.SetActive(false);
        }
        levelScene_OldCanvas.SetActive(false);
        isPaused = true;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

        if (levelScene_FirstPersonController != null)
        {
            levelScene_FirstPersonController.enabled = false;
            Debug.Log("[Pause Manager]: FirstPersonController disabled successfully.");
        }
        else
        {
            Debug.LogWarning("[Pause Manager]: FirstPersonController is missing!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause all active and playing AudioSources
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in audioSources)
        {
            if (source.isPlaying && source.gameObject.activeInHierarchy)
            {
                source.Pause();
                activeAudioSources.Add(source);
            }
        }
        Debug.Log($"[Pause Manager]: Paused {activeAudioSources.Count} audio sources.");
    }

    // Resume the game and revert changes
    public void ResumeGame()
    {
        Time.timeScale = 1;
        if(levelScene_Weapon.activeInHierarchy)
        {
            levelScene_WeaponCamera.SetActive(true);
            Debug.Log("[Pause Manager]: Weapon Camera reactivated.");
        }

        levelScene_OldCanvas.SetActive(true);
        isPaused = false;

        SceneManager.UnloadSceneAsync("MainMenu");

        if (levelScene_FirstPersonController != null)
        {
            levelScene_FirstPersonController.enabled = true;
            Debug.Log("[Pause Manager]: FirstPersonController enabled successfully.");
        }
        else
        {
            Debug.LogWarning("[Pause Manager]: FirstPersonController is missing!");
        }

        PlayerHealth pHealth = levelScene_FirstPersonController.gameObject.GetComponent<PlayerHealth>();
        pHealth.healthDisplay.UpdateDisplay(pHealth.health, pHealth.armor, pHealth.ammo);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Resume all paused AudioSources
        foreach (var source in activeAudioSources)
        {
            if (source != null) // Check if the source hasn't been destroyed
            {
                source.UnPause();
            }
        }
        Debug.Log($"[Pause Manager]: Resumed {activeAudioSources.Count} audio sources.");
        activeAudioSources.Clear();
    }
}

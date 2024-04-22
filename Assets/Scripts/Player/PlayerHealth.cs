//This handles health and armor pickups, also enforces minimum and maximum values.
using UnityEngine;

using UnityEngine.SceneManagement; // Added for scene management

using System.Collections; // This is the namespace for IEnumerator
using UnityStandardAssets.Characters.FirstPerson; //Was having trouble storing the FirstPersonController script as a object variable, using the namespace here helps.

public class PlayerHealth : MonoBehaviour
{
    // Constants for default max values of health and armor
    public const int DefaultMaxHealth = 6;
    public const int DefaultMaxArmor = DefaultMaxHealth;

    // Dynamic variables for current max health and armor
    public int currentMaxHealth;
    public int currentMaxArmor;

    // Variables for health and armor
    public int health = 7;
    public int armor = 3;
    public int ammo = 0;

    // Reference to the HealthDisplay script attached to the UI
    public HealthDisplay healthDisplay;

    private AudioSource audioSource;
    public AudioClip knockedOutClip;
    public bool isKnockedOut = false;
    public GameObject knockOutScreen;


    void Awake()
    {
        // Get the AudioSource component from the player
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Set current max values based on difficulty
        int difficulty = PlayerPrefs.GetInt("PP_Difficulty", 1); // Default to medium difficulty
        switch (difficulty)
        {
            case 0: // Easy
                currentMaxHealth = DefaultMaxHealth + 3;
                currentMaxArmor = DefaultMaxArmor + 3;
                break;
            case 2: // Hard
                currentMaxHealth = DefaultMaxHealth - 3;
                currentMaxArmor = DefaultMaxArmor - 3;
                break;
            default: // Normal or any other case
                currentMaxHealth = DefaultMaxHealth;
                currentMaxArmor = DefaultMaxArmor;
                break;
        }

        // Ensure health and armor are within the current limits
        health = Mathf.Clamp(health, 0, currentMaxHealth);
        armor = Mathf.Clamp(armor, 0, currentMaxArmor);

        // Update the display
        if (healthDisplay != null)
        {
            healthDisplay.UpdateDisplay(health, armor, ammo);
        }
        StartCoroutine(ReplenishAmmoCoroutine());
    }

    IEnumerator ReplenishAmmoCoroutine()
    {
        while (true)
        {
            int difficulty = PlayerPrefs.GetInt("PP_Difficulty", 1);
            if (ammo < 6 && difficulty != 2)
            {
                ammo++;
            }
            if (healthDisplay != null)
            {
                healthDisplay.UpdateDisplay(health, armor, ammo);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (PlayerPrefs.GetInt("PP_GodMode", 0) == 1)
        {
            return; // No damage is taken in God Mode
        }
        // Reduce armor first, then health
        if (armor > 0)
        {
            armor = Mathf.Max(armor - damage, 0);
        }
        else
        {
            health = Mathf.Max(health - damage, 0);
        }

        // Update the UI display
        if (healthDisplay != null)
        {
            healthDisplay.UpdateDisplay(health, armor, ammo);
        }
    }

    public void PickUpArmor(int armorAmount)
    {
        // Increase armor value, ensuring it does not exceed the maximum
        armor = Mathf.Min(armor + armorAmount, currentMaxArmor);

        // Update the UI display
        if (healthDisplay != null)
        {
            healthDisplay.UpdateDisplay(health, armor, ammo);
        }
    }

    // Method to increase health
    public void IncreaseHealth(int healthAmount)
    {
        // Increase health value, ensuring it does not exceed the maximum
        health = Mathf.Min(health + healthAmount, currentMaxHealth);

        // Update the UI display
        if (healthDisplay != null)
        {
            healthDisplay.UpdateDisplay(health, armor, ammo);
        }
    }

    void Update()
    {
        // Check if the player's health has reached zero
        if (health <= 0 && !isKnockedOut)
        {
            PlayerKnockedOut();
        }
    }

    private void PlayerKnockedOut()
    {
        //Do not knock out the player if the game is paused.
        if(PauseManager.isPaused){
            return;
        }
        isKnockedOut = true;
        knockOutScreen.SetActive(true);
        healthDisplay.transform.gameObject.SetActive(false);
        // Play the 'KnockedOut' audio clip
        if (audioSource != null && knockedOutClip != null)
        {
            audioSource.PlayOneShot(knockedOutClip);
        }

        // Disable the FirstPersonController script
        var firstPersonController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;
        }

        // Wait some seconds and reload the scene
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        //This whole death scene is inspired by doom. Where the screen goes red, your character's view lowers to symbolize laying down, and you eventually reload.
        float totalDuration = 8f;  // Total duration of the coroutine
        float shrinkDuration = 0.6f;  // Duration of the shrinking process

        // Get the current scale of the player
        Vector3 originalScale = transform.localScale;

        // Define the target scale (height reduced to half)
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

        // Record the start time of the shrinking process
        float startTime = Time.time;

        while (Time.time - startTime < shrinkDuration)
        {
            // Calculate the fraction of the shrink duration that has passed
            float fraction = (Time.time - startTime) / shrinkDuration;

            // Interpolate the scale of the player based on the time fraction
            transform.localScale = Vector3.Lerp(originalScale, targetScale, fraction);

            // Wait until the next frame before continuing the loop
            yield return null;
        }

        // Ensure the player is set to the final scale after the shrinking process
        transform.localScale = targetScale;

        // Wait for the remaining time of the total duration
        yield return new WaitForSeconds(totalDuration - shrinkDuration);

        // Finally, reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



}

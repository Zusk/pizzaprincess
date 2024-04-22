using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int increaseHealth = 0; // Amount of health this pickup provides
    public int increaseArmor = 0;  // Amount of armor this pickup provides
    public int increaseAmmo = 0;   // Amount of ammo this pickup provides
    public bool isWeaponPickup = false; // Whether this is a weapon pickup
    public AudioClip pickupSound;  // Audio clip to play on pickup
    public GameObject onPickup_EnableObject;  // GameObject to enable upon pickup
    public GameObject onPickup_DisableObject; // GameObject to disable upon pickup

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            AudioSource playerAudioSource = other.GetComponent<AudioSource>();
            if (playerHealth == null || playerAudioSource == null) return;

            bool pickedUp = false;

            // Increase health if applicable and needed
            if (increaseHealth > 0 && playerHealth.health < playerHealth.currentMaxHealth)
            {
                playerHealth.IncreaseHealth(increaseHealth);
                pickedUp = true;
            }

            // Increase armor if applicable and needed
            if (increaseArmor > 0 && playerHealth.armor < playerHealth.currentMaxArmor)
            {
                playerHealth.PickUpArmor(increaseArmor);
                pickedUp = true;
            }

            // Increase ammo if applicable and needed
            if (increaseAmmo > 0)
            {
                playerHealth.ammo++;
                playerHealth.healthDisplay.UpdateDisplay(playerHealth.health, playerHealth.armor, playerHealth.ammo);
                pickedUp = true;
            }

            // Handle weapon pickup
            if (isWeaponPickup)
            {
                playerHealth.ammo += 5;
                playerHealth.GetComponent<WeaponHandler>().weaponHandlerObject.SetActive(true);
                playerHealth.healthDisplay.UpdateDisplay(playerHealth.health, playerHealth.armor, playerHealth.ammo);
                pickedUp = true;
            }

            if (pickedUp)
            {
                // Play audio clip if the player has an AudioSource component and a clip is set
                if (pickupSound != null)
                {
                    playerAudioSource.PlayOneShot(pickupSound);
                }

                // Enable the specified object if it is set
                if (onPickup_EnableObject != null)
                {
                    onPickup_EnableObject.SetActive(true);
                }

                // Disable the specified object if it is set
                if (onPickup_DisableObject != null)
                {
                    onPickup_DisableObject.SetActive(false);
                }

                Destroy(this.gameObject); // Destroy the pickup object
            }
        }
    }
}

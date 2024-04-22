//This script handles updating the textmeshpro UI element.
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI healthText;

    // Method to update the health and armor display
    public void UpdateDisplay(int health, int armor, int ammo)
    {
        string displayText = "";

        // Add blue asterisks for armor
        for (int i = 0; i < armor; i++)
        {
            displayText += "<color=#ADD8E6>*</color>"; 
        }

        // Add red asterisks for health
        for (int i = armor; i < health; i++)
        {
            displayText += "<color=#FF0000>*</color>"; 
        }
        // New line for ammo
        displayText += "\n";

        // Add green asterisks for ammo
        for (int i = 0; i < ammo; i++)
        {
            displayText += "<color=#00FF00>*</color>"; 
        }
        int difficulty = PlayerPrefs.GetInt("PP_Difficulty", 1);
        // Add one blue asterisk if ammo is below 6
        if (ammo < 6 && difficulty != 2)
        {
            displayText += "<color=#0000FF>*</color>";
        }

        // Update the text in the UI
        healthText.text = displayText;
    }
}

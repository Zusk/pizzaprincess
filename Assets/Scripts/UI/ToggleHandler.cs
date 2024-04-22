using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleHandler : MonoBehaviour, IPointerEnterHandler
{
    public ButtonManager buttonManager;
    public TextMeshProUGUI textComponent;
    private bool ignoreValueChanges = true;

    private void Start()
    {
        // Get the Toggle component attached to the same GameObject.
        Toggle toggle = GetComponent<Toggle>();
        ignoreValueChanges = true;

        if (toggle != null)
        {
            // Extract the prefix (e.g., "PP_Graphics") and the value (e.g., "2") from the GameObject's name.
            string[] nameParts = gameObject.name.Split('_');
            if (nameParts.Length < 3)
            {
                Debug.LogError("Invalid toggle name format.");
                return;
            }

            string prefName = nameParts[0] + "_" + nameParts[1]; // e.g., "PP_Graphics"
            int expectedValue = int.Parse(nameParts[2]); // e.g., 2

            // Use a switch statement to handle different PlayerPrefs names.
            int toggleState = -1;
            switch (prefName)
            {
                case "PP_Graphics":
                    toggleState = PlayerPrefs.GetInt("PP_Graphics", -1);
                    break;
                case "PP_Difficulty":
                    toggleState = PlayerPrefs.GetInt("PP_Difficulty", -1);
                    break;
                case "PP_GodMode":
                    toggleState = PlayerPrefs.GetInt("PP_GodMode", -1);
                    break;
                default:
                    Debug.LogError("Unrecognized PlayerPrefs name: " + prefName);
                    return;
            }

            // Set the toggle's state based on PlayerPrefs.
            toggle.isOn = (toggleState == expectedValue);
            // Re-enable the Toggle component after setup.
            ignoreValueChanges = false;
            Debug.Log($"[ToggleHandler] {gameObject.name}: PlayerPrefs '{prefName}' is {toggleState}, Toggle set to {(toggleState == expectedValue)}");
        }
        else
        {
            Debug.LogError("Toggle component not found on the GameObject.");
        }
    }
    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            if(ignoreValueChanges){
                return;
            }
            // Split the GameObject's name to extract PlayerPrefs key and value
            string[] nameParts = gameObject.name.Split('_');
            if (nameParts.Length < 3)
            {
                Debug.LogError("[ToggleHandler] Invalid toggle name format. Expected format 'PP_Name_Value'. GameObject Name: " + gameObject.name);
                return;
            }

            // Construct the PlayerPrefs key from the first two parts of the name
            string prefName = nameParts[0] + "_" + nameParts[1]; // e.g., "PP_Graphics"
            int valueToSet = int.Parse(nameParts[2]); // Use the last part as value

            // Check if the constructed PlayerPrefs key is valid
            if (prefName == "PP_Graphics" || prefName == "PP_Difficulty" || prefName == "PP_GodMode")
            {
                PlayerPrefs.SetInt(prefName, valueToSet);
                PlayerPrefs.Save();

                Debug.Log($"[ToggleHandler] PlayerPrefs updated: {prefName} set to {valueToSet}");
            }
            else
            {
                Debug.LogError("[ToggleHandler] Unrecognized PlayerPrefs key: " + prefName);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayButtonAudio();
    }

    //This is a button, has to be essentially instant.
    private void PlayButtonAudio()
    {
        if (buttonManager.audioSource != null)
        {
            // Get the current AudioClip from the AudioSource
            AudioClip clip = buttonManager.audioSource.clip;

            // Check if the clip is not null and if it needs to be loaded
            if (clip != null && (clip.loadType == AudioClipLoadType.Streaming || clip.loadType == AudioClipLoadType.DecompressOnLoad))
            {
                // Load the audio data
                clip.LoadAudioData();
            }

            // Play the audio
            Debug.Log("Play" + Time.deltaTime);
            buttonManager.audioSource.Play();
        }
    }
}
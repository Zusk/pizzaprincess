using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonClickHandler : MonoBehaviour, IPointerEnterHandler
{
    public ButtonManager buttonManager;
    public ButtonType buttonType; // This should be set in the ButtonManager when the button is created


    // Parameterless method for Unity Event (UnityEvents, i.e. onButtonClick require that your method has no parameter)
    public void OnButtonClick()
    {
        OnButtonClick(buttonType);
    }

    // Method to check if the buttonType is a label
    private bool IsLabel(ButtonType type)
    {
        return type == ButtonType.Options_Graphics_Label ||
               type == ButtonType.ExitGame_AreYouSure_Label ||
               type == ButtonType.Options_Difficulty_Label ||
               type == ButtonType.Options_Godmode_Label ||
               type == ButtonType.Blank_Label;
    }

    // Parameterized method for internal logic
    private void OnButtonClick(ButtonType type)
    {
        //This method is designed around taking whatever is the buttonType stored in this script - intended to be propagated by the buttonManager
        //It takes a array of objects, and does something depending on what the buttonType is.
        object[] menuItems; //This here is an array of objects.

        switch (type)
        {
            case ButtonType.StartGame:
                if(PauseManager.isPaused){
                    //Lock the cursor
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    //Get the pausemanager component
                    PauseManager pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
                    // Call the ResumeGame method on the PauseManager component
                    pauseManager.ResumeGame();
                }
                else{
                    SceneManager.LoadScene("LevelOne");
                }
                break;
            case ButtonType.Options:
                menuItems = new object[]
                {
                    ButtonType.Options_Graphics_Label,
                    ToggleType.PP_Graphics_2,
                    ToggleType.PP_Graphics_1,
                    ToggleType.PP_Graphics_0,
                    ButtonType.Options_Difficulty_Label,
                    ToggleType.PP_Difficulty_2,
                    ToggleType.PP_Difficulty_1,
                    ToggleType.PP_Difficulty_0,
                    ButtonType.Options_Godmode_Label,
                    ToggleType.PP_GodMode_1,
                    ToggleType.PP_GodMode_0,
                    ButtonType.Blank_Label,
                    ButtonType.Option_Default_Settings,
                    ButtonType.BackToMainMenu
                };
                buttonManager.BuildButtonMenu(menuItems);
                break;
            case ButtonType.ExitGame:
                menuItems = new object[]
                {
                    ButtonType.ExitGame_AreYouSure_Label,
                    ButtonType.ExitGame_AreYouSure_Y,
                    ButtonType.ExitGame_AreYouSure_N
                };
                buttonManager.BuildButtonMenu(menuItems);
                break;
            case ButtonType.EndGame_Exit:
            case ButtonType.ExitGame_AreYouSure_Y:
                Application.Quit();
                break;
            case ButtonType.ExitGame_AreYouSure_N:
                // Return to main menu logic
                ReturnToMainMenu();
                break;
            case ButtonType.BackToMainMenu:
                // Return to main menu logic
                ReturnToMainMenu();
                break;
            case ButtonType.Option_Default_Settings:
                buttonManager.BuildDefaultPlayerPrefs();
                menuItems = new object[]
                {
                    ButtonType.Options_Graphics_Label,
                    ToggleType.PP_Graphics_2,
                    ToggleType.PP_Graphics_1,
                    ToggleType.PP_Graphics_0,
                    ButtonType.Options_Difficulty_Label,
                    ToggleType.PP_Difficulty_2,
                    ToggleType.PP_Difficulty_1,
                    ToggleType.PP_Difficulty_0,
                    ButtonType.Options_Godmode_Label,
                    ToggleType.PP_GodMode_1,
                    ToggleType.PP_GodMode_0,
                    ButtonType.Blank_Label,
                    ButtonType.Option_Default_Settings,
                    ButtonType.BackToMainMenu
                };
                buttonManager.BuildButtonMenu(menuItems);
                break;
            case ButtonType.Lore:
                object[] loreMenuItems = new object[]
                {
                    ButtonType.Lore_Label,
                    ButtonType.BackToMainMenu
                };
                buttonManager.BuildButtonMenu(loreMenuItems);
                break;
            case ButtonType.Credits:
                object[] creditsMenuItems = new object[]
                {
                    ButtonType.EndGame_Credits_Label,
                    ButtonType.BackToMainMenu
                };
                buttonManager.BuildButtonMenu(creditsMenuItems);
                break;
            case ButtonType.How_To_Play:
                object[] howtoplayMenuItems = new object[]
                {
                    ButtonType.How_To_Play_Label,
                    ButtonType.BackToMainMenu
                };
                buttonManager.BuildButtonMenu(howtoplayMenuItems);
                break;
            //Use fallthrough here, as they all do the same thing.
            case ButtonType.Dialogue_1_Option_1:
            case ButtonType.Dialogue_2_Option_1:
            case ButtonType.Dialogue_3_Option_1:
            case ButtonType.Dialogue_4_Option_1:
            case ButtonType.Dialogue_5_Option_1:
            case ButtonType.Dialogue_6_Option_1:
            case ButtonType.Dialogue_7_Option_1:
                EndDialogue();
                break;
            case ButtonType.Dialogue_4_Option_2:
                StartMinigame();
                EndDialogue();
                break;
            case ButtonType.EndGame_Restart:
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            default:
                Debug.Log("Unknown Button Type");
                break;
        }
    }
    private void EndDialogue(){
        // Find the DialogueManager in the scene and call EndDialogue
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.ToggleDialogue();
        }
        else
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }
    }

    private void StartMinigame()
    {
        buttonManager.miniGameLayout.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsLabel(buttonType))
        {
            PlayButtonAudio();
        }
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


    // Function to return to the main menu
    private void ReturnToMainMenu()
    {
        object[] menuItems = new object[]
        {
            ButtonType.StartGame,
            ButtonType.Options,
            ButtonType.How_To_Play,
            ButtonType.Lore,
            ButtonType.Credits,
            ButtonType.ExitGame
        };
        buttonManager.BuildButtonMenu(menuItems);
    }
}

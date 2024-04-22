using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum ButtonType
{
    StartGame,
    Options,
    Options_Graphics_Label,
    Options_Graphics_Fancy,
    Options_Graphics_Medium,
    Options_Graphics_Wimpy,
    Options_Difficulty_Label,
    Options_Difficulty_Hard,
    Options_Difficulty_Normal,
    Options_Difficulty_Easy,
    Options_Cheats_Label,
    Options_Godmode_Label,
    Blank_Label,
    Option_Default_Settings,
    BackToMainMenu,
    ExitGame,
    ExitGame_AreYouSure_Label,
    ExitGame_AreYouSure_Y,
    ExitGame_AreYouSure_N,
    Dialogue_1_Topic_Label,
    Dialogue_1_Option_1,
    Dialogue_2_Topic_Label,
    Dialogue_2_Option_1,
    Dialogue_3_Topic_Label,
    Dialogue_3_Option_1,
    Dialogue_4_Topic_Label,
    Dialogue_4_Option_1,
    Dialogue_4_Option_2,
    Dialogue_5_Topic_Label,
    Dialogue_5_Option_1,
    Dialogue_6_Topic_Label,
    Dialogue_6_Option_1,
    Dialogue_7_Option_Label,
    Dialogue_7_Option_1,
    EndGame_Lore_Label,
    EndGame_Credits_Label,
    EndGame_Restart,
    EndGame_Exit,
    Lore,
    How_To_Play,
    Credits,
    How_To_Play_Label,
    Lore_Label
}

public enum ToggleType
{
    PP_Graphics_2,
    PP_Graphics_1,
    PP_Graphics_0,
    PP_Difficulty_2,
    PP_Difficulty_1,
    PP_Difficulty_0,
    PP_GodMode_1,
    PP_GodMode_0
}

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject togglePrefab;
    public GameObject toggleGroupPrefab;
    public AudioSource audioSource;
    public GameObject miniGameLayout;


    //A key-value pair that offers a localization string to each button.
    private Dictionary<ButtonType, string> buttonNames = new Dictionary<ButtonType, string>()
    {
        { ButtonType.StartGame, "Start Game" },
        { ButtonType.Options, "Options" },
        { ButtonType.Options_Graphics_Label, "Graphics Quality" },
        { ButtonType.Options_Difficulty_Label, "Difficulty" },
        { ButtonType.Options_Godmode_Label, "Godmode" },
        { ButtonType.Blank_Label, "" },
        { ButtonType.Option_Default_Settings, "Reset Settings" },
        { ButtonType.BackToMainMenu, "Main Menu" },
        { ButtonType.ExitGame, "Exit Game" },
        { ButtonType.ExitGame_AreYouSure_Label, "Are You Sure?" },
        { ButtonType.ExitGame_AreYouSure_Y, "Yes" },
        { ButtonType.ExitGame_AreYouSure_N, "No" },
        { ButtonType.Dialogue_1_Topic_Label, "Princess! You still need to clean up this mess!\nPick up your fork and go open up the door down there.\nI think it's been magically bewitched by the Corpers.\n\nJust use your fork with the Fire button and it will heal the door,\nremoving the gross Gibs!" },
        { ButtonType.Dialogue_1_Option_1, "Alright!" },
        { ButtonType.Dialogue_2_Topic_Label, "Those pizza boxes walking around are Pizzites.\n\nThey are ordinary objects magically bewitched with evil gibs.\nYour fork is able to cast a spell to heal the objects to return them to normal.\nYou don't know what you might find when you get rid of those Gibs, you could get anything!"},
        { ButtonType.Dialogue_2_Option_1, "They sure are cute though."},
        { ButtonType.Dialogue_3_Topic_Label, "Careful with that Big Pizza!\n\nIt has a LOT of Gibs in it. It might take a few spells to heal it fully!\n\nStay out of its reach, it can crush you\nif it gets too close!"},
        { ButtonType.Dialogue_3_Option_1, "I don't want to be smushed!"}, 
        { ButtonType.Dialogue_4_Topic_Label, "Hahaha! I am the Prank King!\n\nIf you want to have any hope of escaping my realm, you must\ncomplete a game, designed by me!\n\nClear my obstacle course, and I will give you the key to escape!"},
        { ButtonType.Dialogue_4_Option_1, "I am not ready for that yet!" },
        { ButtonType.Dialogue_4_Option_2, "Okay! Let's play!"},
        { ButtonType.Dialogue_5_Topic_Label, "Gibs are swarming this Level Exit Door!\n\nI think if you get the Key on top of the truck, you can\nscare them away."},
        { ButtonType.Dialogue_5_Option_1, "Guess I will have to get that!" },
        { ButtonType.Dialogue_6_Topic_Label, "You did it!" },
        { ButtonType.Dialogue_6_Option_1, "I am the best!" },
        { ButtonType.Dialogue_7_Option_Label, "I can't believe you did it!\nNot even the jumper legends have been able to clear this challenge!" },
        { ButtonType.Dialogue_7_Option_1, "I really am great."},
        { ButtonType.EndGame_Lore_Label, "With the help of her co-workers, and the begrudding assistance of the Prank King,\nthe Corpos and their evil Gibs have been kept out of the restauraunt! The day is saved!"},
        { ButtonType.EndGame_Credits_Label, "Pizza Princess\n\nA Game by Wiley Duncan O'Connell,\nfor Maysville Community and Technical College, 2023-2024."},
        { ButtonType.EndGame_Restart, "Play Again" },
        { ButtonType.EndGame_Exit, "Return to Desktop" },
        { ButtonType.How_To_Play, "How to Play" },
        { ButtonType.Lore, "Story" },
        { ButtonType.Credits, "Credits" },
        { ButtonType.Lore_Label, "Black Suits from the 8th Dimension are sick of losing customers to\nChef Bizarro Burrito Bravo's intergalactic eatery. In a\ndevious plot to undermine his success, they employ their advanced technology to\ntransform Chef Bravo's delightful dishes into monstrous 'Terror Treats'.\n\nIt's up to Princess Pepperoni, with her zesty wit and flavorful arsenal,\nto sweeten the pantry, turn the Terror Treats back into sweet snacks,\nand save the day before the lunch rush begins." },
        { ButtonType.How_To_Play_Label, "Shift to sprint\n\nEscape to pause\n\nMouse 1 to fire\n\nLook around with your mouse\n\nWASD to move around."}
    };

    //A key-value pair that offers a localization string to each toggle.
    private Dictionary<ToggleType, string> toggleNames = new Dictionary<ToggleType, string>()
    {
        { ToggleType.PP_Graphics_2, "Fancy" },
        { ToggleType.PP_Graphics_1, "Medium" },
        { ToggleType.PP_Graphics_0, "Wimpy" },
        { ToggleType.PP_Difficulty_2, "Hard" },
        { ToggleType.PP_Difficulty_1, "Normal" },
        { ToggleType.PP_Difficulty_0, "Easy" },
        { ToggleType.PP_GodMode_1, "Enable" },
        { ToggleType.PP_GodMode_0, "Disable" }
    };

    public void Awake()
    {
        // Check if "PP_Graphics" key doesn't exist
        if (!PlayerPrefs.HasKey("PP_Graphics"))
        {
            // No default player prefs exist. Build them.
            BuildDefaultPlayerPrefs();
        }
        else
        {
            // Retrieve the quality level from PlayerPrefs
            int qualityLevel = PlayerPrefs.GetInt("PP_Graphics");
            
            // Set the graphics quality based on the retrieved value
            QualitySettings.SetQualityLevel(qualityLevel, true);
        }
        if (!gameObject.CompareTag("Special_1"))
        {
            // Builds the base menu
            BuildButtonMenu(new object[] { ButtonType.StartGame, ButtonType.Options, ButtonType.How_To_Play, ButtonType.Lore, ButtonType.Credits, ButtonType.ExitGame });

            if (!PauseManager.isPaused)
            {
                gameObject.AddComponent<AudioListener>();
            }
        }
    }

    // Call this method to set default player preferences
    public void BuildDefaultPlayerPrefs()
    {
        // Sets "PP_Graphics" to 1
        PlayerPrefs.SetInt("PP_Graphics", 2);

        // Sets "PP_Godmode" to 0
        PlayerPrefs.SetInt("PP_GodMode", 0);

        // Sets "PP_Difficulty" to 1
        PlayerPrefs.SetInt("PP_Difficulty", 1);

        // Save the changes
        PlayerPrefs.Save();
    }

    public void BuildButtonMenu(object[] items)
    {
        // Remove all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        //Initialize the toggle group
        ToggleGroup currentToggleGroup = null;
        //Loop through each item in the array. Toggles and Buttons have unique logic that you have to account for.
        foreach (var item in items)
        {
            if (item is ButtonType)
            {
                // Cast the item to ButtonType.
                ButtonType buttonType = (ButtonType)item;
                // Instantiate a new button from the prefab and set its parent.
                GameObject newButton = Instantiate(buttonPrefab, transform);
                if (newButton == null)
                {
                    Debug.LogError("[ButtonManager]: Failed to instantiate new button.");
                    return; // Exit early if button instantiation fails.
                }
                // Set the button's name based on the ButtonType.
                newButton.name = buttonNames[buttonType];
                // Get and set the TextMeshProUGUI component text.
                TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = newButton.name;
                }
                else
                {
                    Debug.LogError("[ButtonManager]: TextMeshProUGUI component not found on the Button.");
                }
                // Get and set the ButtonClickHandler component.
                ButtonClickHandler clickHandler = newButton.GetComponent<ButtonClickHandler>();
                if (clickHandler != null)
                {
                    clickHandler.buttonManager = this;
                    clickHandler.buttonType = buttonType;
                }
                else
                {
                    Debug.LogError("[ButtonManager]: ButtonClickHandler component not found on the Button.");
                }
                // Disable interaction for label buttons and change their images to gray.
                if (buttonType.ToString().Contains("_Label"))
                {
                    Button buttonComponent = newButton.GetComponent<Button>();
                    Image imageComponent = newButton.GetComponent<Image>(); // Getting the UIImage component

                    if (buttonComponent != null)
                    {
                        buttonComponent.interactable = false;

                        if (imageComponent != null)
                        {
                            imageComponent.color = Color.gray; // Changing the color of the UIImage to gray
                        }
                        else
                        {
                            Debug.LogError("[ButtonManager]: Image component not found on the label Button.");
                        }
                    }
                    else
                    {
                        Debug.LogError("[ButtonManager]: Button component not found on the label Button.");
                    }
                }
                Debug.Log("[ButtonManager]: Successfully Made Button: " + buttonType.ToString());
                // Reset the currentToggleGroup when encountering a non-toggle ButtonType.
                if (currentToggleGroup != null)
                {
                    UpdateLayoutGroup(currentToggleGroup.gameObject);
                    currentToggleGroup = null;
                }
            }
            //Create a toggle
            else if (item is ToggleType)
            {
                Debug.Log("[ButtonManager]: Making a toggle..");
                //Cast this item to a toggle
                ToggleType toggleType = (ToggleType)item;

                // Check if we need to create a new ToggleGroup
                if (currentToggleGroup == null)
                {
                    GameObject toggleGroupObj = Instantiate(toggleGroupPrefab, transform);
                    currentToggleGroup = toggleGroupObj.GetComponent<ToggleGroup>();
                    if (currentToggleGroup == null)
                    {
                        Debug.LogError("[ButtonManager]: ToggleGroup component not found on the instantiated toggleGroupPrefab.");
                        return; // Exit early as something is fundamentally wrong
                    }
                }

                Debug.Log("[ButtonManager]: Attempting to create the toggle..");
                if (togglePrefab != null)
                {
                    Debug.Log("[ButtonManager]: Toggle Prefab Name: " + togglePrefab.name);
                }
                else
                {
                    Debug.LogError("[ButtonManager]: Toggle prefab is not assigned.");
                    return; // Exit early as the toggle prefab is missing
                }

                GameObject newToggle = Instantiate(togglePrefab, currentToggleGroup.transform);
                if (newToggle == null)
                {
                    Debug.LogError("[ButtonManager]: Failed to instantiate new toggle.");
                    return; // Exit early as the toggle failed to instantiate
                }
                //Sets the name of the toggle object to the corresponding enum string.
                newToggle.name = toggleType.ToString();
                ToggleHandler toggleHandler = newToggle.GetComponent<ToggleHandler>();
                if (toggleHandler != null)
                {
                    toggleHandler.buttonManager = this;
                }
                else
                {
                    Debug.LogError("[ButtonManager]: ToggleHandler component not found on the Toggle.");
                }

                TextMeshProUGUI toggleTextComponent = toggleHandler.textComponent;
                if (toggleTextComponent != null)
                {
                    //Sets the text of the textmeshpro object to the corresponding enum string.
                    toggleTextComponent.text = toggleNames[toggleType];
                }
                else
                {
                    Debug.LogError("[ButtonManager]: TextMeshProUGUI component not found on the Toggle.");
                }

                Toggle toggleComponent = newToggle.GetComponent<Toggle>();
                if (toggleComponent != null)
                {
                    //Sets up the togglegroup, which is a unity component to handle toggle exclusivity.
                    toggleComponent.group = currentToggleGroup;
                }
                else
                {
                    Debug.LogError("[ButtonManager]: Toggle component not found on the new toggle game object.");
                }
            }
        }
        //Layout updating. First update the internal vertical layout groups if it hasn't been yet
        if(currentToggleGroup != null){
            UpdateLayoutGroup(currentToggleGroup.gameObject);
        }
        // ...Then update the main one
        UpdateLayoutGroup(this.gameObject);
        UpdateLayoutGroupInChildren(this.gameObject);
    }

    // Method to update the layouts for vertical layouts specifically
    // Vertical layout groups, after you change their children, have to be updated with 'ForceRebuildLayoutImmediate'. This is just a wrapper method to handle that call.
    private void UpdateLayoutGroup(GameObject layoutGroupObject)
    {
        //Rebuilds the layout if we have a vertical layout using built in unity functions.
        VerticalLayoutGroup layoutGroup = layoutGroupObject.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
        else
        {
            Debug.LogError("[ButtonManager]: VerticalLayoutGroup component not found on the UI GameObject.");
        }
    }
    // Updated method to update the layouts for vertical layouts in all children
    private void UpdateLayoutGroupInChildren(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            VerticalLayoutGroup childLayoutGroup = child.GetComponent<VerticalLayoutGroup>();
            if (childLayoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(childLayoutGroup.GetComponent<RectTransform>());
            }

            // Recursive call to update layout groups in nested children
            if (child.childCount > 0)
            {
                UpdateLayoutGroupInChildren(child.gameObject);
            }
        }
    }
}

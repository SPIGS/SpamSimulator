using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Toolbar : MonoBehaviour
{
    public VisualTreeAsset aboutWindowTemplate;
    public ModalWindowController windowCreator;
    public FontScaler fontScaler;

    private VisualElement root;
    
    // File Dropdown and buttons
    private Button fileButton;
    private Button newGameButton;
    private Button exitGameButton;

    // Settings dropdown and buttons
    private Button settingsButton;

    // Tools dropdown and buttons
    private Button toolsButton;

    // About button
    private Button aboutButton;

    private enum ToolbarButtons {
        FILE,
        SETTINGS,
        TOOLS,
        ABOUT
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the buttons
        root = GetComponent<UIDocument>().rootVisualElement;

        // File buttons
        fileButton = root.Query<Button>("FileButton");
        newGameButton = root.Query<Button>("New");
        exitGameButton = root.Query<Button>("Exit");

        settingsButton = root.Query<Button>("SettingsButton");
        toolsButton = root.Query<Button>("ToolsButton");
        aboutButton = root.Query<Button>("AboutButton");

        //Register Callbacks

        // File Dropdown
        fileButton.clickable.clicked += () => {
            ToggleButtonActive(ToolbarButtons.FILE);
        };
        newGameButton.clickable.clicked += () => {
            SceneManager.LoadScene("Game");
        };
        exitGameButton.clickable.clicked += () =>
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        };



        settingsButton.clickable.clicked += () =>
        {
            ToggleButtonActive(ToolbarButtons.SETTINGS);
        };
        toolsButton.clickable.clicked += () =>
        {
            ToggleButtonActive(ToolbarButtons.TOOLS);
        };
        aboutButton.clickable.clicked += () =>
        {
            ToggleButtonActive(ToolbarButtons.ABOUT);
        };

        // Set pointer up on event
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);

    }

    void ToggleButtonActive (ToolbarButtons pressed) {

        DeactivateButtons();
        HideAllDropdowns();

        switch (pressed)
        {
            case ToolbarButtons.FILE:
                fileButton.AddToClassList("toolbar-button-selected");
                ShowFileDropdown();
                break;
            case ToolbarButtons.SETTINGS:
                settingsButton.AddToClassList("toolbar-button-selected");
                ShowSettingsDropdown();
                break;
            case ToolbarButtons.TOOLS:
                toolsButton.AddToClassList("toolbar-button-selected");
                ShowToolsDropdown();
                break;
            case ToolbarButtons.ABOUT:
                aboutButton.AddToClassList("toolbar-button-selected");
                ShowAboutModal();
                break;
        }
    }
    void OnPointerUp (PointerUpEvent evt) {
        // This is to handle the case where we click somewhere that isn't the toolbar
        DeactivateButtons();
        HideAllDropdowns();
    }

    void DeactivateButtons() {
        fileButton.RemoveFromClassList("toolbar-button-selected");
        settingsButton.RemoveFromClassList("toolbar-button-selected");
        toolsButton.RemoveFromClassList("toolbar-button-selected");
        aboutButton.RemoveFromClassList("toolbar-button-selected");
    }

    void ShowFileDropdown () {
        VisualElement fileDropdown = root.Query<VisualElement>("FileDropdown");
        fileDropdown.visible = true;
    }

    void ShowSettingsDropdown() {
        VisualElement settingsDropdown = root.Query<VisualElement>("SettingsDropdown");
        settingsDropdown.visible = true;
    }

    void ShowToolsDropdown() {
        VisualElement toolsDropdown = root.Query<VisualElement>("ToolsDropdown");
        toolsDropdown.visible = true;
    }

    void ShowAboutModal () {
        float width = 600.0f * fontScaler.fontScale *fontScaler.fontScale;
        float height = 200.0f * fontScaler.fontScale * fontScaler.fontScale;
        this.windowCreator.CreateModal(this.aboutWindowTemplate, 
                (Screen.width/2.0f) - width/2.0f, 
                (Screen.height/2.0f) - height/2.0f, 
                width, 
                height,
                "About this game");
    }

    void HideAllDropdowns () {
        VisualElement fileDropdown = root.Query<VisualElement>("FileDropdown");
        fileDropdown.visible = false;
        VisualElement settingsDropdown = root.Query<VisualElement>("SettingsDropdown");
        settingsDropdown.visible = false;
        VisualElement toolsDropdown = root.Query<VisualElement>("ToolsDropdown");
        toolsDropdown.visible = false;
    }
}

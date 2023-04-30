using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;

public class BlueScreenController : MonoBehaviour
{
    public UIDocument uiDocument;
    public SoundController soundController;
    private VisualElement root;

    private enum Option {
        NEW_GAME,
        QUIT
    }

    private Option currentOption = Option.NEW_GAME;

    void Start() {
        root = uiDocument.rootVisualElement;
        Button newgameButton = root.Query<Button>("NewGame");
        newgameButton.Focus();
        soundController.PlayAudioClip("BIOS Beep");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentOption == Option.NEW_GAME) {
                currentOption = Option.QUIT;
                Button quitButton = root.Query<Button>("Quit");
                quitButton.Focus();
            } else {
                currentOption = Option.NEW_GAME;
                Button newgameButton = root.Query<Button>("NewGame");
                newgameButton.Focus();
            }
        } else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            if (currentOption == Option.NEW_GAME) {
                SceneManager.LoadScene("Game");
            } else {
                
                if (EditorApplication.isPlaying) {
                    EditorApplication.isPlaying = false;
                } else {
                    Application.Quit();
                }
            }
            
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
//using UnityEditor;

public class BlueScreenController : MonoBehaviour
{
    public UIDocument uiDocument;
    public string scoreLabelText = "*** YOUR SCORE: ";
    public string highscoreLabelText = "*** HIGHSCORE: ";
    private VisualElement root;
    private Label scoreLabel;

    private Label lossLabel;

    private enum Option {
        NEW_GAME,
        QUIT
    }

    private Option currentOption = Option.NEW_GAME;

    void Start() {
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        root = uiDocument.rootVisualElement;

        scoreLabel = root.Q<Label>("Score");
        scoreLabel.text = scoreLabelText + PlayerPrefs.GetInt("Score", -1);

        scoreLabel = root.Q<Label>("Highscore");
        scoreLabel.text = highscoreLabelText + PlayerPrefs.GetInt("Highscore", -1);

        lossLabel = root.Q<Label>("GameOverReason");
        if (PlayerPrefs.GetInt("Fired", 0) == 0) {
            lossLabel.text = "SERVER OVERLOAD";
        } else {
            lossLabel.text = "YOU'VE BEEN FIRED";
        }

        Button newGameButton = root.Query<Button>("NewGame");
        newGameButton.clickable.clicked += () => {
            SceneManager.LoadScene("Boot");
        };

        Button quitButton = root.Query<Button>("Quit");
        quitButton.clickable.clicked += () => {
            Application.Quit();
        };
        newGameButton.Focus();
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
                SceneManager.LoadScene("Boot");
            } else {
                
                // if (EditorApplication.isPlaying) {
                //     EditorApplication.isPlaying = false;
                // } else {
                    
                // }
                Application.Quit();
            }
            
        }

    }
}

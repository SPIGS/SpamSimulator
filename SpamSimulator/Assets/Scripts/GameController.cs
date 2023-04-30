using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UIDocument uiDocument;
    public int score = 0;
    public int strikes = 0;
    public float newEmailDelaySec = 1.0f;
    public bool gameOver = false;
    
    private VisualElement root;
    
    private float timeElapsed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        root = uiDocument.rootVisualElement;
    }

    void Update () {

        if (!gameOver) {
            // Email generation timing
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= newEmailDelaySec)
            {
                // Tell email controller to make new email.
                timeElapsed = 0.0f;
            }
        } else {
            // Load BSOD
            SceneManager.LoadScene("GameOver");
        }
    }

    public void OnDeleteGoodEmail () {
        //
        Debug.Log("Delete Good Email");
    }

    public void OnPassGoodEmail () {
        //score go up
        // power up points?
        Debug.Log("Approve Good Email");
    }

    public void OnPassBadEmail () {
        /// various things
        Debug.Log("Approve Bad Email");
    }

    public void OnDeleteBadEmail () {
        //score go up
        //power up poitns?
        Debug.Log("Delete Bad Email");
    }

    public void OnFullStorage () {
        //end game
        this.gameOver = true;
    }
}

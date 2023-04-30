using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using SpamSim;

public class GameController : MonoBehaviour
{
    public UIDocument uiDocument;
    public StoryController storyController;
    public EmailController emailController;
    public int[] adminEmailCounters;
    public int score = 0;
    public Vector2 emailDelayRange = new Vector2(5.0f, 10.0f);
    
    [Range(0.0f, 1.0f)]
    public float spamWeight = 0.10f;
    public bool gameOver = false;
    
    private VisualElement root;
    
    private float timeElapsed = 0.0f;
    private int numEmailsCreated = 0;
    private int nextAdminEmail = 0;
    // Start is called before the first frame update
    void Start()
    {
        root = uiDocument.rootVisualElement;
    }

    void Update () {

        if (!gameOver) {
            // Email generation timing
            timeElapsed += Time.deltaTime;
            float delay = Random.Range(emailDelayRange.x, emailDelayRange.y);
            if (timeElapsed >= delay)
            {
                if (numEmailsCreated == nextAdminEmail) {
                    //Send an admin email
                    string email = storyController.GetAdminEmail(nextAdminEmail);
                    emailController.AddEmail(email);
                    nextAdminEmail ++;
                } else {
                    // Ask pretty please for a new email
                    double randomValue = Random.Range(0.0f, 1.0f);
                    if (randomValue <= spamWeight)
                    {
                        string email = storyController.GetEmail(true);
                        emailController.AddEmail(email);
                    }
                    else
                    {
                        string email = storyController.GetEmail(false);
                        emailController.AddEmail(email);
                    }
                }
                timeElapsed = 0.0f;
                numEmailsCreated++;
            }
        } else {
            // Load BSOD
            SceneManager.LoadScene("BlueScreen");
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

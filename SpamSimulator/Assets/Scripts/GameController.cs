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
    public VirusController virusController;
    public int[] adminEmailCounters;
    public int score = 0;
    public Vector2 emailDelayRange = new Vector2(5.0f, 10.0f);
    
    [Range(0.0f, 1.0f)]
    public float spamWeight = 0.10f;
    public bool gameOver = false;
    public string scoreFooterLabel = "Score: ";
    public string storageFooterLabel = "Available Storage: ";
    
    private VisualElement root;
    
    private float timeElapsed = 0.0f;
    private int numEmailsCreated = 0;
    private int nextAdminEmail = 0;
    private bool adminEmailIsActive = false;
    private bool bootIsDone = false;
    private Label scoreLabel;
    private Label storageLabel;
    void Start()
    {
        root = uiDocument.rootVisualElement;
        scoreLabel = root.Q<Label>("ScoreLabel");
        scoreLabel.text = scoreFooterLabel + score;
        storageLabel = root.Q<Label>("StorageLabel");
        UpdateStorage();
    }

    void Update () {

        if (!gameOver) {
            // Email generation timing
            timeElapsed += Time.deltaTime;
            if (bootIsDone) {
                float delay = Random.Range(emailDelayRange.x, emailDelayRange.y);
                if (timeElapsed >= delay)
                {
                    if (numEmailsCreated >= nextAdminEmail && nextAdminEmail < adminEmailCounters.Length && !adminEmailIsActive)
                    {
                        //Send an admin email
                        adminEmailIsActive = true;
                        string email = storyController.GetAdminEmail(nextAdminEmail);
                        emailController.AddEmail(email);
                        nextAdminEmail++;
                    }
                    else
                    {
                        if (!adminEmailIsActive)
                        {
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
                    }
                    timeElapsed = 0.0f;
                    numEmailsCreated++;
                    UpdateStorage();
                }
            } else {
                if (timeElapsed >= 6.5) {
                    bootIsDone = true;
                }
            } 
        } else {
            // Load BSOD
            SceneManager.LoadScene("BlueScreen");
        }
    }

    public void OnDeleteGoodEmail () {
        UpdateScore(-1);
    }

    public void OnPassGoodEmail () {
        UpdateScore(1);
    }

    public void OnPassBadEmail () {
        Debug.Log("Pass bad email");
        UpdateScore(-1);
        virusController.CreateAndTriggerVirus(VirusType.SEND_EXTRA_SPAM);
    }

    public void OnDeleteBadEmail () {
        Debug.Log("Delete Bad Email");
        UpdateScore(1);
    }

    public void OnFullStorage () {
        //end game
        this.gameOver = true;
    }

    public void SetAdminEmailInactive () {
        adminEmailIsActive = false;
    }

    public void UpdateScore(int points){
        score += points;
        scoreLabel.text = scoreFooterLabel + score;
    }

    public void UpdateStorage()
    {
        storageLabel.text = storageFooterLabel + emailController.GetCurrentStorage() + " / " + emailController.maxEmails;
    }
}

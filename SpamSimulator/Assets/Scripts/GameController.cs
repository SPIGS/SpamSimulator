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
    public SoundController soundController;
    public int[] adminEmailCounters;
    public int score = 0;
    public int strikes = 0;
    public int maxStrikes = 10;
    public float totalTimePassed = 0.0f;
    public Vector2 emailDelayRange = new Vector2(5.0f, 7.0f);

    [Range(0.0f, 1.0f)]
    public float spamWeight = 0.10f;
    public bool gameOver = false;
    public bool DEBUG_LOAD_ALL_EMAILS = false;
    public string scoreFooterLabel = "Score: ";
    public string storageFooterLabel = "Available Storage: ";
    public string strikesFooterLabel = "Strikes: ";

    public int adminEmailsSent = 0;

    private VisualElement root;

    private float timeElapsed = 0.0f;
    private float delay;
    private int numEmailsCreated = 0;
    private int nextAdminEmailScore = 0;
    private int adminEmailIndex = 0;
    private bool adminEmailIsActive = false;
    private bool allAdminEmailsSent = false;
    private bool DEBUG_SKIP_UPDATE = false;
    private Label scoreLabel;
    private Label storageLabel;
    private Label strikesLabel;

    void Start()
    {
        root = uiDocument.rootVisualElement;
        scoreLabel = root.Q<Label>("ScoreLabel");
        scoreLabel.text = scoreFooterLabel + score;
        storageLabel = root.Q<Label>("StorageLabel");
        strikesLabel = root.Q<Label>("StrikesLabel");
        UpdateStorage();
        UpdateStrikes(0);

        delay = 1f;
    }

    void Update()
    {

        if (DEBUG_LOAD_ALL_EMAILS)
        {
            emailController.maxEmails = 10000;
            maxStrikes = 10000;

            var emails = storyController.GetAllEmails();
            foreach (var email in emails)
            {
                emailController.AddEmail(email);
            }

            DEBUG_LOAD_ALL_EMAILS = false;
            DEBUG_SKIP_UPDATE = true;
            return;
        }

        if (!gameOver && !DEBUG_SKIP_UPDATE)
        {
            // Email generation timing
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= delay)
            {
                if (ShouldSendAdminEmail())
                {
                    //Send an admin email
                    adminEmailIsActive = true;
                    string email = storyController.GetAdminEmail(adminEmailIndex);
                    emailController.AddEmail(email);
                    adminEmailsSent++;
                    if (adminEmailIndex + 1 >= adminEmailCounters.Length)
                    {
                        allAdminEmailsSent = true;
                    }
                    else
                    {
                        nextAdminEmailScore = adminEmailCounters[adminEmailIndex + 1];
                        adminEmailIndex++;
                    }
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

                float delayOffset = (570f/ (-totalTimePassed - 114f)) + 5;

                delay = Random.Range(emailDelayRange.x - delayOffset, emailDelayRange.y - delayOffset);
                //Debug.Log($"Time Passed: {totalTimePassed} Range: {emailDelayRange.x - delayOffset}, {emailDelayRange.y - delayOffset} Next Email: {delay}");
            }

            if (!adminEmailIsActive)
            {
                totalTimePassed += Time.deltaTime;
            }

        }
        else if (!DEBUG_SKIP_UPDATE)
        {
            // Load BSOD

            int score = PlayerPrefs.GetInt("Score");

            // If the players current score is higher than the last highscore,
            // make it the new high score
            if (PlayerPrefs.GetInt("Highscore", -1) == -1 || PlayerPrefs.GetInt("Highscore", -1) <= score)
            {
                PlayerPrefs.SetInt("Highscore", score);
            }
            SceneManager.LoadScene("BlueScreen");
        }
    }

    bool ShouldSendAdminEmail()
    {
        if (allAdminEmailsSent)
        {
            return false;
        }
        if (adminEmailIsActive)
        {
            return false;
        }
        if (score < nextAdminEmailScore)
        {
            return false;
        }
        if (adminEmailIndex >= adminEmailCounters.Length)
        {
            return false;
        }
        return true;
    }

    public void OnDeleteGoodEmail()
    {
        UpdateScore(-1);
        UpdateStrikes(1);
        soundController.PlaySoundEffect("Error");
    }

    public void OnPassGoodEmail()
    {
        UpdateScore(1);
    }

    public void OnPassBadEmail()
    {
        Debug.Log("Pass bad email");
        UpdateScore(-1);
        if (adminEmailsSent >= 2 && !adminEmailIsActive)
        {
            int choice = Random.Range(0, 4);
            virusController.CreateAndTriggerVirus((VirusType)choice);
        }
        soundController.PlaySoundEffect("Error");
    }

    public void OnDeleteBadEmail()
    {
        Debug.Log("Delete Bad Email");
        UpdateScore(1);
    }

    public void OnFullStorage()
    {
        //end game
        PlayerPrefs.SetInt("Fired", 0);
        this.gameOver = true;
    }

    public void SetAdminEmailInactive()
    {
        adminEmailIsActive = false;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreLabel.text = scoreFooterLabel + score;

        //Store the players current score
        PlayerPrefs.SetInt("Score", score);
    }

    public void UpdateStorage()
    {
        storageLabel.text = storageFooterLabel + emailController.GetCurrentStorage() + " / " + emailController.maxEmails;
    }

    public void UpdateStrikes(int strikes) {
        this.strikes += strikes;
        strikesLabel.text = strikesFooterLabel + this.strikes + " / " + maxStrikes; 
        if (this.strikes >= maxStrikes) {
            PlayerPrefs.SetInt("Fired", 1);
            this.gameOver = true;
        }
    }
}

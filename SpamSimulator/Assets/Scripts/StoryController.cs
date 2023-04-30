using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryController : MonoBehaviour
{
    private class Story {
        public string Name;
        public List<string> Storyline;
    }

    public int maxActiveStories = 1;
    public List<string> storylineFiles;
    public List<string> spamFiles;
    private List<string> goodEmails = new List<string>();
    private List<string> badEmails = new List<string>();
    
    [Range(0.0f, 1.0f)]
    public float storyWeight = 0.33f;

    
    private List<Story> stories = new List<Story>();

    private List<string> activeStories;

    void Awake () {
        // Load Stories emails
        foreach (string storylineFile in storylineFiles) {
            string path = $"Assets/Emails/Content/{storylineFile}.json";
            string jsonString = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Storylines/{storylineFile}.json", typeof(TextAsset))).text;
            Story story = JsonUtility.FromJson<Story>(jsonString);
            this.stories.Add(story);
        }

        //Load spam
        foreach (string spamFile in spamFiles)
        {
            string spamFilename = $"{spamFile}.json";
            badEmails.Add(spamFilename);
        }
       
    }
    
    public string GetEmail(bool spam) {
        if (spam) {
            return badEmails[Random.Range(0, badEmails.Count)];

        } else {
            // Decide if good email is story or generic
            double randomValue = Random.Range(0.0f, 1.0f);
            if (randomValue <= storyWeight) {
                // Give story

                //Choose A story
                Story story = stories[Random.Range(0, stories.Count)];
                
                //Check if we can add a new story line/ story is already active. else give a generic email.
                if (!activeStories.Contains(story.Name) && activeStories.Count + 1 < maxActiveStories 
                    || activeStories.Contains(story.Name) && activeStories.Count <= maxActiveStories) {
                    activeStories.Add(story.Name);
                } else {
                    return goodEmails[Random.Range(0, goodEmails.Count)];
                }

                // Return actual story
                if (story.Storyline.Count <= 0) {
                    stories.Remove(story);
                    return goodEmails[Random.Range(0, goodEmails.Count)];
                } else {
                    string email = story.Storyline[0];
                    story.Storyline.Remove(email);
                    story.Storyline.TrimExcess();

                    return email;
                }
                
            } else {
                // Give Generic
                return goodEmails[Random.Range(0, goodEmails.Count)];
            }

        }
    }
}

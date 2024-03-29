using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class StoryController : MonoBehaviour
{
    private class Story {
        public string Name;
        public List<string> Storyline;
    }

    public int maxActiveStories = 1;
    public List<string> storylineFiles;
    public string adminStoryline;
    private List<string> goodEmailsAList = new List<string>();
    private List<string> goodEmailsBList = new List<string>();
    private List<string> badEmailsAList = new List<string>();
    private List<string> badEmailsBList = new List<string>();
    
    [Range(0.0f, 1.0f)]
    public float storyWeight = 0.33f;

    private List<Story> stories = new List<Story>();
    private List<string> activeStories = new List<string>();
    private Story adminStory;

    void Awake () {
        // Load Stories emails
        foreach (string storylineFile in storylineFiles) {
            string path = $"Assets/Emails/Content/{storylineFile}.json";
            //string jsonString = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Storylines/{storylineFile}.json", typeof(TextAsset))).text;
            string jsonString = ((TextAsset)Resources.Load($"Emails/Storylines/{storylineFile}", typeof(TextAsset))).text;

            Story story = JsonUtility.FromJson<Story>(jsonString);
            this.stories.Add(story);
        }

        //Load spam
        //string badJson = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/bad.json", typeof(TextAsset))).text;
        string badJson = Resources.Load<TextAsset>($"Emails/bad").text;
        Story tempStory = JsonUtility.FromJson<Story>(badJson);
        foreach (string badEmail in tempStory.Storyline)
        {
            badEmailsAList.Add(badEmail);
        }

        //Load generic good
        //string goodJson = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/good.json", typeof(TextAsset))).text;
        string goodJson = Resources.Load<TextAsset>($"Emails/good").text;
        tempStory = JsonUtility.FromJson<Story>(goodJson);
        foreach (string goodEmail in tempStory.Storyline)
        {
            goodEmailsAList.Add(goodEmail);
        }

        //Load admin Story
        //string adminStoryLineJSON = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Storylines/{adminStoryline}.json", typeof(TextAsset))).text;
        string adminStoryLineJSON = Resources.Load<TextAsset>($"Emails/Storylines/{adminStoryline}").text;
        adminStory = JsonUtility.FromJson<Story>(adminStoryLineJSON);
       
    }

    public string GetGoodGenericEmail() {
        
        if (goodEmailsAList.Count == 0)
        {
            goodEmailsAList.Clear();
            goodEmailsAList.AddRange(goodEmailsBList);
            goodEmailsBList.Clear();
        }
        int index = Random.Range(0, goodEmailsAList.Count);
        string email = goodEmailsAList[index];
        goodEmailsAList.Remove(email);
        goodEmailsBList.Add(email);
    
        return email;
    }
    
    public string GetSpamEmail () {
        if (badEmailsAList.Count == 0)
        {
            badEmailsAList.Clear();
            badEmailsAList.AddRange(badEmailsBList);
            badEmailsBList.Clear();
        }
        int index = Random.Range(0, badEmailsAList.Count);
        string email = badEmailsAList[index];
        badEmailsAList.Remove(email);
        badEmailsBList.Add(email);

        return email;
    }
    
    public string GetEmail(bool spam) {
        if (spam) {
            return GetSpamEmail();
        } else {
            // Decide if good email is story or generic
            double randomValue = Random.Range(0.0f, 1.0f);
            if (randomValue <= storyWeight) {
                // Give story

                //Choose A story
                Story story = stories[Random.Range(0, stories.Count)];
                //Check if we can add a new story line/ story is already active. else give a generic email.
                if (!activeStories.Contains(story.Name) && activeStories.Count + 1 <= maxActiveStories) {
                    activeStories.Add(story.Name);
                    //Return story
                } else if (activeStories.Contains(story.Name)) {
                    //Return Story
                } else {
                    return GetGoodGenericEmail();
                }

                // Return actual story
                if (story.Storyline.Count <= 0) {
                    stories.Remove(story);
                    return GetGoodGenericEmail();
                } else {
                    string email = story.Storyline[0];
                    story.Storyline.Remove(email);
                    story.Storyline.TrimExcess();
                    return email;
                }
                
            } else {
                // Give Generic
                return GetGoodGenericEmail();
            }

        }
    }

    public string GetAdminEmail (int index) {
        return adminStory.Storyline[index];
    }

    public List<string> GetAllEmails()
    {
        var allEmails = new List<string>();

        allEmails.AddRange(goodEmailsAList);
        allEmails.AddRange(goodEmailsBList);

        allEmails.AddRange(badEmailsAList);
        allEmails.AddRange(badEmailsBList);
        
        foreach (var story in stories)
        {
            allEmails.AddRange(story.Storyline);
        }

        allEmails.AddRange(adminStory.Storyline);

        return allEmails;
    }
}

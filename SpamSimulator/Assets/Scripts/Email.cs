using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class Email
    {
        public string Subject;
        public string Sender;

        public List<string> ImageFiles;
        public List<string> Headers;
        public List<string> Paragraphs;
        public List<string> Buttons;

        public List<Texture2D> Images;

        public bool IsSpam;

        public string Template;

        public int id;

        public bool IsAdmin;

        public static Email LoadEmail(string emailFileName, int id)
        {
            string jsonString = ((TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Content/{emailFileName}", typeof(TextAsset))).text;
            
            Email email = JsonUtility.FromJson<Email>(jsonString);

            email.id = id;
            email.LoadImages();

            return email;
        }

        public VisualElement InstantiateEmail()
        {
            VisualTreeAsset vta = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Templates/{Template}", typeof(VisualTreeAsset));

            VisualElement emailElement = vta.Instantiate();

            // Update Paragraphs
            int pCount = 0;
            foreach (Label p in emailElement.Query<Label>("Paragraph").ToList())
            {
                p.text = Paragraphs[pCount];
                pCount++;
            }

            // Update Images
            int iCount = 0;
            foreach (VisualElement i in emailElement.Query<VisualElement>("Image").ToList())
            {
                i.style.backgroundImage = new StyleBackground(Images[iCount]);
                iCount++;
            }

            // Update Headers
            int hCount = 0;
            foreach (Label h in emailElement.Query<Label>("Header").ToList())
            {
                h.text = Headers[hCount];
                hCount++;
            }

            // Update Buttons
            int bCount = 0;
            foreach (Button b in emailElement.Query<Button>("Button").ToList())
            {
                b.text = Buttons[bCount];
                bCount++;
            }

            return emailElement;
        }

        public void LoadImages()
        {
            Images = new List<Texture2D>();

            foreach (string file in ImageFiles)
            {
                Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Images/{file}", typeof(Texture2D));

                Images.Add(image);
            }
        }
    }
}
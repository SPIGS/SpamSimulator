using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class Email
    {
        public string Subject;
        public string Sender;
        public string SenderName;

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
            string jsonString = Resources.Load<TextAsset>($"Emails/Content/{emailFileName}").text;
            
            Email email = JsonUtility.FromJson<Email>(jsonString);

            email.id = id;
            email.LoadImages();

            return email;
        }

        public VisualElement InstantiateEmail()
        {
            string fileName = Template.Split('.')[0];
            VisualTreeAsset vta = Resources.Load<VisualTreeAsset>($"Emails/Templates/{fileName}");

            VisualElement emailElement = vta.Instantiate();

            // Update Email Header
            Label fromHeader = emailElement.Q<Label>("EmailHeaderFrom");
            fromHeader.text = $"{SenderName} ({Sender})";

            Label subjectHeader = emailElement.Q<Label>("EmailHeaderSubject");
            subjectHeader.text = Subject;

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
                Texture2D image = Images[iCount];
                i.style.backgroundImage = new StyleBackground(image);
                i.style.height = image.height;
                i.style.width = image.width;
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

            return emailElement.Children().First();
        }

        public void LoadImages()
        {
            Images = new List<Texture2D>();

            foreach (string file in ImageFiles)
            {
                //Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Emails/Images/{file}", typeof(Texture2D));
                string fileName = file.Split('.')[0];
                Texture2D image = (Texture2D)Resources.Load<Texture2D>($"Emails/Images/{fileName}");
                Images.Add(image);
            }
        }
    }
}
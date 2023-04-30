using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class EmailController : MonoBehaviour
    {
        private VisualElement emailRoot;
        private ScrollView inboxScrollView;
        private HashSet<Email> inbox;
        private Email currentEmail;
        private int emailCounter;

        public UIDocument uI;
        public VisualTreeAsset inboxItemTemplate;
        public FontScaler fontScaler;
        public GameController gameController;

        public int maxEmails;

        // Start is called before the first frame update
        void Start()
        {
            inbox = new HashSet<Email>();
            emailCounter = 0;

            emailRoot = uI.rootVisualElement.Q<VisualElement>(name: "EmailUI");
            inboxScrollView = uI.rootVisualElement.Q<ScrollView>(name: "InboxScrollView");

            AddEmail("testEmail.json");
            AddEmail("Apology_tom.json");
            AddEmail("Company Picnic_events.json");
            AddEmail("Complaint_cindy.json");
            AddEmail("Delay in Report_bob.json");
            //AddEmail("smellsuft_widgets_intro_spam.json");
            AddEmail("Meeting Confirmation_jane.json");
            AddEmail("smellsuft_widgets_promo1_spam.json");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void AddEmail(string emailFileName)
        {
            if (inbox.Count >= maxEmails)
            {
                OnGameOver();
                return;
            }

            Email email = Email.LoadEmail(emailFileName, emailCounter);

            inbox.Add(email);

            AddInboxItem(email);

            emailCounter++;
        }

        void AddInboxItem(Email email) {
            //Instaniate the button template
            VisualElement inboxItem = inboxItemTemplate.Instantiate();

            //Set the name of the button template so that we can delete it later.
            string inboxItemName = $"InboxItem{email.id}";
            inboxItem.name = inboxItemName;

            //Set From and Subject
            Label fromLabel = inboxItem.Q<Label>("FromLabel");
            fromLabel.text = email.Sender;
            fromLabel.style.fontSize = 24 * fontScaler.fontScale;

            Label subjectLabel = inboxItem.Q<Label>("SubjectLabel");
            subjectLabel.text = email.Subject;
            subjectLabel.style.fontSize = 24 * fontScaler.fontScale;

            inboxItem.Q<Label>("FromTitle").style.fontSize = 24 * fontScaler.fontScale;
            inboxItem.Q<Label>("SubjectTitle").style.fontSize = 24 * fontScaler.fontScale;

            //Button scaling
            VisualElement trashButton = inboxItem.Q<VisualElement>("TrashIcon");
            trashButton.style.width = 24 * fontScaler.fontScale;
            trashButton.style.height = 24 * fontScaler.fontScale;

            VisualElement passButton = inboxItem.Q<VisualElement>("PassIcon");
            passButton.style.width = 24 * fontScaler.fontScale;
            passButton.style.height = 24 * fontScaler.fontScale;

            //Set button callbacks
            inboxItem.Q<Button>("MainItemButton").clickable.clicked += () => {
                OnOpenEmail(email);
            };
            inboxItem.Q<Button>("PassButton").clickable.clicked += () => {
                OnProcessEmail(inboxItem, email, "approve");
            };
            inboxItem.Q<Button>("TrashButton").clickable.clicked += () => {
                OnProcessEmail(inboxItem, email, "delete");
            };

            inboxScrollView.Add(inboxItem);
        }

        void OnOpenEmail(Email email)
        {
            emailRoot.Clear();
            emailRoot.Add(email.InstantiateEmail());
            currentEmail = email;
        }

        void OnProcessEmail(VisualElement inboxItem, Email email, string action)
        {
            inboxScrollView.Remove(inboxItem);

            if (currentEmail == email) 
            {
                emailRoot.Clear();
                currentEmail = null;
            }

            inbox.Remove(email);

            if (action == "approve" && !email.IsSpam) 
            {
                gameController.OnPassGoodEmail();
            }
            else if (action == "approve" && email.IsSpam)
            {
                gameController.OnPassBadEmail();
            }
            else if (action == "delete" && !email.IsSpam) 
            {
                gameController.OnDeleteGoodEmail();
            }
            else if (action == "delete" && email.IsSpam)
            {
                gameController.OnDeleteBadEmail();
            }
        }

        void OnGameOver()
        {
            gameController.OnFullStorage();
        }
    }
}

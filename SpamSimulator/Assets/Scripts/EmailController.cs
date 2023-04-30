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
        private List<Email> inbox;
        private Email currentEmail;
        private int emailCounter;

        public UIDocument uI;
        public VisualTreeAsset inboxItemTemplate;
        public FontScaler fontScaler;

        // Start is called before the first frame update
        void Start()
        {
            inbox = new List<Email>();
            emailCounter = 0;

            emailRoot = uI.rootVisualElement.Q<VisualElement>(name: "EmailUI");
            inboxScrollView = uI.rootVisualElement.Q<ScrollView>(name: "InboxScrollView");

            AddEmail("testEmail.json");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void AddEmail(string emailFileName)
        {
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
                OnPassEmail(inboxItem, email);
            };
            inboxItem.Q<Button>("TrashButton").clickable.clicked += () => {
                OnTrashEmail(inboxItem, email);
            };

            inboxScrollView.Add(inboxItem);
        }

        void OnOpenEmail(Email email)
        {
            emailRoot.Clear();
            emailRoot.Add(email.InstantiateEmail());
            currentEmail = email;
        }

        void OnTrashEmail(VisualElement inboxItem, Email email)
        {

        }

        void OnPassEmail(VisualElement inboxItem, Email email)
        {

        }
    }
}

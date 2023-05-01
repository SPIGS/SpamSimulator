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
        public SoundController soundController; 
        public string newEmailSoundEffect = "New Email";
        public string deleteEmailSoundEffect = "Trash Email";
        public string passEmailSoundEffect = "Pass Email";

        public int maxEmails = 32;
        public int subjectLengthCutoff = 25;

        // Start is called before the first frame update
        void Start()
        {
            inbox = new HashSet<Email>();
            emailCounter = 0;

            emailRoot = uI.rootVisualElement.Q<VisualElement>(name: "EmailUI");
            inboxScrollView = uI.rootVisualElement.Q<ScrollView>(name: "InboxScrollView");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void AddEmail(string emailFileName)
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
            fromLabel.text = email.SenderName;

            string subject = email.Subject;
            if (email.Subject.Length > subjectLengthCutoff)
            {
                subject = $"{email.Subject.Substring(0, subjectLengthCutoff-3)}...";
            }

            Label subjectLabel = inboxItem.Q<Label>("SubjectLabel");
            subjectLabel.text = subject;

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

            fontScaler.scaleFont(inboxItem);

            inboxScrollView.Insert(0, inboxItem);

            soundController.PlaySoundEffect(newEmailSoundEffect);
        }

        void OnOpenEmail(Email email)
        {
            emailRoot.Clear();

            VisualElement emailView = email.InstantiateEmail();
            fontScaler.scaleFont(emailView);

            emailRoot.Add(emailView);
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

            if (email.IsAdmin) {
                gameController.SetAdminEmailInactive();
            }

            inbox.Remove(email);

            // if delete play trash sound else play approve sound
            if (action == "delete") {
                soundController.PlaySoundEffect(deleteEmailSoundEffect);
            } else {
                soundController.PlaySoundEffect(passEmailSoundEffect);
            }


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
            gameController.UpdateStorage();
        }

        void OnGameOver()
        {
            gameController.OnFullStorage();
        }

        public int GetCurrentStorage() {
            return inbox.Count;
        }
    }
}

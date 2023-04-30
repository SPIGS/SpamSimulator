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

        public int maxEmails;

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
            fromLabel.text = email.Sender;

            Label subjectLabel = inboxItem.Q<Label>("SubjectLabel");
            subjectLabel.text = email.Subject;

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

            inboxScrollView.Add(inboxItem);

            soundController.PlaySoundEffect("New Email");
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

            if (email.IsAdmin) {
                gameController.SetAdminEmailInactive();
            }

            inbox.Remove(email);

            // if delete play trash sound else play approve sound
            if (action == "delete") {
                soundController.PlaySoundEffect("Trash Email");
            } else {

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
        }

        void OnGameOver()
        {
            gameController.OnFullStorage();
        }
    }
}

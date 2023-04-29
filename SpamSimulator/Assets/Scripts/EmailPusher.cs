using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmailPusher : MonoBehaviour
{
    public VisualTreeAsset inboxButtonTemplate;
    public float newEmailDelaySec = 1.0f; 

    private float timeElapsed = 0.0f;
    private int emailCounter = 0;
    private ScrollView inboxScrollView;

    void Start() {
        // Get the inbox scroll view
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        inboxScrollView = root.Query<ScrollView>("InboxScrollView");
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= newEmailDelaySec) {
            inboxScrollView.Add(CreateInboxItem("Administrator", "Happy Birthday!"));
            timeElapsed = 0.0f;
        }
    }

    void OpenEmail (int itemId) {
        Debug.Log("Open Email");

        //Code to open email goes here
    }

    void PassEmail (int itemId) {
        Debug.Log("Pass Email");
        DeleteInboxItem(itemId);

        //Code to pass email goes here
    }

    void TrashEmail (int itemId) {
        Debug.Log("Trash Email");
        DeleteInboxItem(itemId);

        //Code to trash email goes here
    }

    VisualElement CreateInboxItem(string from, string subject) {
        emailCounter++;
        //Instaniate the button template
        VisualElement test = inboxButtonTemplate.Instantiate();

        int itemId = emailCounter;

        //Set the name of the button template so that we can delete it later.
        test.name = "InboxItem" + itemId;

        //Set From and Subject
        test.Q<Label>("FromLabel").text = from;
        test.Q<Label>("SubjectLabel").text = emailCounter.ToString();

        //Set button callbacks
        test.Q<Button>("MainItemButton").clickable.clicked += () => {
            OpenEmail(itemId);
        };
        test.Q<Button>("PassButton").clickable.clicked += () => {
            PassEmail(itemId);
        };
        test.Q<Button>("TrashButton").clickable.clicked += () => {
            TrashEmail(itemId);
        };
        return test;
    }

    void DeleteInboxItem(int itemId) {
        inboxScrollView.Q<VisualElement>("InboxItem" + itemId).RemoveFromHierarchy();
    }
}

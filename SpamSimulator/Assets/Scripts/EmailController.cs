using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class EmailController : MonoBehaviour
    {
        private VisualElement emailRoot;
        private List<Email> inbox;
        private Email currentEmail;

        public UIDocument uI;

        // Start is called before the first frame update
        void Start()
        {
            inbox = new List<Email>()
            {
                Email.LoadEmail("testEmail.json")
            };

            emailRoot = uI.rootVisualElement.Q<VisualElement>(name: "EmailUI");
            emailRoot.Add(inbox[0].InstantiateEmail());
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

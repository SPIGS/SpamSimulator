using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class PopUpVirus : Virus
    {
        private int minimumPopups = 5;
        private int maximumPopups = 9;
        private float timeElapsed = 0.0f;
        private float minDelayBetweenNewPopupsSec = 30.0f;
        private float maxDelayBetweenNewPopupsSec = 60.0f;
        private float currentDelayBetweenNewPopupsSec = 30.0f;
        private VisualTreeAsset popupWindowTemplate;

        public PopUpVirus(VirusType virusType, VisualElement root, GameController gameController,
                     EmailController emailController, SoundController soundController, 
                     ModalWindowController windowController, StoryController storyController, VisualTreeAsset popupWindow): 
            base(VirusType.POPUP, root, gameController, emailController, soundController, windowController, storyController)
        {
            this.popupWindowTemplate = popupWindow; 
        }

        public override void Update() {
            timeElapsed+= Time.deltaTime;
            if (timeElapsed >= currentDelayBetweenNewPopupsSec) {
                CreatePopup();
                timeElapsed = 0.0f;
                currentDelayBetweenNewPopupsSec = Random.Range(minDelayBetweenNewPopupsSec, maxDelayBetweenNewPopupsSec);
            }    
        }

        public override void TriggerVirus() {
            int numberOfPopups = Random.Range(minimumPopups, maximumPopups);
            for (int i = 0; i < numberOfPopups; i ++) {
                CreatePopup();
            }
        }

        private void CreatePopup () {
            float xPos = Random.Range(0.0f, Screen.width - 500.0f);
            float yPos = Random.Range(0.0f, Screen.height - 500.0f);
            windowController.CreateModal(popupWindowTemplate, xPos, yPos, 500, 500, "You've been gnomed!");
            soundController.PlaySoundEffect("Trash Email");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class ExtraSpamVirus : Virus
    {
        private int minimumSpam = 3;
        private int maximumSpam = 7;
        private bool makeSpam = false;
        private int numberOfSpam = 3;
        private float delayBetweenSpamSec = 0.75f;
        private float timeElapsed = 0.0f;

        public ExtraSpamVirus(VisualElement root, GameController gameController,
                     EmailController emailController, SoundController soundController, 
                     ModalWindowController windowController, StoryController storyController): 
            base(VirusType.SEND_EXTRA_SPAM, root, gameController, emailController, soundController, windowController, storyController) {}

        public override void Update() {
            if (makeSpam) {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= delayBetweenSpamSec) {
                    string spamEmail = storyController.GetSpamEmail();
                    emailController.AddEmail(spamEmail);
                    gameController.UpdateStorage();
                    timeElapsed = 0.0f;
                    numberOfSpam--;
                    if (numberOfSpam <= 0) {
                        makeSpam = false;
                    }
                }
            }
        }
        public override void TriggerVirus() {
            numberOfSpam = Random.Range(minimumSpam, maximumSpam);
            makeSpam = true;
        }
    }
}
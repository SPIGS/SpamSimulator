using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class SoundVirus : Virus
    {
        private string newEmailVirusSound = "YouveGotMail";
        private string deleteEmailVirusSound = "Zap";
        private string passEmailVirusSound = "Whirl";
        private VisualTreeAsset thankyouPopupTemplate;

        public SoundVirus(VisualElement root, GameController gameController,
                     EmailController emailController, SoundController soundController,
                     ModalWindowController windowController, StoryController storyController, VisualTreeAsset popupTemplate) :
            base(VirusType.SOUND, root, gameController, emailController, soundController, windowController, storyController)
        { 
            thankyouPopupTemplate = popupTemplate;
        }

        public override void Update()
        {
            
        }
        public override void TriggerVirus()
        {
            emailController.newEmailSoundEffect = newEmailVirusSound;
            emailController.deleteEmailSoundEffect = deleteEmailVirusSound;
            emailController.passEmailSoundEffect = passEmailVirusSound;
            windowController.CreateModal(
                thankyouPopupTemplate,
                (Screen.width/2) - 250,
                (Screen.height/2) - 250,
                500,
                300,
                "Thank you!"
            );
        }
    }
}
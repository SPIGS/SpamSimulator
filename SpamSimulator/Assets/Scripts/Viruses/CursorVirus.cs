using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpamSim
{
    public class CursorVirus : Virus
    {

        private Texture2D virusCursor;
        private VisualTreeAsset thankyouPopupTemplate;

        public CursorVirus(VisualElement root, GameController gameController,
                     EmailController emailController, SoundController soundController,
                     ModalWindowController windowController, StoryController storyController, VisualTreeAsset thankyouPopupTemplate, Texture2D cursor) :
            base(VirusType.CURSOR, root, gameController, emailController, soundController, windowController, storyController)
        { 
            this.thankyouPopupTemplate = thankyouPopupTemplate;
            virusCursor = cursor;
        }

        public override void Update()
        {
            
        }
        public override void TriggerVirus()
        {
            UnityEngine.Cursor.SetCursor(virusCursor, new Vector2(0,5), CursorMode.Auto);
            windowController.CreateModal(
                thankyouPopupTemplate,
                (Screen.width / 2) - 250,
                (Screen.height / 2) - 250,
                500,
                300,
                "Thank you!"
            );
        }
    }
}
using UnityEngine;
using System.Collections;

public class UiButtonClicker : MonoBehaviour {
    public void ButtonClick(GameObject button)
    {
        //determine which button was clicked then do appropriate function in gameController

        //probably need a null check in here somewhere
        string buttonName = button.name;

        //Vending Machine Related
        if (buttonName.Contains("Vend")){
            //Determine which it was, this needs to be more generic in the future but for now
            //this is fine just to get something working quickly
            int buttonIndex = -1;
            switch (buttonName){
                case "VendLeftOption":buttonIndex = 0;break;
                case "VendMidOption":buttonIndex = 1;break;
                case "VendRightOption":buttonIndex = 2;break;
                case "VendConfirm": buttonIndex = 100; break;
                default:buttonIndex = -1;break;
            }
            //call the game controller function
            if (buttonIndex != -1){
                GameController.vendor.VendingMachineButtonClick(buttonIndex);
            }
        }

        //other things related
    }
}

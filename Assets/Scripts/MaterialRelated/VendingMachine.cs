using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class VendingMachine {
    private GameObject vendPanel;
    private List<PortalMaterial> visiblePMs = new List<PortalMaterial>();
    private PortalMaterial displayMaterial;
    public int variableMachineSize = 3;

    //contructor
    public VendingMachine(GameObject panel)
    {
        this.vendPanel = panel;
    }

    //Return the requested material index, then determine if it needs to be replaced
    public PortalMaterial AccessVendingMachine(int vpmIndex, bool replace)
    {
        PortalMaterial requestedPM = null;
        if (vpmIndex >= 0 && visiblePMs.Count - 1 >= vpmIndex){
            requestedPM = visiblePMs[vpmIndex];
            if (replace){
                //generate a new random material and stick it in the list
                PortalMaterial replacementMaterial = GameController.mh.GenerateRandomMaterial();
                visiblePMs[vpmIndex] = replacementMaterial;
            }
        }
        
        return requestedPM;
    }

    //refresh the vending machine with x new random materials
    public void RefreshVendingMachine()
    {
        for (int i=0; i<variableMachineSize; i++){
            PortalMaterial tempMaterial = GameController.mh.GenerateRandomMaterial();
            visiblePMs.Add(tempMaterial);
        }

        //display the current choices
        DisplayPMChoices();
    }

    //Displays the available vending machine choices
    public void DisplayPMChoices()
    {
        //this needs to not be hard coded so it can be scalable later
        if (vendPanel != null)
        {
            Text left, mid, right;
            left = vendPanel.transform.Find("VendLeftOption/LeftMaterialText").GetComponent<Text>();
            mid = vendPanel.transform.Find("VendMidOption/MidMaterialText").GetComponent<Text>();
            right = vendPanel.transform.Find("VendRightOption/RightMaterialText").GetComponent<Text>();

            if (left != null && mid != null && right != null)
            {
                left.text = AccessVendingMachine(0, false).toString();
                mid.text = AccessVendingMachine(1, false).toString();
                right.text = AccessVendingMachine(2, false).toString();
            }
        }
    }

    //button was clicked, called from ui button clicker
    public void VendingMachineButtonClick(int buttonIndex)
    {
        
        if(buttonIndex == 100){
            //confirmed material was clicked
            ConfirmMaterialSelection();
        }
        else {
            //a material selector was clicked
            displayMaterial = AccessVendingMachine(buttonIndex, false);
            DisplayMaterialDescription();
        }
    }

    //displays the material information
    public void DisplayMaterialDescription()
    {
       Text displayText = vendPanel.transform.Find("VendDescriptionArea/PMDescriptText").GetComponent<Text>();
        if(displayText != null)
            displayText.text = displayMaterial.toString();
    }

    //use the selected material for the next portal stage
    public void ConfirmMaterialSelection()
    {
        //clear description
        
    }
}

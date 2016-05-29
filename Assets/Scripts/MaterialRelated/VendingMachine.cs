using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        if (vpmIndex >= 0 && visiblePMs.Count <= vpmIndex){
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
        Transform test = vendPanel.transform.FindChild("LeftMaterialText");
        Debug.Log(test.name);

        //this needs to not be hard coded so it can be scalable later
        GUIText left, mid, right;
        left = vendPanel.transform.FindChild("LeftMaterialText").GetComponent<GUIText>();
        mid = vendPanel.transform.Find("MidMaterialText").GetComponent<GUIText>();
        right = vendPanel.transform.Find("RightMaterialText").GetComponent<GUIText>();

        left.text = AccessVendingMachine(0, false).toString();
        mid.text = AccessVendingMachine(1, false).toString();
        right.text = AccessVendingMachine(2, false).toString();
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
       TextMesh displayText = vendPanel.transform.Find("PMDescriptText").GetComponent<TextMesh>();
       displayText.text = displayMaterial.toString();
    }

    //use the selected material for the next portal stage
    public void ConfirmMaterialSelection()
    {

    }
}

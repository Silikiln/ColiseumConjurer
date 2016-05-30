using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class VendingMachine {
    private GameObject vendPanel;
    private GameObject portalPanel;
    private List<PortalMaterial> visiblePMs = new List<PortalMaterial>();
    private PortalMaterial displayMaterial;
    private int displayMaterialIndex = -1;
    public int variableMachineSize = 3;
    private Text displayText, left, mid, right;
    private int maxPortalSize = 10;
    private int stageIndex = 1;
    //contructor
    public VendingMachine(GameObject pPanel, GameObject vPanel)
    {
        this.portalPanel = pPanel;
        this.vendPanel = vPanel;
        displayText = vendPanel.transform.Find("VendDescriptionArea/PMDescriptText").GetComponent<Text>();
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
            displayMaterialIndex = buttonIndex;
            DisplayMaterialDescription();
        }
    }

    //displays the material information
    public void DisplayMaterialDescription()
    {
        if(displayText != null)
            displayText.text = displayMaterial.toString();
    }

    //use the selected material for the next portal stage
    public void ConfirmMaterialSelection()
    {
        if (displayText != null && displayMaterial != null && displayMaterialIndex != -1 && stageIndex <= maxPortalSize){
            //add to the list
            //Debug.Log("Display material index: " + displayMaterialIndex);
            GameController.stageMaterials.Add(AccessVendingMachine(displayMaterialIndex, true));
            //update the vending display
            DisplayPMChoices();
            //update the portal display
            DisplayPortalChoices();
        }
        //clear material
        ClearDisplayMaterial();
    }

    public void DisplayPortalChoices()
    {
        //remove everything from portalPanel
        foreach (Transform child in portalPanel.transform){
            GameObject.Destroy(child.gameObject);
        }
        int yPos = 0;
        stageIndex = 1;
        //access the list of materials, loop over each one and display
        foreach (PortalMaterial tempMaterial in GameController.stageMaterials){
            //this 10 needs to be changed for scaleability
            if(stageIndex <= maxPortalSize){
                yPos += 40;
                //instantiate a portalblock prefab
                GameObject portalStageInfo = GameObject.Instantiate(Resources.Load("PortalScreen/PortalStageInfo")) as GameObject;
                if (portalStageInfo != null){
                    //set rect transform parent
                    portalStageInfo.GetComponent<RectTransform>().SetParent(portalPanel.GetComponent<RectTransform>(), false);
                    portalStageInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yPos, 0);
                    //set text to be the tempMaterial info
                    portalStageInfo.transform.Find("StageText").GetComponent<Text>().text = "Stage " + stageIndex + " : " + tempMaterial.toString();
                    stageIndex++;
                }
            }
        }
    }

    public void ClearDisplayMaterial()
    {
        displayText.text = "";
        displayMaterial = null;
        displayMaterialIndex = -1;
    }
}

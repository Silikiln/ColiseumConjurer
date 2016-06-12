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
    private Text displayText;
    private char[] colorArray = {'R','G','B'};
    private Text[] visibleMaterialsText = new Text[3];
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
            //determine if we want to generate a new random material and stick it in the list
            if (replace){
                //check the color
                char replacementMaterialColor = requestedPM.color;
                if (requestedPM.color == 'M'){
                    //we want to generate a color based on the index instead, this may need to be changed to something else later.
                    //as of now, we could just run this switch everytime to always generate the replacement by index
                    switch(vpmIndex)
                    {
                        case 0:
                            replacementMaterialColor = 'R';
                            break;
                        case 1:
                            replacementMaterialColor = 'G';
                            break;
                        case 2:
                            replacementMaterialColor = 'B';
                            break;
                    }
                }
                PortalMaterial replacementMaterial = GameController.mh.GenerateRandomMaterial(replacementMaterialColor);
                visiblePMs[vpmIndex] = replacementMaterial;
            }
        }
        
        return requestedPM;
    }

    //refresh the vending machine with x new random materials
    public void RefreshVendingMachine()
    {
        foreach (char color in colorArray) {
            PortalMaterial tempMaterial = GameController.mh.GenerateRandomMaterial(color);
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
            visibleMaterialsText[0] = vendPanel.transform.Find("VendLeftOption/LeftMaterialText").GetComponent<Text>();
            visibleMaterialsText[1] = vendPanel.transform.Find("VendMidOption/MidMaterialText").GetComponent<Text>();
            visibleMaterialsText[2] = vendPanel.transform.Find("VendRightOption/RightMaterialText").GetComponent<Text>();

            if (visibleMaterialsText[0] != null && visibleMaterialsText[1] != null && visibleMaterialsText[2] != null)
            {
                for (int x=0; x<visibleMaterialsText.Length; x++){
                    visibleMaterialsText[x].text = AccessVendingMachine(x, false).toString();
                    visibleMaterialsText[x].color = GameController.mh.GetMaterialColor(AccessVendingMachine(x, false).color);
                }
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
            displayText.text = displayMaterial.toString() + "\n" + "Color: " + displayMaterial.color + "\n" + displayMaterial.GetAllEffects();
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
                    portalStageInfo.transform.Find("StageText").GetComponent<Text>().color = GameController.mh.GetMaterialColor(tempMaterial.color);
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

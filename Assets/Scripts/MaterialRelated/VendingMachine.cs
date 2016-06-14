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
    private char[] colorArray = {'R','G','B'};
    private int maxPortalSize = 10;
    private int stageIndex = 1;

    Image[] visibleMaterialImages = new Image[3];
    Button[] materialSelectButtons = new Button[3];
    Text materialNameText, materialDescriptionText, materialEffectSectionText, materialEffectTypeText, materialEffectValueText;

    //contructor
    public VendingMachine(GameObject pPanel, GameObject vPanel)
    {
        this.portalPanel = pPanel;
        this.vendPanel = vPanel;

        string vendDetailsParent = "VendDetailsPanel/Container/";
        materialNameText = vendPanel.transform.Find(vendDetailsParent + "NameText").GetComponent<Text>();
        materialDescriptionText = vendPanel.transform.Find(vendDetailsParent + "DescriptionText").GetComponent<Text>();
        materialEffectSectionText = vendPanel.transform.Find(vendDetailsParent + "EffectSectionText").GetComponent<Text>();
        materialEffectTypeText = vendPanel.transform.Find(vendDetailsParent + "EffectTypeText").GetComponent<Text>();
        materialEffectValueText = vendPanel.transform.Find(vendDetailsParent + "EffectValueText").GetComponent<Text>();

        visibleMaterialImages[0] = vendPanel.transform.Find("VendLeftOption/Image").GetComponent<Image>();
        visibleMaterialImages[1] = vendPanel.transform.Find("VendMidOption/Image").GetComponent<Image>();
        visibleMaterialImages[2] = vendPanel.transform.Find("VendRightOption/Image").GetComponent<Image>();

        materialSelectButtons[0] = vendPanel.transform.Find("VendLeftOption").GetComponent<Button>();
        materialSelectButtons[1] = vendPanel.transform.Find("VendMidOption").GetComponent<Button>();
        materialSelectButtons[2] = vendPanel.transform.Find("VendRightOption").GetComponent<Button>();

        ClearDisplayMaterial();
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
                visiblePMs[vpmIndex] = PortalMaterial.RandomMaterial;
                //visiblePMs[vpmIndex] = PortalMaterial.RandomMaterialWithColor(replacementMaterialColor);
            }
        }
        
        return requestedPM;
    }

    //refresh the vending machine with x new random materials
    public void RefreshVendingMachine()
    {
        foreach (char color in colorArray) {
            PortalMaterial tempMaterial = PortalMaterial.RandomMaterialWithColor(color);
            visiblePMs.Add(tempMaterial);
        }

        //display the current choices
        DisplayPMChoices();
    }

    //Displays the available vending machine choices
    public void DisplayPMChoices()
    {
        for (int x = 0; x < visibleMaterialImages.Length; x++)
        {
            visibleMaterialImages[x].color = PortalMaterial.GetMaterialColor(AccessVendingMachine(x, false).color);
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
            if (displayMaterialIndex > -1)
                materialSelectButtons[displayMaterialIndex].image.color = materialSelectButtons[displayMaterialIndex].colors.normalColor;

            //a material selector was clicked
            materialSelectButtons[buttonIndex].image.color = materialSelectButtons[buttonIndex].colors.pressedColor;

            displayMaterial = AccessVendingMachine(buttonIndex, false);
            displayMaterialIndex = buttonIndex;
            DisplayMaterialDescription();
        }
    }

    //displays the material information
    public void DisplayMaterialDescription()
    {
        Color displayColor = PortalMaterial.GetMaterialColor(displayMaterial.color);

        materialNameText.text = displayMaterial.MaterialName;
        materialNameText.color = displayColor;

        //materialDescriptionText.text = displayMaterial.Description;

        materialEffectSectionText.enabled = true;
        materialEffectTypeText.text = displayMaterial.EffectTypeNames;
        materialEffectValueText.text = displayMaterial.EffectFormattedValues;

        /*
        if(displayText != null)
            displayText.text =  + "\n" + "Color: " + displayMaterial.color + "\n" + displayMaterial.GetAllEffects();
        */
    }

    //use the selected material for the next portal stage
    public void ConfirmMaterialSelection()
    {
        if (displayMaterial != null && displayMaterialIndex != -1 && stageIndex <= maxPortalSize){
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
            //this needs to be changed for scaleability
            if(stageIndex <= maxPortalSize){
                yPos += 40;
                //instantiate a portalblock prefab
                GameObject portalStageInfo = GameObject.Instantiate(Resources.Load("PortalScreen/PortalStageInfo")) as GameObject;
                if (portalStageInfo != null){
                    //set rect transform parent
                    portalStageInfo.GetComponent<RectTransform>().SetParent(portalPanel.GetComponent<RectTransform>(), false);
                    portalStageInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yPos, 0);
                    //set text to be the tempMaterial info
                    portalStageInfo.transform.Find("StageText").GetComponent<Text>().text = "Stage " + stageIndex + " : " + tempMaterial.MaterialName;
                    portalStageInfo.transform.Find("StageText").GetComponent<Text>().color = PortalMaterial.GetMaterialColor(tempMaterial.color);
                    stageIndex++;
                }
            }
        }
    }

    public void ClearDisplayMaterial()
    {
        if (displayMaterialIndex > -1)
            materialSelectButtons[displayMaterialIndex].image.color = materialSelectButtons[displayMaterialIndex].colors.normalColor;

        materialNameText.text = "";
        materialDescriptionText.text = "";
        materialEffectSectionText.enabled = false;
        materialEffectTypeText.text = "";
        materialEffectValueText.text = "";
        displayMaterial = null;
        displayMaterialIndex = -1;
    }
}

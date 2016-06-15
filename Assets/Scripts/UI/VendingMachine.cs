using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour {
    // TODO: Add option for more than 3 materials at a time
    //public int MaterialSlotCount = 3;

    Button[] materialSelectButtons = new Button[3];
    Image[] visibleMaterialImages = new Image[3];
    
    Text materialNameText, materialDescriptionText, materialEffectSectionText, materialEffectTypeText, materialEffectValueText;

    PortalMaterial[] AvailableMaterials = new PortalMaterial[3];
    int SelectedMaterialIndex = -1;

    private PortalMaterial DisplayMaterial { get { return SelectedMaterialIndex > -1 ? AvailableMaterials[SelectedMaterialIndex] : null; } }

    void Start()
    {
        // Find material details UI
        string vendDetailsParent = "VendDetailsPanel/Container/";
        materialNameText = transform.Find(vendDetailsParent + "NameText").GetComponent<Text>();
        materialDescriptionText = transform.Find(vendDetailsParent + "DescriptionText").GetComponent<Text>();
        materialEffectSectionText = transform.Find(vendDetailsParent + "EffectSectionText").GetComponent<Text>();
        materialEffectTypeText = transform.Find(vendDetailsParent + "EffectTypeText").GetComponent<Text>();
        materialEffectValueText = transform.Find(vendDetailsParent + "EffectValueText").GetComponent<Text>();

        // Find material select buttons
        materialSelectButtons[0] = transform.Find("VendLeftOption").GetComponent<Button>();
        materialSelectButtons[1] = transform.Find("VendMidOption").GetComponent<Button>();
        materialSelectButtons[2] = transform.Find("VendRightOption").GetComponent<Button>();

        // Find material select button images
        visibleMaterialImages[0] = transform.Find("VendLeftOption/Image").GetComponent<Image>();
        visibleMaterialImages[1] = transform.Find("VendMidOption/Image").GetComponent<Image>();
        visibleMaterialImages[2] = transform.Find("VendRightOption/Image").GetComponent<Image>();

        ClearDisplay();
        RefreshVendingMachine();
    }

    // Replace each material with a new one
    public void RefreshVendingMachine()
    {
        for (int i = 0; i < AvailableMaterials.Length; i++)
        {
            AvailableMaterials[i] = PortalMaterial.RandomMaterial;
            visibleMaterialImages[i].color = AvailableMaterials[i].VisualColor;
        }
    }

    // Select the material at the given index
    // Called by the material select buttons
    public void SelectMaterial(int index)
    {
        if (index > AvailableMaterials.Length - 1) return;
        UnselectCurrentButton();
        
        SelectedMaterialIndex = index;
        UpdateMaterialDescription();
    }

    // Confirm the current selection
    // Called by the confirm button
    public void ConfirmSelection()
    {
        if (DisplayMaterial == null || SelectedMaterialIndex == -1 
            || GameController.Instance.CurrentStage > GameController.Instance.MaxPortalSize) return;
        
        // Add the material to the portal
        GameController.Instance.AddMaterial(DisplayMaterial);

        // Generate a new random material to replace the old one
        AvailableMaterials[SelectedMaterialIndex] = PortalMaterial.RandomMaterial;

        // Update the image color
        visibleMaterialImages[SelectedMaterialIndex].color = AvailableMaterials[SelectedMaterialIndex].VisualColor;

        // Clear the display
        ClearDisplay();
    }

    // If there is a selected button, unselect it
    void UnselectCurrentButton()
    {
        if (SelectedMaterialIndex > -1)
            SetSelectButtonColor(false);
    }

    // Change the color of a select button
    void SetSelectButtonColor(int index, bool pressed)
    {
        materialSelectButtons[SelectedMaterialIndex].image.color =
            pressed ? 
            materialSelectButtons[SelectedMaterialIndex].colors.pressedColor :
            materialSelectButtons[SelectedMaterialIndex].colors.normalColor;
    }

    // Change the color of the current button
    void SetSelectButtonColor(bool pressed) { SetSelectButtonColor(SelectedMaterialIndex, pressed); }

    //displays the material information
    public void UpdateMaterialDescription()
    {
        SetSelectButtonColor(true);
        Color displayColor = DisplayMaterial.VisualColor;

        materialNameText.text = DisplayMaterial.Name;
        materialNameText.color = displayColor;

        materialDescriptionText.text = DisplayMaterial.Description;

        materialEffectSectionText.enabled = true;
        materialEffectTypeText.text = DisplayMaterial.EffectTypeNames;
        materialEffectValueText.text = DisplayMaterial.EffectFormattedValues;
    }

    // Unselect the current button and clear the description panel
    public void ClearDisplay()
    {
        UnselectCurrentButton();

        materialNameText.text = "";
        materialDescriptionText.text = "";
        materialEffectSectionText.enabled = false;
        materialEffectTypeText.text = "";
        materialEffectValueText.text = "";
        SelectedMaterialIndex = -1;
    }
}

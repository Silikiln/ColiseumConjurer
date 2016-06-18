using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour {
    // TODO: Add option for more than 3 materials at a time
    //public int MaterialSlotCount = 3;

    Button[] materialSelectButtons = new Button[3];
    Image[] visibleMaterialImages = new Image[3];
    
    PortalMaterial[] AvailableMaterials = new PortalMaterial[3];
    int SelectedMaterialIndex = -1;

    private PortalMaterial DisplayMaterial { get { return SelectedMaterialIndex > -1 ? AvailableMaterials[SelectedMaterialIndex] : null; } }

    void Start()
    {
        // Find material select buttons
        materialSelectButtons[0] = transform.Find("VendLeftOption").GetComponent<Button>();
        materialSelectButtons[1] = transform.Find("VendMidOption").GetComponent<Button>();
        materialSelectButtons[2] = transform.Find("VendRightOption").GetComponent<Button>();

        // Find material select button images
        visibleMaterialImages[0] = transform.Find("VendLeftOption/Image").GetComponent<Image>();
        visibleMaterialImages[1] = transform.Find("VendMidOption/Image").GetComponent<Image>();
        visibleMaterialImages[2] = transform.Find("VendRightOption/Image").GetComponent<Image>();

        RefreshVendingMachine();
    }

    // Replace each material with a new one
    public void RefreshVendingMachine()
    {
        for (int i = 0; i < AvailableMaterials.Length; i++)
        {
            AvailableMaterials[i] = PortalMaterial.RandomMaterial;
            visibleMaterialImages[i].color = AvailableMaterials[i].VisualColor;
            //visibleMaterialImages[i].sprite = AvailableMaterials[i].Image;
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
        ClearDisplayPanel();
    }

    // If there is a selected button, unselect it
    public void UnselectCurrentButton()
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
        GameController.Instance.displayPanel.UpdateDisplayPanel(DisplayMaterial);
    }

    public void ClearDisplayPanel()
    {
        UnselectCurrentButton();
        SelectedMaterialIndex = -1;
        GameController.Instance.displayPanel.ClearDisplay();
    }
}

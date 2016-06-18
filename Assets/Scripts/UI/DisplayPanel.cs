using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayPanel : PanelUtility {
    Text materialNameText, materialDescriptionText, materialEffectSectionText, materialEffectTypeText, materialEffectValueText;

    void Start()
    {
        string vendDetailsParent = "Container/";
        materialNameText = transform.Find(vendDetailsParent + "NameText").GetComponent<Text>();
        materialDescriptionText = transform.Find(vendDetailsParent + "DescriptionText").GetComponent<Text>();
        materialEffectSectionText = transform.Find(vendDetailsParent + "EffectSectionText").GetComponent<Text>();
        materialEffectTypeText = transform.Find(vendDetailsParent + "EffectTypeText").GetComponent<Text>();
        materialEffectValueText = transform.Find(vendDetailsParent + "EffectValueText").GetComponent<Text>();

        ClearDisplay();
    }
    public void UpdateDisplayPanel(PortalMaterial displayMaterial, bool fromPortal = false)
    {
        materialNameText.text = displayMaterial.Name;
        materialNameText.color = displayMaterial.VisualColor;

        materialDescriptionText.text = displayMaterial.Description;

        materialEffectSectionText.enabled = true;
        materialEffectTypeText.text = displayMaterial.EffectTypeNames;
        materialEffectValueText.text = displayMaterial.EffectFormattedValues;

        //clear selected vend button
        if(fromPortal)
            GameController.Instance.vendPanel.UnselectCurrentButton();
    }

    public void ClearDisplay()
    {
        materialNameText.text = "";
        materialDescriptionText.text = "";
        materialEffectSectionText.enabled = false;
        materialEffectTypeText.text = "";
        materialEffectValueText.text = "";
    }
}

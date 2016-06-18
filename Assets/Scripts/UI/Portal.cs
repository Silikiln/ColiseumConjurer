using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Portal : MonoBehaviour {
    public float TopMargin = 40;

    List<SelectButton> infoButtons = new List<SelectButton>();
    int selectedButton = -1;

    public void AddMaterial(int currentStage, PortalMaterial material)
    {
        // Create new info panel
        RectTransform portalStageInfo = 
            (Instantiate(Resources.Load("PortalScreen/PortalStageInfo")) as GameObject).GetComponent<RectTransform>();
        portalStageInfo.SetParent(transform, false);
        portalStageInfo.anchoredPosition = new Vector3(0, TopMargin * currentStage);

        // Update info panel text for new material
        Text portalStageText = portalStageInfo.Find("StageText").GetComponent<Text>();
        portalStageText.text = "Stage " + currentStage + " : " + material.Name;
        portalStageText.color = material.VisualColor;

        //update portal button
        infoButtons.Add(portalStageInfo.GetComponent<SelectButton>());
        infoButtons[currentStage - 1].targetGraphic = portalStageInfo.GetComponent<Image>();
        infoButtons[currentStage - 1].onClick.AddListener(delegate { InfoSelected(currentStage - 1); });
    }

    public void UnselectCurrentButton()
    {
        if (selectedButton == -1) return;
        infoButtons[selectedButton].Select(false);
        selectedButton = -1;
    }

    public void InfoSelected(int stage)
    {
        UnselectCurrentButton();
        selectedButton = stage;
        GameController.Instance.displayPanel.UpdateDisplayPanel(GameController.Instance.stageMaterials[stage], true);
    }

    public void ClearMaterials()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }

}

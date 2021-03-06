﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Portal : MonoBehaviour {
    public float TopMargin = 40;

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
    }

    public void ClearMaterials()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }
}

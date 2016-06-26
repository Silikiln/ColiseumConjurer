using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    //panels
    public TrialPanel trialPanel;
    public Portal Portal;
    public Canvas mainCanvas;
    public DisplayPanel displayPanel;
    public VendingMachine vendPanel;
    public Slider stableSlider;

    public int MaxPortalSize = 10;
    public List<PortalMaterial> stageMaterials = new List<PortalMaterial>();
    public float portalStability = 0;
    public int CurrentStage { get { return stageMaterials.Count + 1; } }

    private Text stableSliderText;

    void Start () {
        Instance = this;
        stableSliderText = stableSlider.transform.Find("stabilityText").GetComponent<Text>();
    }

    public void AddMaterial(PortalMaterial material)
    {
        stageMaterials.Add(material);
        Portal.AddMaterial(stageMaterials.Count, material);

        foreach (MaterialEffect effect in material)
            effect.ApplyEffect();

        //update the stability meter
        CalculatePortalStability();

        //load a new trial
        Type trialToLoad = TrialHandler.Instance.RandomTrial;
        if (stageMaterials.Count == MaxPortalSize)
            trialToLoad = FindBossToSummon();

        TrialHandler.Instance.LoadEvent(trialToLoad);

        //Display a new panel with trial information and start count down
        trialPanel.UpdatePanelDisplay(TrialHandler.CurrentTrial);
        trialPanel.TogglePanel();

        //start trial scene
        trialPanel.StartTimer();

    }

    public void CalculatePortalStability()
    {
        portalStability = 0;
        foreach (PortalMaterial pm in stageMaterials)
           portalStability += (int)pm.Color;
        stableSlider.value = portalStability/160;
        stableSliderText.text = "Portal Stability: " + ((portalStability / 160)*100) + "%";
    }

    public Type FindBossToSummon()
    {
        Type bt;
        CalculatePortalStability();
        int tempPortalStability = (int)portalStability;
        while (!Boss.PossibleBosses.TryGetValue(tempPortalStability--, out bt));
        return bt;
    }
}

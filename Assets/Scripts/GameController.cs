using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    //panels
    public TrialPanel trialPanel;
    public Portal Portal;
    public Canvas mainCanvas;
    public DisplayPanel displayPanel;
    public VendingMachine vendPanel;

    public int MaxPortalSize = 10;
    public List<PortalMaterial> stageMaterials = new List<PortalMaterial>();
    public int portalStability = 0;
    public int CurrentStage { get { return stageMaterials.Count + 1; } }

    void Start () {
        Instance = this;
    }

    public void AddMaterial(PortalMaterial material)
    {
        stageMaterials.Add(material);
        Portal.AddMaterial(stageMaterials.Count, material);

        foreach (MaterialEffect effect in material)
            effect.ApplyEffect();

        //load a new trial
        Type RandomTrial = TrialHandler.Instance.RandomTrial;
        TrialHandler.Instance.LoadEvent(RandomTrial);

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
    }

    public Type FindBossToSummon(int stability)
    {
        Type bt;
        while (Boss.PossibleBosses.TryGetValue(stability--, out bt)) ;
        return bt;
    }
}

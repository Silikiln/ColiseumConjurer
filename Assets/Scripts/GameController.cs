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

        EnemyController.CurrentEnemyCount++;

        //load a new trial
        Type trialToLoad = TrialHandler.Instance.RandomTrial;
        if (stageMaterials.Count == MaxPortalSize)
            trialToLoad = FindBossToSummon();

        TrialHandler.Instance.LoadTrial(trialToLoad);

        //Display a new panel with trial information and start count down
        trialPanel.UpdatePanelDisplay(TrialHandler.CurrentTrial);
        trialPanel.TogglePanel();

        //start trial scene
        trialPanel.StartTimer();

    }

    public void GameOver()
    {
        vendPanel.DisableConfirm();
        Debug.Log("Game Over!");
    }

    public void CalculatePortalStability()
    {
        portalStability = 0;
        foreach (PortalMaterial pm in stageMaterials)
           portalStability += (int)pm.Color;
    }

    public Type FindBossToSummon()
    {
        Type bt;
        CalculatePortalStability();
        int tempPortalStability = portalStability;
        while (!Boss.PossibleBosses.TryGetValue(tempPortalStability--, out bt));
        return bt;
    }
}

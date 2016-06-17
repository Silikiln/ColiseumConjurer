using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }
    public TrialPanel trialPanel;
    public Portal Portal;
    public Canvas mainCanvas;

    public int MaxPortalSize = 10;
    List<PortalMaterial> stageMaterials = new List<PortalMaterial>();

    public int CurrentStage { get { return stageMaterials.Count + 1; } }

    void Start () {
        Instance = this;
    }

    public void AddMaterial(PortalMaterial material)
    {
        stageMaterials.Add(material);
        Portal.AddMaterial(stageMaterials.Count, material);

        //load a new trial
        Type RandomTrial = TrialHandler.Instance.RandomTrial;
        TrialHandler.Instance.LoadEvent(RandomTrial);

        //Display a new panel with trial information and start count down
        trialPanel.UpdatePanelDisplay(TrialHandler.CurrentTrial);
        trialPanel.TogglePanel();

        //start trial scene
        trialPanel.StartTimer();

    }
}

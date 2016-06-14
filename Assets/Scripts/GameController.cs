using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    public Portal Portal;
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
    }
}

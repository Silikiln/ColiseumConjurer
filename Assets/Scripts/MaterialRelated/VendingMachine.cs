using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VendingMachine : MonoBehaviour {
    private List<PortalMaterial> visiblePMs = new List<PortalMaterial>();
    public int variableMachineSize = 3;

    //this object will need to be in the game manager class but for now it is here
    MaterialHandler mh = new MaterialHandler();

    void Start()
    {
        //this would be in the game manager class
        mh.ImportMaterials();

        RefreshVendingMachine();
    }

    //Return the requested material index, then determine if it needs to be replaced
    public PortalMaterial AccessVendingMachine(int vpmIndex, bool replace)
    {
        PortalMaterial requestedPM = null;
        if (vpmIndex >= 0 && visiblePMs.Count <= vpmIndex){
            requestedPM = visiblePMs[vpmIndex];
            if (replace){
                //generate a new random material and stick it in the list
                PortalMaterial replacementMaterial = mh.GenerateRandomMaterial();
                visiblePMs[vpmIndex] = replacementMaterial;
            }
        }
        
        return requestedPM;
    }

    //refresh the vending machine with x new random materials
    public void RefreshVendingMachine()
    {
        for (int i=0; i<variableMachineSize; i++){
            PortalMaterial replacementMaterial = mh.GenerateRandomMaterial();
            visiblePMs[i] = replacementMaterial;
        }
    }
}

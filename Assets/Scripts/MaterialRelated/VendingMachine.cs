using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VendingMachine {
    private List<PortalMaterial> visiblePMs = new List<PortalMaterial>();
    public int variableMachineSize = 3;

    void Start()
    {

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
                PortalMaterial replacementMaterial = GameManager.mh.GenerateRandomMaterial();
                visiblePMs[vpmIndex] = replacementMaterial;
            }
        }
        
        return requestedPM;
    }

    //refresh the vending machine with x new random materials
    public void RefreshVendingMachine()
    {
        for (int i=0; i<variableMachineSize; i++){
            PortalMaterial tempMaterial = GameManager.mh.GenerateRandomMaterial();
            visiblePMs[i] = tempMaterial;
        }
    }
}

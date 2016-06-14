using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {
    //set by the inspector
    public GameObject vendingPanel;
    public GameObject portalPanel;

    //random other variables
    public static List<PortalMaterial> stageMaterials = new List<PortalMaterial>();
    public static VendingMachine vendor;

    void Start () {
        vendor = new VendingMachine(portalPanel, vendingPanel);
        vendor.RefreshVendingMachine();
        //Debug.Log(mh.GetMaterialByName("PMOne").toString());
        //Debug.Log(mh.GetMaterialByName("PMTwo").toString());
        //Debug.Log(mh.GetMaterialByName("PMThree").toString());

        //Debug.Log(mh.GetMaterialByNumber(0).toString());
        //Debug.Log(mh.GetMaterialByNumber(1).toString());
        //Debug.Log(mh.GetMaterialByNumber(2).toString());

        //Debug.Log(mh.GenerateRandomMaterial().toString());
    }
	
	void Update () {
	
	}

}

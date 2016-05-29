using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    //set by the inspector
    public GameObject vendingPanel;

    //random other variables
    public static MaterialHandler mh = new MaterialHandler();
    public static VendingMachine vendor;

    void Start () {
        //load the materials from the xml
        mh.ImportMaterials();
        vendor = new VendingMachine(vendingPanel);
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

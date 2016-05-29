using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    //set by the inspector

    //random other variables
    public static MaterialHandler mh = new MaterialHandler();

    void Start () {
        //load the materials from the xml
        mh.ImportMaterials();
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

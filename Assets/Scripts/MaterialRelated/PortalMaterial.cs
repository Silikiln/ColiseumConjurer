using UnityEngine;
using System.Collections;

public class PortalMaterial {
    public string materialName { get; set; }
    public string imageLocation { get; set; }

    public string toString(){
        string requestedString = "Name: " + materialName;
        return requestedString;
    }
}

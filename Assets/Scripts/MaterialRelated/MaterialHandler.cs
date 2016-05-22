using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class MaterialHandler : MonoBehaviour {

    private SortedList<string, PortalMaterial> pmDictionary = new SortedList<string, PortalMaterial>();

    //init a look up map for every material and store the names inside an array
    public void ImportMaterials()
    {
        //import some stuff
        using (XmlReader reader = XmlReader.Create("Assets/PortalMaterials.xml"))
        {
            PortalMaterial tempPM = new PortalMaterial();
            reader.ReadToFollowing("Material");
            while (!reader.EOF)
            {
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    switch (reader.Name)
                    {
                        case "Material":
                            tempPM = new PortalMaterial();
                            break;
                        case "Name":
                            tempPM.materialName = reader.ReadInnerXml();
                            break;
                    }
                else
                    switch (reader.Name)
                    {
                        case "Material":
                            pmDictionary.Add(tempPM.materialName, tempPM);
                            break;
                    }
            }
        }
    }

    //select a random material from the dictionary and return it to caller
    public PortalMaterial GenerateRandomMaterial()
    {
        PortalMaterial randomMaterial = new PortalMaterial();
        randomMaterial = GetMaterialByNumber(Random.Range(0, pmDictionary.Count));
        return randomMaterial;
    }

    //get material by providing a name
    public PortalMaterial GetMaterialByName(string givenName)
    {
        PortalMaterial requestedMaterial = pmDictionary[givenName];
        return requestedMaterial;
    }

    //get material by providing a number
    public PortalMaterial GetMaterialByNumber(int givenNumber)
    {
        PortalMaterial requestedMaterial = pmDictionary.Values[givenNumber];
        return requestedMaterial;
    }
}

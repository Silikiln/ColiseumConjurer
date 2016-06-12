using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class MaterialHandler {

    private SortedList<string, PortalMaterial> pmList = new SortedList<string, PortalMaterial>();

    //init a look up map for every material and store the names inside an array
    public void ImportMaterials()
    {
        //import some stuff
        using (XmlReader reader = XmlReader.Create("Assets/PortalMaterials.xml"))
        {
            PortalMaterial tempPM = new PortalMaterial();
            MaterialEffect tempME = new MaterialEffect();
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
                        case "Color":
                            tempPM.color = char.Parse(reader.ReadInnerXml());
                            break;
                        case "Effect":
                            tempME = new MaterialEffect();
                            break;
                        case "Effect-Type":
                            tempME.effectType = int.Parse(reader.ReadInnerXml());
                            break;
                        case "Effect-Value":
                            tempME.effectValue = float.Parse(reader.ReadInnerXml());
                            break;

                    }
                else
                    switch (reader.Name)
                    {
                        case "Material":
                            pmList.Add(tempPM.materialName, tempPM);
                            break;
                        case "Effect":
                            tempPM.AddEffect(tempME);
                            break;
                    }
            }
        }
    }

    //select a random material from the sortedlist and return it to caller
    public PortalMaterial GenerateRandomMaterial()
    {
        PortalMaterial randomMaterial = new PortalMaterial();
        randomMaterial = GetMaterialByNumber(Random.Range(0, pmList.Count));
        return randomMaterial;
    }

    //select a random material of the given color
    public PortalMaterial GenerateRandomMaterial(char color)
    {
        PortalMaterial randomMaterial = new PortalMaterial();
        do
        {
            randomMaterial = GetMaterialByNumber(Random.Range(0, pmList.Count));
        } while (randomMaterial.color != color && randomMaterial.color != 'M');

        return randomMaterial;
    }

    //get material by providing a name
    public PortalMaterial GetMaterialByName(string givenName)
    {
        PortalMaterial requestedMaterial = pmList[givenName];
        return requestedMaterial;
    }

    //get material by providing a number
    public PortalMaterial GetMaterialByNumber(int givenNumber)
    {
        PortalMaterial requestedMaterial = pmList.Values[givenNumber];
        return requestedMaterial;
    }

    public Color GetMaterialColor(char color)
    {
        Color tempColorHex = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        switch (color)
        {
            case 'R':
                tempColorHex = new Color(1.0f, 0.4f, 0.4f, 1.0f);
                break;
            case 'G':
                tempColorHex = new Color(0.4f, 1.0f, 0.4f, 1.0f);
                break;
            case 'B':
                tempColorHex = new Color(0.4f, 0.4f, 1.0f, 1.0f);
                break;
            case 'M':
                tempColorHex = new Color(1.0f, 1.0f, 0.4f, 1.0f);
                break;
        }
        //Debug.Log("Access Color: " + tempColorHex);
        return tempColorHex;
    }
}

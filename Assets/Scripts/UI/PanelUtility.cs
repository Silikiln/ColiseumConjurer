using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelUtility : MonoBehaviour {

    public void TogglePanel()
    {
        //gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }

    public void ClearPanel()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }

    public void ResetText()
    {
        foreach (Text tempText in transform.GetComponents<Text>())
            tempText.text = "";
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LaunchTrial : MonoBehaviour {
    public Text timeText;

    // Use this for initialization
    void Start()
    {
        //launch ritual stuff
        TrialHandler.Instance.BeginTrial(gameObject.transform, timeText);
    }
}

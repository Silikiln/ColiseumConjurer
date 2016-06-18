using UnityEngine;
using System.Collections;

public class LaunchTrial : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //launch ritual stuff
        TrialHandler.Instance.BeginPreloadedEvent(gameObject);
    }
}

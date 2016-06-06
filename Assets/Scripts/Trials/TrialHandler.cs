using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class TrialHandler : MonoBehaviour {

    public static TrialHandler main;
    public static Type[] PossibleEvents = { typeof(PointMove) };

    public Trial currentTrial;

    public Trial RandomTrial { get { return CreateTrial(PossibleEvents[Random.Range(0, PossibleEvents.Length)]); } }

    // Use this for initialization
    void Start() {
        main = this;
        BeginEvent(RandomTrial);
    }

    public Trial CreateTrial(Type t) { return (Trial)Activator.CreateInstance(t); }

    public void BeginEvent(Trial t)
    {
        currentTrial = t;
        currentTrial.Setup();

        Debug.Log("Beginning Trial: " + t.Name);
    }

    public void EventFinished()
    {
        Debug.Log("Trial Complete!");
        BeginEvent(RandomTrial);
    }
}

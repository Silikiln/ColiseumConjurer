using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class TrialHandler : MonoBehaviour {

    public static TrialHandler main;
    public static Type[] PossibleEvents = { typeof(SimonSays) };

    public static Trial CurrentTrial { get { return main.GetComponent<Trial>(); } }

    public Type RandomTrial { get { return PossibleEvents[Random.Range(0, PossibleEvents.Length)]; } }

    // Use this for initialization
    void Start() {
        main = this;
        BeginEvent(RandomTrial);
    }

    public Trial CreateTrial(Type t) { return (Trial)Activator.CreateInstance(t); }

    public void BeginEvent(Type t)
    {
        Debug.Log("Beginning Trial: " + t.Name);

        gameObject.AddComponent(t);
        CurrentTrial.Setup();
    }

    public void EventFinished()
    {
        Debug.Log("Trial Complete!");
        StartCoroutine(SwapEvent());
    }

    IEnumerator SwapEvent()
    {
        Destroy(CurrentTrial);
        yield return new WaitForEndOfFrame();
        BeginEvent(RandomTrial);
    }
}

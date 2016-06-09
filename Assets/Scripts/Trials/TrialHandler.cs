using UnityEngine;
using System.Collections;
using System;
using System.Linq;

using Random = UnityEngine.Random;

public class TrialHandler : MonoBehaviour {

    public static TrialHandler Instance { get; private set; }
    public static Type[] PossibleEvents { get; private set; }

    public static Trial CurrentTrial { get { return Instance.GetComponent<Trial>(); } }

    public Type RandomTrial { get { return PossibleEvents[Random.Range(0, PossibleEvents.Length)]; } }

    // Use this for initialization
    void Start() {
        Instance = this;
        PossibleEvents = typeof(Trial).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Trial))).ToArray();
    }

    void Update()
    {
        if (CurrentTrial == null && Input.GetKeyDown(KeyCode.B))
            BeginEvent(RandomTrial);
    }

    public Trial CreateTrial(Type t) { return (Trial)Activator.CreateInstance(t); }

    public void BeginEvent(Type t)
    {
        gameObject.AddComponent(t);
        Debug.Log("Beginning Trial: " + CurrentTrial.Name);
        CurrentTrial.Setup();
    }

    public void EventFailed()
    {
        CurrentTrial.Cleanup();
        Debug.Log("Trial Failed...");
        StartCoroutine(SwapEvent());
    }

    public void EventFinished()
    {
        CurrentTrial.Cleanup();
        Debug.Log("Trial Complete!");
        StartCoroutine(SwapEvent());
    }

    IEnumerator SwapEvent()
    {
        Destroy(CurrentTrial);
        yield return new WaitForEndOfFrame();
        //BeginEvent(RandomTrial);
    }
}

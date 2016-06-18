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

    #region Material Modifiers

    public static float ObjectiveMultiplier = 1;
    public static float ObjectivesAdded = 0;

    public static float ObjectMultiplier = 1;
    public static float ObjectsAdded = 0;

    public static float TimeLimitMultiplier = 1;
    public static float TimeLimitAdded = 0;

    #endregion

    // Use this for initialization
    void Start() {
        Instance = this;
        ObjectiveMultiplier = 1;

        PossibleEvents = typeof(Trial).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Trial))).ToArray();
        //PossibleEvents = new Type[]{ typeof(TargetPractice) };
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
    }

    public void EventFinished()
    {
        CurrentTrial.Cleanup();
        Debug.Log("Trial Complete!");
    }
}

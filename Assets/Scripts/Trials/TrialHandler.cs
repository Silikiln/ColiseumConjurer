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

    static int _objectivesAdded = 0;
    public static int ObjectivesAdded
    {
        get { return _objectivesAdded > 0 ? _objectivesAdded : 0; }
        set { _objectivesAdded = value; }
    }

    public static float ObjectiveMultiplier { get; set; }

    static int _objectsAdded = 0;
    public static int ObjectsAdded
    {
        get { return _objectsAdded > 0 ? _objectsAdded : 0; }
        set { _objectsAdded = value; }
    }

    public static float ObjectMultiplier { get; set; }

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

using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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
        //PossibleEvents = new Type[]{ typeof(LightReflect) };
    }

    void Update()
    {
        if (CurrentTrial == null && Input.GetKeyDown(KeyCode.B))
            BeginEvent(RandomTrial);
    }

    public Trial CreateTrial(Type t) { return (Trial)Activator.CreateInstance(t); }

    //this could be better
    public void LoadEvent(Type t){gameObject.AddComponent(t);}
    public void BeginPreloadedEvent()
    {
        if (CurrentTrial != null){
            Debug.Log("Beginning Preloaded Trial: " + CurrentTrial.Name);
            CurrentTrial.Setup();
        }
    }

    public void BeginEvent(Type t)
    {
        if (CurrentTrial != null)
            CurrentTrial.Cleanup();
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

    public void loadTrialScene()
    {
        if (CurrentTrial != null)
        {
            //start the animation(couroutine?)
            //load scene as additive
            SceneManager.LoadSceneAsync("TrialScene", LoadSceneMode.Additive);
            GameController.Instance.mainCanvas.enabled = false;
        }
        else
            Debug.Log("No Specified Scene");
    }
}

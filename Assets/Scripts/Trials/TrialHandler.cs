using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class TrialHandler : MonoBehaviour {

    public static TrialHandler Instance { get; private set; }
    public static Type[] PossibleEvents { get; private set; }
    public Transform loadOnThis;
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

        /*
        PossibleEvents = typeof(Trial).Assembly.GetTypes().Where(type => 
            type != typeof(Boss) &&
            type.IsSubclassOf(typeof(Trial)) && 
            !type.IsSubclassOf(typeof(Boss))
        ).ToArray();
        */
        PossibleEvents = new Type[]{ typeof(Blimmy) };
    }

    void Update()
    {
        if (CurrentTrial == null && Input.GetKeyDown(KeyCode.B))
            BeginEvent(RandomTrial);
    }

    public Trial CreateTrial(Type t) { return (Trial)Activator.CreateInstance(t); }

    //this could be better
    public void LoadEvent(Type t)
    {
        gameObject.AddComponent(t);
    }
    public void BeginPreloadedEvent(Transform loadToMe)
    {
        loadOnThis = loadToMe;
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

    public void UnloadTrial()
    {
        if (CurrentTrial != null)
            Destroy(CurrentTrial);
    }

    public void TrialFailed()
    {
        CurrentTrial.Cleanup();
        Debug.Log("Trial Failed...");
        UnloadTrial();
    }

    public void TrialFinished()
    {
        CurrentTrial.Cleanup();
        Debug.Log("Trial Complete!");
        UnloadTrial();
    }

    public void LoadTrialScene()
    {
        SceneManager.LoadSceneAsync("TrialScene", LoadSceneMode.Additive);
    }

    public void UnloadTrialScene()
    {
        StartCoroutine(UnloadScene());
        GameController.Instance.mainCanvas.enabled = true;
    }
    
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.UnloadScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
    }
}

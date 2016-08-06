using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;
using UnityEngine.UI;

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

    public Transform loadOnThis;
    public Text timeText;
    public float TimeRemaining { get; private set; }
    public float PercentTimeRemaining { get { return TimeRemaining / CurrentTrial.TimeLimit; } }

    // Use this for initialization
    void Start() {
        Instance = this;
        ObjectiveMultiplier = 1;

        
        PossibleEvents = typeof(Trial).Assembly.GetTypes().Where(type => 
            type != typeof(Boss) &&
            type.IsSubclassOf(typeof(Trial)) && 
            !type.IsSubclassOf(typeof(Boss))
        ).ToArray();
        

        //PossibleEvents = new Type[]{ typeof(Corral) };
    }

    void Update()
    {
        if (CurrentTrial == null || CurrentTrial.CurrentState != Trial.TrialState.Active) return;

        if ((TimeRemaining -= Time.deltaTime) <= 0)
        {
            TimeRemaining = 0;
            TrialFailed("You ran out of time");
        }
        else if (timeText != null)
            timeText.text = string.Format("Time Left: {0:0.0}s", TimeRemaining);
    }

    //this could be better
    public void LoadTrial(Type t)
    {
        gameObject.AddComponent(t);
    }
    public void BeginTrial(Transform loadToMe, Text textOnMe)
    {
        loadOnThis = loadToMe;
        timeText = textOnMe;
        if (CurrentTrial != null){
            Debug.Log("Beginning Trial: " + CurrentTrial.Name);
            CurrentTrial.Setup();
            TimeRemaining = CurrentTrial.TimeLimit;
        }
    }

    public void PlayerKilled()
    {
        TrialFailed("You died");
    }

    public void TrialFailed(string reason = "")
    {
        if (CurrentTrial.CurrentState == Trial.TrialState.Ending) return;
        PlayerController.Lives--;
        CurrentTrial.Cleanup();
        Debug.Log("Trial Failed... " + reason);
    }

    public void TrialFinished()
    {
        if (CurrentTrial.CurrentState == Trial.TrialState.Ending) return;

        CurrentTrial.Cleanup();
        Debug.Log("Trial Complete!");
    }

    public void LoadTrialScene()
    {
        SceneManager.LoadSceneAsync("TrialScene", LoadSceneMode.Additive);
    }

    public void UnloadTrialScene()
    {
        StartCoroutine(UnloadScene());        
    }
    
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.UnloadScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
        GameController.Instance.mainCanvas.enabled = true;

        if (PlayerController.Lives <= 0)
            GameController.Instance.GameOver();

        if (CurrentTrial != null)
            Destroy(CurrentTrial);
    }
}

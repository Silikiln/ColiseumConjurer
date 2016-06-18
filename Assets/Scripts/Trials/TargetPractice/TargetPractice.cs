using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class TargetPractice : Trial
{
    static float MinimumDistance = 1;

    public TargetPractice()
    {
        Name = "Target Practice";
        Description = "Destroy the dummy";

        BaseObjectiveCount = 1;

        dummyPrefab = LoadResource<GameObject>("Dummy");
    }

    Vector2 lastPosition = PlayerController.Instance.transform.position;
    int objectivesComplete = 0;
    GameObject dummyPrefab;

    public void DummyKilled()
    {
        objectivesComplete++;
        if (RequirementsMet)
            TrialHandler.Instance.EventFinished();
        else
            Setup();
    }

    public override void Setup()
    {
        Instantiate(dummyPrefab, new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}

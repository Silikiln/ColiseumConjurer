using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class PointMove : Trial {
    static float MinimumDistance = 1;
    Vector2 lastPosition;
    public PointMove()
    {
        Name = "Move to Point";
        Description = "Move to the highlighted point(s)";

        BaseObjectiveCount = 1;
        pointPrefab = LoadResource<GameObject>("Point");
    }

    int objectivesComplete = 0;
    GameObject pointPrefab;

    public void PointReached()
    {
        objectivesComplete++;
        if (RequirementsMet)
            TrialHandler.Instance.TrialFinished();
        else
            CreatePoint();
    }

    void CreatePoint()
    {
        Vector2 position;
        do
        {
            position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        } while (Vector3.Distance(lastPosition, position) <= MinimumDistance);
        lastPosition = position;
        Instantiate(pointPrefab, position, Quaternion.identity);
    }

    public override void Setup()
    {
        lastPosition = PlayerController.Instance.transform.position;
        CreatePoint();
        base.Setup();
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}
